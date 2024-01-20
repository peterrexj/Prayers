using Newtonsoft.Json;
using Prayers.Extensions;
using Prayers.Models;
using Prayers.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Prayers.ViewModels.Extras
{
    public class SettingsViewModel : BaseViewModel
    {
        private bool isUpdating = false;

        public SettingsViewModel()
        {
            themes = new ObservableCollection<string>(Enum.GetNames(typeof(AppThemes)));
        }

        private string selectedTheme;
        public string SelectedTheme
        {
            get
            {
                return selectedTheme;
            }
            set
            {
                if (isUpdating == false)
                {
                    isUpdating = true;
                    selectedTheme = value;
                    OnPropertyChanged("SelectedTheme");
                    SaveData();
                    DefaultStyle = ThemeHelper.GetDefaultStyleTheme(SelectedAppTheme);
                    ThemeHelper.UpdateAppThemes(DefaultStyle);
                    OnPropertyChanged("DefaultStyle");
                    isUpdating = false;
                }
            }
        }
        public AppThemes SelectedAppTheme => EnumHelper<AppThemes>.FromString(selectedTheme);

        #region Commands


        #endregion


        [JsonIgnore]
        private ObservableCollection<string> themes;
        [JsonIgnore]
        public ObservableCollection<string> Themes
        {
            get
            {
                return themes;
            }
            set
            {
                Themes = value;
                OnPropertyChanged("Themes");
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
                OnPropertyChanged(nameof(StyleModelDefault));
            }
        }

        private void SaveData()
        {
            Task.Run(async () =>
            {
                await SharedServices.LocalStorage.SaveData(this);
            }).Wait();
        }
    }
}
