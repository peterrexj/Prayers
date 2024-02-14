using Prayers.iOS.Services;
using Prayers.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppInformation))]
namespace Prayers.iOS.Services
{
    public class AppInformation : IAppInformation
    {
        public string AppCentreAppKey => "3ada4676-8728-4c94-b865-51423cb1190b";
    }
}