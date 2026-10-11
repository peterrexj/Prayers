using CatholicPrayers.Maui.Models;

namespace CatholicPrayers.Maui.Services;

public class ThemeHandler : IThemeHandler
{
    private readonly ILocalStorageService _storage;
    private const string ThemeKey = "AppThemeSettings";

    public AppThemes CurrentTheme { get; private set; } = AppThemes.Dark;

    public ThemeHandler(ILocalStorageService storage)
    {
        _storage = storage;
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public void LoadDefaultStyle()
    {
        var theme = Task.Run(LoadSavedThemeAsync).GetAwaiter().GetResult();
        LoadDefaultStyle(theme);
    }

    public void LoadDefaultStyle(AppThemes appTheme)
    {
        CurrentTheme = appTheme;
        try
        {
            var common = LoadResourceDictionary("Theme_Styles_Common.xaml");
            var themed = LoadResourceDictionary(ThemeFileName(appTheme));

            var app = Application.Current;
            if (app == null) return;

            // Remove all previously-loaded theme dictionaries
            ClearThemeDictionaries(app);

            // Remove all PrayApp* keys from the root dictionary so the
            // null→value cycle below is the only source of truth
            ClearPrefixedKeys(app, "PrayApp");

            // Add the fresh dictionaries
            if (common != null) app.Resources.MergedDictionaries.Add(common);
            if (themed != null) app.Resources.MergedDictionaries.Add(themed);

            // Force every DynamicResource binding to re-evaluate by doing a
            // null→value cycle on every PrayApp* key now in scope
            ForceRebind(app, "PrayApp");

            // Restore font choice — CommonStyles resets it to "Lato"
            var savedFont = SharedServices.AppSettings.FontFamily;
            if (!string.IsNullOrEmpty(savedFont))
                app.Resources["PrayAppDefaultFontFamily"] = savedFont;

            UpdateStatusBar(appTheme);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ThemeHandler] LoadDefaultStyle failed: {ex.Message}");
        }
    }

    public async Task LoadDefaultStyleAsync(AppThemes? theme = null)
    {
        var target = theme ?? await LoadSavedThemeAsync();
        await MainThread.InvokeOnMainThreadAsync(() => LoadDefaultStyle(target));
    }

    public async Task SaveThemeAsync(AppThemes theme)
    {
        CurrentTheme = theme;
        await _storage.SaveAsync(ThemeKey, new ThemeSettings { Theme = theme });
        Preferences.Set("AppThemeBg", GetThemeBgHex(theme));
        await MainThread.InvokeOnMainThreadAsync(() => LoadDefaultStyle(theme));
    }

    public async Task<AppThemes> LoadSavedThemeAsync()
    {
        var saved = await _storage.LoadAsync<ThemeSettings>(ThemeKey);
        return saved?.Theme ?? AppThemes.Dark;
    }

    // ── Resource loading ──────────────────────────────────────────────────────

    private static ResourceDictionary? LoadResourceDictionary(string resourceFileName)
    {
        try
        {
            var assembly = typeof(ThemeHandler).Assembly;
            var resourceName = $"CatholicPrayers.Maui.Resources.Styles.{resourceFileName}";
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                System.Diagnostics.Debug.WriteLine($"[ThemeHandler] Resource not found: {resourceName}");
                return null;
            }
            using var reader = new StreamReader(stream);
            var xaml = reader.ReadToEnd();
            var dict = new ResourceDictionary();
            dict.LoadFromXaml(xaml);
            return dict;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ThemeHandler] LoadResourceDictionary({resourceFileName}) failed: {ex.Message}");
            return null;
        }
    }

    // ── Resource management ───────────────────────────────────────────────────

    // Remove any MergedDictionary that contains PrayApp* keys (our theme dicts)
    private static void ClearThemeDictionaries(Application app)
    {
        var toRemove = app.Resources.MergedDictionaries
            .Where(d => d.Keys.Cast<string>().Any(k => k.StartsWith("PrayApp")))
            .ToList();
        foreach (var d in toRemove)
            app.Resources.MergedDictionaries.Remove(d);
    }

    // Remove all PrayApp* keys from the root resource dictionary
    private static void ClearPrefixedKeys(Application app, string prefix)
    {
        var keys = app.Resources.Keys.Cast<string>()
            .Where(k => k.StartsWith(prefix))
            .ToList();
        foreach (var k in keys)
        {
            app.Resources[k] = null;
            app.Resources.Remove(k);
        }
    }

    // Force DynamicResource bindings to re-evaluate: null then restore each key
    private static void ForceRebind(Application app, string prefix)
    {
        // Collect all PrayApp* keys now in scope (root + merged)
        var allKeys = new HashSet<string>();

        foreach (var k in app.Resources.Keys.Cast<string>().Where(k => k.StartsWith(prefix)))
            allKeys.Add(k);

        foreach (var d in app.Resources.MergedDictionaries)
            foreach (var k in d.Keys.Cast<string>().Where(k => k.StartsWith(prefix)))
                allKeys.Add(k);

        foreach (var key in allKeys)
        {
            app.Resources.TryGetValue(key, out var value);
            app.Resources[key] = null;
            app.Resources[key] = value;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string ThemeFileName(AppThemes theme) => theme switch
    {
        AppThemes.Dark    => "Theme_Dark.xaml",
        AppThemes.Light   => "Theme_Light.xaml",
        AppThemes.Forest  => "Theme_Forest.xaml",
        AppThemes.Candy   => "Theme_Candy.xaml",
        AppThemes.Sunset  => "Theme_Sunset.xaml",
        AppThemes.Ocean   => "Theme_Ocean.xaml",
        _                 => "Theme_Dark.xaml"
    };

    private static void UpdateStatusBar(AppThemes theme)
    {
        try
        {
            var hex = GetThemeBgHex(theme);
            var color = Color.FromArgb(hex);
            bool isDark = color.GetLuminosity() < 0.5f;

#if ANDROID
            var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            if (activity?.Window != null)
            {
                var androidColor = Android.Graphics.Color.ParseColor(hex);
                activity.Window.SetStatusBarColor(androidColor);
                var decorView = activity.Window.DecorView;
                if (decorView != null)
                {
                    var flags = (Android.Views.SystemUiFlags)decorView.SystemUiVisibility;
                    if (isDark)
                        flags &= ~Android.Views.SystemUiFlags.LightStatusBar;
                    else
                        flags |= Android.Views.SystemUiFlags.LightStatusBar;
                    decorView.SystemUiVisibility = (Android.Views.StatusBarVisibility)flags;
                }
            }
#elif IOS || MACCATALYST
            var uiColor = UIKit.UIColor.FromRGB(
                (nfloat)color.Red, (nfloat)color.Green, (nfloat)color.Blue);
            foreach (var scene in UIKit.UIApplication.SharedApplication.ConnectedScenes)
            {
                if (scene is UIKit.UIWindowScene ws)
                    foreach (var win in ws.Windows)
                        win.BackgroundColor = uiColor;
            }
#endif
        }
        catch { }
    }

    public static string GetThemeBgHex(AppThemes theme) => theme switch
    {
        AppThemes.Dark    => "#0D1B2A",
        AppThemes.Light   => "#FAF5FF",
        AppThemes.Forest  => "#0D2818",
        AppThemes.Candy   => "#FFF0F7",
        AppThemes.Sunset  => "#FFF7ED",
        AppThemes.Ocean   => "#EFF6FF",
        _                 => "#0D1B2A"
    };
}
