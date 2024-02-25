using Android.OS;
using Android.Views;
using Microsoft.AppCenter.Crashes;
using Prayers.Droid.Services;
using Prayers.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceSpecificEnvironmentService))]
namespace Prayers.Droid.Services
{
    public class DeviceSpecificEnvironmentService : IDeviceSpecificEnvironment
    {
        public void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint)
        {
            try
            {
                if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                    return;

                var activity = Platform.CurrentActivity;
                var window = activity.Window;
                window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                window.SetStatusBarColor(color.ToPlatformColor());

                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    var flag = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
                    window.DecorView.SystemUiVisibility = darkStatusBarTint ? flag : 0;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}