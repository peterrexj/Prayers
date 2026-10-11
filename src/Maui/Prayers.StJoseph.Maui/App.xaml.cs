using Prayers.StJoseph.Maui.Services;
using Prayers.StJoseph.Maui.ViewModels;
using Syncfusion.Licensing;

namespace Prayers.StJoseph.Maui;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider, IThemeService themeService, SettingsViewModel settings)
    {
        InitializeComponent();

        SyncfusionLicenseProvider.RegisterLicense(
            "Ngo9BigBOggjHTQxAR8/V1JHaF5cWWdCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWXpfd3RQR2VZUUFwWERWYEo=");

        // Restore persisted settings (theme, font size) into the singleton before any view renders
        Task.Run(async () =>
        {
            var storage = ServiceLocator.GetService<ILocalStorageService>();
            if (storage != null)
            {
                var saved = await storage.GetData<SettingsViewModel>();
                if (saved != null)
                {
                    if (!string.IsNullOrEmpty(saved.SelectedTheme))
                        settings.RestoreTheme(saved.SelectedTheme);
                    if (saved.CurrentFontSize != 0)
                        settings.RestoreFontSize(saved.CurrentFontSize);
                }
            }
        }).Wait();

        var style = themeService.GetDefaultStyleTheme();
        themeService.UpdateAppThemes(style);

        // Kick off data load — PrayerView.OnAppearing awaits it before rendering
        _ = SharedServices.LoadPrayerDataAsync();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
