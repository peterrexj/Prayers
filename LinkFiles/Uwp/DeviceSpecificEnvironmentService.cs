using Prayers.Services;
using Prayers.UWP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
