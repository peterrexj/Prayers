using Prayers.Services;
using Prayers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prayers.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        MainViewModel viewModel;
        public MainView()
        {
            InitializeComponent();
            viewModel = new MainViewModel
            {

            };

            if (SharedServices.PrayerViewModelData.MainHeaders.Count > 0)
            {
                viewModel.PrayerMainHeader01 = SharedServices.PrayerViewModelData.MainHeaders.First();
            }
            if (SharedServices.PrayerViewModelData.MainHeaders.Count > 1)
            {
                viewModel.PrayerMainHeader02 = SharedServices.PrayerViewModelData.MainHeaders.Skip(1).First();
            }

            BindingContext = viewModel;
        }
    }
}