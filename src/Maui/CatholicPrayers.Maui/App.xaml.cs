using CatholicPrayers.Maui.Services;
using CatholicPrayers.Maui.ViewModels;
using Syncfusion.Licensing;

namespace CatholicPrayers.Maui;

public partial class App : Application
{
    private readonly IThemeHandler _themeHandler;
    private readonly SettingsViewModel _settings;

    public App(IThemeHandler themeHandler, SettingsViewModel settings)
    {
        _themeHandler = themeHandler;
        _settings = settings;

        SyncfusionLicenseProvider.RegisterLicense(
            "Ngo9BigBOggjHTQxAR8/V1JHaF5cWWdCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWXpfd3RQR2VZUUFwWERWYEo=");

        InitializeComponent();

        // Apply default Dark theme synchronously so first frame is correct
        _ = InitAsync();
    }

    private async Task InitAsync()
    {
        try
        {
            await _settings.RestoreAsync();
            await MainThread.InvokeOnMainThreadAsync(
                () => _themeHandler.LoadDefaultStyle(SharedServices.AppSettings.Theme));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[App] InitAsync failed: {ex.Message}");
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
        => new Window(new AppShell());
}
