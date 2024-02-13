using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Prayers.Droid.Services;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using AndroidX.AppCompat.App;
using Android.Content;

namespace Prayers.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        async void SimulateStartup()
        {
            //await Task.Delay(3000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}