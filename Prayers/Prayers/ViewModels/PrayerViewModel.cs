using FontAwesome;
using Newtonsoft.Json;
using Pj.Library;
using Prayers.Extensions;
using Prayers.Models;
using Prayers.Services;
using Prayers.ViewModels.Extras;
using Syncfusion.XForms.ProgressBar;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Prayers.ViewModels
{
    public class PrayerViewModel : BaseViewModel
    {
        public ICommand btnGoPreviousCommand { get; set; }
        public ICommand btnGoNextCommand { get; set; }
        public ICommand progressTappedCommand { get; set; }
        public ICommand playAudio { get; set; }
        public ICommand pauseAudio { get; set; }
        public ICommand stopAudio { get; set; }
        public ICommand changeTheme { get; set; }
        public ICommand goHome { get; set; }

        int _pageId;
        public int PageId
        {
            get { return _pageId; }
            set
            {
                _pageId = value;
                OnPropertyChanged(nameof(PageId));
            }
        }
        private int pageIndex => PageId - 1;

        private SinglePageDataModel _singlePageDataModel;
        public SinglePageDataModel SinglePageDataModel
        {
            get => _singlePageDataModel;
            set
            {
                _singlePageDataModel = value;
                if (value != null)
                {
                    PageId = value.PageId;
                }
                OnPropertyChanged(nameof(SinglePageDataModel));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(CanGoNext));
            }
        }

        private ObservableCollection<PrayerProgressModel> _progressInformation;
        public ObservableCollection<PrayerProgressModel> ProgressInformation
        {
            get => _progressInformation;
            set
            {
                _progressInformation = value;
                OnPropertyChanged(nameof(ProgressInformation));
            }
        }

        public PrayerViewModel()
        {
            AudioFiles = new List<string>();
            btnGoPreviousCommand = new Command(async () => await GoPrevious());
            btnGoNextCommand = new Command(async () => await GoNext());
            progressTappedCommand = new Command<StepTappedEventArgs>(async (args) => await OnProgressStepTapped(args));
            playAudio = new Command(async () => await PlayAudio());
            pauseAudio = new Command(PauseAudio);
            stopAudio = new Command(StopAudio);
            changeTheme = new Command(async () => await ToggleAppTheme());
            goHome = new Command(async () => await ProcessRequestToGoHome());
        }

        public async Task GoPrevious()
        {
            if (CanGoPrevious)
            {
                StopAudio();
                var route = $"Page{SinglePageDataModel.PreviousPageId}?PageId={SinglePageDataModel.PreviousPageId}";
                await Shell.Current.GoToAsync(route);
            }
        }

        public async Task GoNext()
        {
            if (CanGoNext)
            {
                StopAudio();
                var route = $"Page{SinglePageDataModel.NextPageId}?PageId={SinglePageDataModel.NextPageId}";
                await Shell.Current.GoToAsync(route);
            }
        }   

        public bool CanGoPrevious => SinglePageDataModel?.PreviousPageId != null && SinglePageDataModel?.PreviousPageId.HasValue == true;

        public bool CanGoNext => SinglePageDataModel?.NextPageId != null && SinglePageDataModel?.NextPageId.HasValue == true;

        public async Task OnProgressStepTapped(StepTappedEventArgs args)
        {
            if (args != null && pageIndex != args.Index)
            {
                var route = $"Page{args.Index + 1}?PageId={args.Index + 1}";
                await Shell.Current.GoToAsync(route);
            }
        }

        public List<string> AudioFiles { get; set; }

        public async Task PlayAudio()
        {
            if (AudioFiles.Any())
            {
                await SharedServices.AudioController.PlayAudio(AudioFiles);
            }
        }

        public void PauseAudio()
        {
            if (AudioFiles.Any())
            {
                SharedServices.AudioController.PauseAudio();
            }
        }

        public void StopAudio()
        {
            if (AudioFiles.Any())
            {
                SharedServices.AudioController.StopAudio();
            }
        }

        public async Task ToggleAppTheme()
        {
            await Task.Run(() =>
            {
                ViewHelper.RunOnAppDispatcher(() =>
                {
                    SettingsHelper.Model.SelectedTheme = SettingsHelper.Model.SelectedTheme == "Light" ? "Dark" : "Light";
                    DefaultStyle = ThemeHelper.GetDefaultStyleTheme(SettingsHelper.Model.SelectedAppTheme);
                });
            });
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

        private async Task ProcessRequestToGoHome()
        {
            StopAudio();
            var route = $"///MainView";
            await Shell.Current.GoToAsync(route);
        }

        public string PathToMainImage => SharedServices.PathToMainImage;
    }
}
