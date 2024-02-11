using Newtonsoft.Json;
using Prayers.ViewModels.Extras;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prayers.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
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
