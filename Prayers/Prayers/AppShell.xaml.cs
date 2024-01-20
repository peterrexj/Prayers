using Prayers.Services;
using Prayers.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Prayers
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(MainView), typeof(MainView));
            Routing.RegisterRoute(nameof(PrayerView), typeof(PrayerView));
            foreach (var item in SharedServices.PrayerViewModelData.SinglePageDataModels)
            {
                Routing.RegisterRoute($"Page{item.PageId}", typeof(PrayerView));
            }
        }

    }
}
