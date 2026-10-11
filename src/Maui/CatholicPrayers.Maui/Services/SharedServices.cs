using CatholicPrayers.Maui.Models;

namespace CatholicPrayers.Maui.Services;

public static class SharedServices
{
    public static IPrayerService PrayerService
        => ServiceLocator.GetRequired<IPrayerService>();

    public static IThemeHandler ThemeHandler
        => ServiceLocator.GetRequired<IThemeHandler>();

    public static ILocalStorageService LocalStorage
        => ServiceLocator.GetRequired<ILocalStorageService>();

    public static AppSettings AppSettings { get; set; } = new();
}
