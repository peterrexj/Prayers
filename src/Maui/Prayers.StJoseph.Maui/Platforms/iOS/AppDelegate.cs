using Foundation;
using Prayers.StJoseph.Maui.Services;
using Prayers.StJoseph.Maui.ViewModels;
using UIKit;

namespace Prayers.StJoseph.Maui;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        var result = base.FinishedLaunching(app, options);

        // Apply saved theme's page background to the native window so the
        // status-bar area and home-indicator area match the theme color.
        UIApplication.SharedApplication.InvokeOnMainThread(() =>
        {
            var settings = ServiceLocator.GetService<SettingsViewModel>();
            var themeService = ServiceLocator.GetService<IThemeService>();
            if (settings == null || themeService == null) return;
            var style = themeService.GetDefaultStyleTheme(settings.SelectedAppTheme);
            ThemeService.UpdateWindowBackground(style.PageBgColor);
        });

        return result;
    }
}
