using Prayers.StJoseph.Maui.Services;

namespace Prayers.StJoseph.Maui.Platforms.Android.DependencyInjection;

public class AppInformationService : IAppInformationService
{
    public string PrayerEmbeddedDataFilePath => "Data/St Joseph Prayer.csv";
    public string AppCentreAppKey => "28ecb3c8-c7d9-4453-8da2-f17d93438eb6";
}
