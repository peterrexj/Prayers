using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;
using Prayers.Extensions;
using Prayers.Services;
using Prayers.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prayers
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            var appCentreKey = DependencyService.Get<IAppInformation>().AppCentreAppKey;

            if (!AppCenter.Configured)
            {
                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                {
                    AppCenter.Start($"android={appCentreKey};", typeof(Analytics), typeof(Crashes));
                }
                else if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {
                    AppCenter.Start($"ios={appCentreKey};", typeof(Analytics), typeof(Crashes));
                }
                else if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.UWP)
                {
                    AppCenter.Start($"uwp={appCentreKey};", typeof(Analytics), typeof(Crashes));
                }
                else
                {
                    AppCenter.Start($"android={appCentreKey};"
                          //  +
                          //"uwp={Your UWP App secret here};" +
                          //"ios={Your iOS App secret here};"
                          //"macos={Your macOS App secret here};"
                          ,
                          typeof(Analytics), typeof(Crashes));
                }
            }

            ThemeHelper.UpdateAppThemes();
            SharedServices.LoadPrayerViewModelData();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
