using Prayers.Extensions;
using Prayers.Models;
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
                PrayerMainTitle = SharedServices.PrayerViewModelData.MainTitle
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

        protected override void OnAppearing()
        {
            viewModel.DefaultStyle = ThemeHelper.GetDefaultStyleTheme();

            base.OnAppearing();
        }

        private async void SfEffectsView_AnimationCompleted(object sender, EventArgs e)
        {
            await ProcessRequestToPrayerPage();
        }

        private async Task ProcessRequestToPrayerPage()
        {
            var route = $"Page1?PageId=1";
            await Shell.Current.GoToAsync(route);
        }
    }
}