using Prayers.Services;
using Prayers.StJoseph.iOS.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppInformation))]
namespace Prayers.StJoseph.iOS.Services
{
    public class AppInformation : IAppInformation
    {
        public string AppCentreAppKey => "3ada4676-8728-4c94-b865-51423cb1190b";
        public string PrayerEmbeddedDataFilePath => "St Joseph Prayer.csv";
    }
}