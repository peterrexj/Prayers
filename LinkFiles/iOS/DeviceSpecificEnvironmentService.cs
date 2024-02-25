using Prayers.iOS.Services;
using Prayers.Services;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceSpecificEnvironmentService))]
namespace Prayers.iOS.Services
{
    public class DeviceSpecificEnvironmentService : IDeviceSpecificEnvironment
    {
        public void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint)
        {
        }
    }
}