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
