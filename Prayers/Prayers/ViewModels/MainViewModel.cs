using Newtonsoft.Json;
using Prayers.Services;
using Prayers.ViewModels.Extras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Prayers.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand btnStartCommand { get; set; }

        private string _prayerMainHeader01;
        public string PrayerMainHeader01
        {
            get { return _prayerMainHeader01; }
            set 
            { 
                _prayerMainHeader01 = value;
                OnPropertyChanged("PrayerMainHeader01");
            }
        }

        private string _prayerMainHeader02;
        public string PrayerMainHeader02
        {
            get { return _prayerMainHeader02; }
            set
            {
                _prayerMainHeader02 = value;
                OnPropertyChanged("PrayerMainHeader02");
            }
        }

        public string PathToMainImage => SharedServices.PathToMainImage;

        public MainViewModel()
        {
            btnStartCommand = new Command(async () => await ProcessRequestToPrayerPage());
        }

        private async Task ProcessRequestToPrayerPage()
        {
            var route = $"Page1?PageId=1";
            await Shell.Current.GoToAsync(route);
        }

        [JsonIgnore]
        private StyleModelDefault styleModelDefault;
        [JsonIgnore]
        public StyleModelDefault DefaultStyle
        {
            get => styleModelDefault;
            set
            {
                styleModelDefault = value;
                OnPropertyChanged("DefaultStyle");
            }
        }
    }
}
