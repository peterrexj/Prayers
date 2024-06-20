using Prayers.Services;
using Prayers.UWP.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceSpecificEnvironmentService))]
namespace Prayers.UWP.Services
{
    public class DeviceSpecificEnvironmentService : IDeviceSpecificEnvironment
    {
        public void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint)
        {
        }
    }
}
