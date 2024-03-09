using Prayers.Services;
using Prayers.UWP.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppInformation))]
namespace Prayers.UWP.Services
{
    public class AppInformation : IAppInformation
    {
        public string AppCentreAppKey => "ff232829-dd96-47be-96c5-5baf610fdf4e";
        public string PrayerEmbeddedDataFilePath => "St Joseph Prayer.csv";
    }
}