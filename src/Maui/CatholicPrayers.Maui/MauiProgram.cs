using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using CatholicPrayers.Maui.Services;
using CatholicPrayers.Maui.ViewModels;
using CatholicPrayers.Maui.Views;

namespace CatholicPrayers.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            System.Diagnostics.Debug.WriteLine($"[UnhandledException] {e.ExceptionObject}");

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            System.Diagnostics.Debug.WriteLine($"[UnobservedTask] {e.Exception?.GetBaseException().Message}");
            e.SetObserved();
        };

        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Lato-Regular.ttf", "Lato");
                fonts.AddFont("Nunito-Regular.ttf", "Nunito");
                fonts.AddFont("Quicksand-Regular.ttf", "Quicksand");
                fonts.AddFont("Raleway-Regular.ttf", "Raleway");
                fonts.AddFont("Merriweather-Regular.ttf", "Merriweather");
                fonts.AddFont("PlayfairDisplay-Regular.ttf", "PlayfairDisplay");
                fonts.AddFont("SourceSerif4-Regular.ttf", "SourceSerif4");
                fonts.AddFont("Pacifico-Regular.ttf", "Pacifico");
                fonts.AddFont("FontAwesome5Solid.otf", "FontAwesome5Solid");
            });

        // ── Services ──────────────────────────────────────────────────────────
        builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
        builder.Services.AddSingleton<IPrayerService, PrayerService>();
        builder.Services.AddSingleton<IThemeHandler, ThemeHandler>();
        builder.Services.AddSingleton<IFavouriteService, FavouriteService>();
        builder.Services.AddSingleton<IBibleService, BibleService>();

        // ── ViewModels ────────────────────────────────────────────────────────
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddTransient<PrayerViewModel>();
        builder.Services.AddTransient<CollectionViewModel>();
        builder.Services.AddTransient<LinkDetailViewModel>();
        builder.Services.AddTransient<PartnerAppViewModel>();
        builder.Services.AddTransient<BibleViewModel>();
        builder.Services.AddTransient<BookViewModel>();

        // ── Views ─────────────────────────────────────────────────────────────
        builder.Services.AddSingleton<DashboardPage>();
        builder.Services.AddTransient<PrayerPage>();
        builder.Services.AddTransient<CollectionPage>();
        builder.Services.AddTransient<LinkDetailPage>();
        builder.Services.AddTransient<PartnerAppPage>();
        builder.Services.AddTransient<BiblePage>();
        builder.Services.AddTransient<BookPage>();
        builder.Services.AddTransient<SettingsPage>();

        // ── App ───────────────────────────────────────────────────────────────
        builder.Services.AddSingleton<App>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Wire service locator so AppShell and static helpers can resolve services
        ServiceLocator.Init(app.Services);

        return app;
    }
}
