using Newtonsoft.Json;
using Prayers.Extensions;
using Prayers.Models;
using Prayers.Services;
using Prayers.ViewModels.Extras;
using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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
        public ICommand increaseFont { get; set; }
        public ICommand decreaseFont { get; set; }

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
        private const int maxFontSize = 8;
        private const int minFontSize = -2;

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

        public List<ParaContentView> ParaContentViews { get; set; }
        public List<ParaBulletView> ParaBulletViews { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
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
            increaseFont = new Command(async () => await IncreaseFont());
            decreaseFont = new Command(async () => await DecreaseFont());
        }

        public async Task GoPrevious()
        {
            try
            {
                if (CanGoPrevious)
                {
                    StopAudio();
                    var route = $"Page{SinglePageDataModel.PreviousPageId}?PageId={SinglePageDataModel.PreviousPageId}";
                    await Shell.Current.GoToAsync(route);
                }
            }
            catch (Exception ex) { ExceptionHandler.CaptureException(ex); }
        }

        public async Task GoNext()
        {
            try
            {
                if (CanGoNext)
                {
                    StopAudio();
                    var route = $"Page{SinglePageDataModel.NextPageId}?PageId={SinglePageDataModel.NextPageId}";
                    await Shell.Current.GoToAsync(route);
                }
            }
            catch (Exception ex) { ExceptionHandler.CaptureException(ex); }
        }

        public bool CanGoPrevious => SinglePageDataModel?.PreviousPageId != null && SinglePageDataModel?.PreviousPageId.HasValue == true;

        public bool CanGoNext => SinglePageDataModel?.NextPageId != null && SinglePageDataModel?.NextPageId.HasValue == true;

        public async Task OnProgressStepTapped(StepTappedEventArgs args)
        {
            try
            {
                if (args != null && pageIndex != args.Index)
                {
                    var route = $"Page{args.Index + 1}?PageId={args.Index + 1}";
                    await Shell.Current.GoToAsync(route);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.CaptureException(ex);
            }
        }

        public List<string> AudioFiles { get; set; }

        public async Task PlayAudio()
        {
            try
            {
                if (AudioFiles.Any())
                {
                    await SharedServices.AudioController.PlayAudio(AudioFiles);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.CaptureException(ex);
            }
        }

        public void PauseAudio()
        {
            try
            {
                if (AudioFiles.Any())
                {
                    SharedServices.AudioController.PauseAudio();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.CaptureException(ex);
            }
        }

        public void StopAudio()
        {
            try
            {
                if (AudioFiles.Any())
                {
                    SharedServices.AudioController.StopAudio();
                }
            }
            catch (Exception ex) { ExceptionHandler.CaptureException(ex); }
        }

        public async Task ToggleAppTheme()
        {
            await Task.Run(() =>
            {
                try
                {
                    ViewHelper.RunOnAppDispatcher(() =>
                            {
                                SettingsHelper.Model.SelectedTheme = SettingsHelper.Model.SelectedTheme == "Light" ? "Dark" : "Light";
                                DefaultStyle = ThemeHelper.GetDefaultStyleTheme(SettingsHelper.Model.SelectedAppTheme);
                            });
                }
                catch (Exception ex)
                {
                    ExceptionHandler.CaptureException(ex);
                }
            });
        }

        public async Task IncreaseFont()
        {
            await Task.Run(() =>
            {
                IsBusy = true;
                try
                {
                    if (SettingsHelper.Model.CurrentFontSize == 0 || SettingsHelper.Model.CurrentFontSize < maxFontSize)
                    {
                        SettingsHelper.Model.CurrentFontSize++;

                        foreach (var para in ParaContentViews)
                        {
                            var realFontSize = SinglePageDataModel.Content.FirstOrDefault(f => f.Content == para.ParaContent)?.FontSize;
                            if (realFontSize != null && realFontSize > 0)
                            {
                                if (para.FontSize < realFontSize + maxFontSize)
                                {
                                    para.FontSize++;
                                }
                            }
                        }

                        foreach (var para in ParaBulletViews)
                        {
                            var realFontSize = SinglePageDataModel.Content.FirstOrDefault(f => f.Content == para.ParaContent)?.FontSize;
                            if (realFontSize != null && realFontSize > 0)
                            {
                                if (para.FontSize < realFontSize + maxFontSize)
                                {
                                    para.FontSize++;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.CaptureException(ex);
                }
                finally { IsBusy = false; }
            });
        }
        public async Task DecreaseFont()
        {
            await Task.Run(() =>
            {
                IsBusy = true;
                try
                {
                    if (SettingsHelper.Model.CurrentFontSize == 0 || SettingsHelper.Model.CurrentFontSize > minFontSize)
                    {
                        SettingsHelper.Model.CurrentFontSize--;

                        foreach (var para in ParaContentViews)
                        {
                            var realFontSize = SinglePageDataModel.Content.FirstOrDefault(f => f.Content == para.ParaContent)?.FontSize;
                            if (realFontSize != null && realFontSize > 0)
                            {
                                if (para.FontSize > realFontSize + minFontSize)
                                {
                                    para.FontSize--;
                                }
                            }
                        }

                        foreach (var para in ParaBulletViews)
                        {
                            var realFontSize = SinglePageDataModel.Content.FirstOrDefault(f => f.Content == para.ParaContent)?.FontSize;
                            if (realFontSize != null && realFontSize > 0)
                            {
                                if (para.FontSize > realFontSize + maxFontSize)
                                {
                                    para.FontSize--;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.CaptureException(ex);
                }
                finally { IsBusy = false; }
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
            try
            {
                StopAudio();
                var route = $"///MainView";
                await Shell.Current.GoToAsync(route);
            }
            catch (Exception ex)
            {
                ExceptionHandler.CaptureException(ex);
            }
        }

        public string PathToMainImage => SharedServices.PathToMainImage;
    }
}
