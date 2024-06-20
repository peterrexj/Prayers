using Prayers.Droid.Services;
using Prayers.ServiceDefinitions;
using Xamarin.Forms;

[assembly: Dependency(typeof(ConfigHelper))]
namespace Prayers.Droid.Services
{
    public class ConfigHelper : IConfigService
    {
        public string SyncFusionLicense => "Ngo9BigBOggjHTQxAR8/V1NHaF5cXmpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH9feHRcR2lYWEdzW0M=";
    }
}