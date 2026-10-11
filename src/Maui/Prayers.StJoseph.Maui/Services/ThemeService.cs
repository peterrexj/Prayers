using Prayers.StJoseph.Maui.Models;
using Prayers.StJoseph.Maui.ViewModels;

namespace Prayers.StJoseph.Maui.Services;

public class ThemeService : IThemeService
{
    private readonly SettingsViewModel _settings;

    public ThemeService(SettingsViewModel settings)
    {
        _settings = settings;
    }

    public StyleModelDefault GetDefaultStyleTheme()
        => GetDefaultStyleTheme(_settings.SelectedAppTheme);

    public StyleModelDefault GetDefaultStyleTheme(AppThemes appTheme)
    {
        return appTheme switch
        {
            AppThemes.Dark => new StyleModelDefault
            {
                AppTheme = appTheme,
                PageBgColor = "#1B2631",
                DefaultPageBgGradient = "linear-gradient(90deg, #1b1b1b 0%, #010101 100%)",
                BoxFontColor = "#D4EFDF",
                BoxBgGradient = "linear-gradient(90deg, #3C220B 0%, #3C870F 100%)",
                ButtonFgColor = "#D4EFDF",
                ButtonBgColor = "#0B5345",
                ThemeButtonBgColor = "#EAEDED",
                ProgressBarTrackColor = "#4A5568",
                ProgressBarInProgressMarkerFillColor = "#1A6B55",
                ProgressBarInProgressMarkerStrokeColor = "#D4EFDF",
                ProgressBarCompletedBgColor = "#0B5345",
                ProgressBarIconMainBgColor = "#3D5166",
                DefaultFontColor = "#BFC9CA",
                AppShellBgColor = "#1B2631",
                AppShellFgColor = "#B3B6B7",
                AppShellTitleColor = "",
                AppShellDisabledColor = "",
                AppShellUnselectedColor = "",
                AppShellTabBarBackgroundColor = "#1B2631",
                AppShellTabBarForegroundColor = "#B3B6B7",
                AppShellTabBarUnselectedColor = "",
                AppShellTabBarDisabledColor = "",
                AppShellTabBarTitleColor = "",
                AppShellBackgroundColor = "#447e58",
            },
            AppThemes.Light => new StyleModelDefault
            {
                AppTheme = appTheme,
                PageBgColor = "#FFFFFF",
                DefaultPageBgGradient = "linear-gradient(45deg, #FFFFFF 0%, #F0F0F0 100%)",
                BoxFontColor = "#FFFFFF",
                BoxBgGradient = "linear-gradient(135deg, #13B0DF44 0%, #036F5F44 100%)",
                ButtonFgColor = "#FBFCFC",
                ButtonBgColor = "#1A5276",
                ThemeButtonBgColor = "#2E4053",
                ProgressBarTrackColor = "#AEB6BF",
                ProgressBarInProgressMarkerFillColor = "#1A5276",
                ProgressBarInProgressMarkerStrokeColor = "#1A5276",
                ProgressBarCompletedBgColor = "#1A5276",
                ProgressBarIconMainBgColor = "#D5D8DC",
                DefaultFontColor = "#1B2631",
                AppShellBgColor = "#B3B6B7",
                AppShellFgColor = "#283747",
                AppShellTitleColor = "#283747",
                AppShellDisabledColor = "#979A9A",
                AppShellUnselectedColor = "#5D6D7E",
                AppShellTabBarBackgroundColor = "#B3B6B7",
                AppShellTabBarForegroundColor = "#283747",
                AppShellTabBarUnselectedColor = "#5D6D7E",
                AppShellTabBarDisabledColor = "#979A9A",
                AppShellTabBarTitleColor = "#21618C",
                AppShellBackgroundColor = "#3091c4",
            },
            _ => GetDefaultStyleTheme(AppThemes.Dark),
        };
    }

    public void UpdateAppThemes(StyleModelDefault? styleModel)
    {
        if (styleModel == null) return;

        var res = Application.Current?.Resources;
        if (res == null) return;

        void Set(string key, string? value)
        {
            if (!string.IsNullOrEmpty(value) && res.ContainsKey(key))
                res[key] = Color.FromArgb(value);
        }

        Set("AppShellBgColor", styleModel.AppShellBgColor);
        Set("AppShellFgColor", styleModel.AppShellFgColor);
        Set("AppShellTitleColor", styleModel.AppShellTitleColor);
        Set("AppShellDisabledColor", styleModel.AppShellDisabledColor);
        Set("AppShellUnselectedColor", styleModel.AppShellUnselectedColor);
        Set("AppShellTabBarBackgroundColor", styleModel.AppShellTabBarBackgroundColor);
        Set("AppShellTabBarForegroundColor", styleModel.AppShellTabBarForegroundColor);
        Set("AppShellTabBarUnselectedColor", styleModel.AppShellTabBarUnselectedColor);
        Set("AppShellTabBarDisabledColor", styleModel.AppShellTabBarDisabledColor);
        Set("AppShellTabBarTitleColor", styleModel.AppShellTabBarTitleColor);
        Set("AppShellBackgroundColor", styleModel.AppShellBackgroundColor);

        UpdateWindowBackground(styleModel.PageBgColor);
    }

    // Updates the native iOS window background so status bar and home-indicator areas match the theme
    public static void UpdateWindowBackground(string? hexColor)
    {
#if IOS || MACCATALYST
        if (string.IsNullOrEmpty(hexColor)) return;
        var color = Color.FromArgb(hexColor);
        var uiColor = UIKit.UIColor.FromRGB(
            (nfloat)color.Red,
            (nfloat)color.Green,
            (nfloat)color.Blue);
        MainThread.BeginInvokeOnMainThread(() =>
        {
            foreach (var scene in UIKit.UIApplication.SharedApplication.ConnectedScenes)
            {
                if (scene is UIKit.UIWindowScene ws)
                    foreach (var win in ws.Windows)
                        win.BackgroundColor = uiColor;
            }
        });
#endif
    }
}
