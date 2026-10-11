using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Prayers.StJoseph.Maui.Services;
using Prayers.StJoseph.Maui.ViewModels;
using Prayers.StJoseph.Maui.Views;
using Syncfusion.Maui.Core.Hosting;

namespace Prayers.StJoseph.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("FontAwesome5Solid.otf", "FontAwesome5Solid");
                fonts.AddFont("Kartika.ttf", "Kartika");
            });

        // Singletons
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<IThemeService, ThemeService>();
        builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
        builder.Services.AddSingleton<MainView>();
        builder.Services.AddSingleton<App>();

        // Transients
        builder.Services.AddTransient<PrayerView>();

        // Platform-specific
#if ANDROID
        builder.Services.AddSingleton<IAppInformationService,
            Prayers.StJoseph.Maui.Platforms.Android.DependencyInjection.AppInformationService>();
        builder.Services.AddSingleton<IAudioService,
            Prayers.StJoseph.Maui.Platforms.Android.DependencyInjection.AudioService>();
#elif IOS || MACCATALYST
        builder.Services.AddSingleton<IAppInformationService,
            Prayers.StJoseph.Maui.Platforms.iOS.AppInformationService>();
        builder.Services.AddSingleton<IAudioService,
            Prayers.StJoseph.Maui.Platforms.iOS.AudioService>();
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        ServiceLocator.ServiceProvider = app.Services;
        return app;
    }
}
