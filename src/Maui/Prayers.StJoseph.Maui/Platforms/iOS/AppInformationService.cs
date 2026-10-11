using Prayers.StJoseph.Maui.Services;

namespace Prayers.StJoseph.Maui.Platforms.iOS;

public class AppInformationService : IAppInformationService
{
    public string PrayerEmbeddedDataFilePath => "Data/St Joseph Prayer.csv";
    public string AppCentreAppKey => "3ada4676-8728-4c94-b865-51423cb1190b";
}
