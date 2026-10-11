using System.Collections.ObjectModel;
using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly IThemeHandler _themeHandler;
    private readonly ILocalStorageService _storage;
    private readonly IPrayerService _prayerService;

    private string _selectedTheme = AppThemes.Dark.ToString();
    private string _selectedFont = "Lato";
    private bool _isUpdatingTheme;
    private double _fontSizeDelta;
    private bool _showDivineAnimations = true;
    private string _selectedLanguage = "en";

    public ObservableCollection<string> Themes { get; } = new(Enum.GetNames<AppThemes>());
    public ObservableCollection<string> FontFamilies { get; } = new(RegisteredFonts.UserSelectableFamilies);

    public ObservableCollection<string> Languages { get; } = new(["English", "Latin", "Español"]);
    private static readonly string[] LanguageCodes = ["en", "la", "es"];

    public string SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            if (value == null || _isUpdatingTheme || value == _selectedTheme) return;
            _selectedTheme = value;
            OnPropertyChanged();
            if (IsBusy) return;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                _isUpdatingTheme = true;
                IsBusy = true;
                try
                {
                    await Task.Delay(100); // let the picker event fully unwind
                    await ChangeThemeAsync(value);
                }
                finally
                {
                    IsBusy = false;
                    _isUpdatingTheme = false;
                }
            });
        }
    }

    public string SelectedFont
    {
        get => _selectedFont;
        set
        {
            if (SetProperty(ref _selectedFont, value) && !IsBusy)
                SaveFontPreference(value);
        }
    }

    public double FontSizeDelta
    {
        get => _fontSizeDelta;
        set
        {
            if (SetProperty(ref _fontSizeDelta, Math.Clamp(value, -2, 8)))
            {
                OnPropertyChanged(nameof(PreviewFontSize));
                if (!IsBusy)
                    SaveFontSizePreference(_fontSizeDelta);
            }
        }
    }

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (SetProperty(ref _selectedLanguage, value) && !IsBusy)
                SaveLanguagePreference(value);
        }
    }

    public int SelectedLanguageIndex
    {
        get
        {
            var idx = Array.IndexOf(LanguageCodes, _selectedLanguage);
            return idx < 0 ? 0 : idx;
        }
        set
        {
            if (value >= 0 && value < LanguageCodes.Length)
                SelectedLanguage = LanguageCodes[value];
        }
    }

    public bool ShowDivineAnimations
    {
        get => _showDivineAnimations;
        set
        {
            if (SetProperty(ref _showDivineAnimations, value) && !IsBusy)
                SaveAnimationPreference(value);
        }
    }

    public double PreviewFontSize => 16 + _fontSizeDelta;

    public int SelectedThemeIndex
    {
        get
        {
            var idx = Themes.IndexOf(_selectedTheme);
            return idx < 0 ? 0 : idx;
        }
        set
        {
            if (value >= 0 && value < Themes.Count)
                SelectedTheme = Themes[value];
        }
    }

    public string AppVersion => AppInfo.VersionString;
    public string AppBuild => AppInfo.BuildString;

    public Command IncreaseFontCommand { get; }
    public Command DecreaseFontCommand { get; }

    public SettingsViewModel(IThemeHandler themeHandler, ILocalStorageService storage, IPrayerService prayerService)
    {
        _themeHandler = themeHandler;
        _storage = storage;
        _prayerService = prayerService;
        IncreaseFontCommand = new Command(() => FontSizeDelta += 1);
        DecreaseFontCommand = new Command(() => FontSizeDelta -= 1);
    }

    public async Task RestoreAsync()
    {
        IsBusy = true;
        try
        {
            var settings = await _storage.LoadAsync<AppSettings>("AppSettings") ?? new AppSettings();
            _selectedTheme = settings.Theme.ToString();
            _selectedFont = settings.FontFamily;
            _fontSizeDelta = settings.FontSizeDelta;
            _showDivineAnimations = settings.ShowDivineAnimations;
            _selectedLanguage = settings.ContentLanguage ?? "en";

            OnPropertyChanged(nameof(SelectedTheme));
            OnPropertyChanged(nameof(SelectedFont));
            OnPropertyChanged(nameof(FontSizeDelta));
            OnPropertyChanged(nameof(ShowDivineAnimations));
            OnPropertyChanged(nameof(SelectedLanguage));
            OnPropertyChanged(nameof(SelectedLanguageIndex));

            SharedServices.AppSettings = settings;
            ApplyFontToResources(_selectedFont);
        }
        finally { IsBusy = false; }
    }

    private async Task ChangeThemeAsync(string themeName)
    {
        try
        {
            if (!Enum.TryParse<AppThemes>(themeName, out var theme)) return;
            _themeHandler.LoadDefaultStyle(theme);
            SharedServices.AppSettings.Theme = theme;
            await _storage.SaveAsync("AppThemeSettings", new ThemeSettings { Theme = theme });
            Preferences.Set("AppThemeBg", ThemeHandler.GetThemeBgHex(theme));
            await PersistSettingsAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Settings] ChangeThemeAsync failed: {ex.Message}");
        }
    }

    private void SaveFontPreference(string font)
    {
        SharedServices.AppSettings.FontFamily = font;
        ApplyFontToResources(font);
        _ = PersistSettingsAsync();
    }

    private void SaveFontSizePreference(double delta)
    {
        SharedServices.AppSettings.FontSizeDelta = delta;
        _ = PersistSettingsAsync();
    }

    private void SaveAnimationPreference(bool enabled)
    {
        SharedServices.AppSettings.ShowDivineAnimations = enabled;
        _ = PersistSettingsAsync();
    }

    private void SaveLanguagePreference(string langCode)
    {
        SharedServices.AppSettings.ContentLanguage = langCode;
        _prayerService.ClearPrayerContentCache();
        _ = PersistSettingsAsync();
    }

    private static void ApplyFontToResources(string fontFamily)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (Application.Current?.Resources != null)
                Application.Current.Resources["PrayAppDefaultFontFamily"] = fontFamily;
        });
    }

    private async Task PersistSettingsAsync()
    {
        try { await _storage.SaveAsync("AppSettings", SharedServices.AppSettings); }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[Settings] PersistAsync failed: {ex.Message}"); }
    }
}
