using Prayers.Services;
using Prayers.StJoseph.Uwp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppInformation))]
namespace Prayers.StJoseph.Uwp.Services
{
    public class AppInformation : IAppInformation
    {
        public string AppCentreAppKey => "ff232829-dd96-47be-96c5-5baf610fdf4e";
        public string PrayerEmbeddedDataFilePath => "St Joseph Prayer.csv";
    }
}