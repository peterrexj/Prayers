using Prayers.Services;
using Prayers.StJoseph.Droid.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppInformation))]
namespace Prayers.StJoseph.Droid.Services
{
    public class AppInformation : IAppInformation
    {
        public string AppCentreAppKey => "28ecb3c8-c7d9-4453-8da2-f17d93438eb6";
        public string PrayerEmbeddedDataFilePath => "St Joseph Prayer.csv";
    }
}