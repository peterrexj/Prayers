using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Input;
using Prayers.StJoseph.Maui.Models;
using Prayers.StJoseph.Maui.Services;

namespace Prayers.StJoseph.Maui.ViewModels;

public class PrayerViewModel : BaseViewModel
{
    public ICommand BtnGoPreviousCommand { get; }
    public ICommand BtnGoNextCommand { get; }
    public ICommand ProgressTappedCommand { get; }
    public ICommand PlayAudioCommand { get; }
    public ICommand PauseAudioCommand { get; }
    public ICommand StopAudioCommand { get; }
    public ICommand ChangeThemeCommand { get; }
    public ICommand GoHomeCommand { get; }
    public ICommand IncreaseFontCommand { get; }
    public ICommand DecreaseFontCommand { get; }

    private const int MaxFontDelta = 8;
    private const int MinFontDelta = -2;

    public PrayerViewModel()
    {
        AudioFiles = [];
        ContentFontSizes = [];

        BtnGoPreviousCommand = new Command(async () => await GoPrevious(), () => CanGoPrevious);
        BtnGoNextCommand = new Command(async () => await GoNext(), () => CanGoNext);
        ProgressTappedCommand = new Command<int>(async (idx) => await OnProgressStepTapped(idx));
        PlayAudioCommand = new Command(async () => await PlayAudio());
        PauseAudioCommand = new Command(PauseAudio);
        StopAudioCommand = new Command(StopAudio);
        ChangeThemeCommand = new Command(async () => await ToggleAppTheme());
        GoHomeCommand = new Command(async () => await ProcessRequestToGoHome());
        IncreaseFontCommand = new Command(async () => await IncreaseFont());
        DecreaseFontCommand = new Command(async () => await DecreaseFont());
    }

    private int _pageId;
    public int PageId
    {
        get => _pageId;
        set { _pageId = value; OnPropertyChanged(nameof(PageId)); OnPropertyChanged(nameof(PageIndex)); }
    }

    public int PageIndex => PageId - 1;

    private string? _prayerMainTitle;
    public string? PrayerMainTitle
    {
        get => _prayerMainTitle;
        set { _prayerMainTitle = value; OnPropertyChanged(nameof(PrayerMainTitle)); }
    }

    private SinglePageDataModel? _singlePageDataModel;
    public SinglePageDataModel? SinglePageDataModel
    {
        get => _singlePageDataModel;
        set
        {
            _singlePageDataModel = value;
            if (value != null) PageId = value.PageId;
            OnPropertyChanged(nameof(SinglePageDataModel));
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
            (BtnGoPreviousCommand as Command)?.ChangeCanExecute();
            (BtnGoNextCommand as Command)?.ChangeCanExecute();
        }
    }


    public bool CanGoPrevious => SinglePageDataModel?.PreviousPageId.HasValue == true;
    public bool CanGoNext => SinglePageDataModel?.NextPageId.HasValue == true;

    public List<string> AudioFiles { get; set; }

    // Maps content text → current font size so IncreaseFont/DecreaseFont can adjust live
    public Dictionary<string, double> ContentFontSizes { get; set; }

    // Raised when font delta changes so controls can re-read their size
    public event Action? FontSizeChanged;

    // Raised when theme is toggled so view can re-apply all content colors
    public event Action? ThemeChanged;

    private double _currentFontDelta;
    public double CurrentFontDelta
    {
        get => _currentFontDelta;
        set { _currentFontDelta = value; OnPropertyChanged(nameof(CurrentFontDelta)); }
    }

    [JsonIgnore]
    private StyleModelDefault? _defaultStyle;
    [JsonIgnore]
    public StyleModelDefault? DefaultStyle
    {
        get => _defaultStyle;
        set { _defaultStyle = value; OnPropertyChanged(nameof(DefaultStyle)); }
    }

    public string PathToMainImage => "mainpic.jpg";

    public async Task GoPrevious()
    {
        if (!CanGoPrevious) return;
        IsBusy = true;
        try
        {
            StopAudio();
            await Shell.Current.GoToAsync($"Page{SinglePageDataModel!.PreviousPageId}?PageId={SinglePageDataModel.PreviousPageId}", animate: false);
        }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
        finally { IsBusy = false; }
    }

    public async Task GoNext()
    {
        if (!CanGoNext) return;
        IsBusy = true;
        try
        {
            StopAudio();
            await Shell.Current.GoToAsync($"Page{SinglePageDataModel!.NextPageId}?PageId={SinglePageDataModel.NextPageId}", animate: false);
        }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
        finally { IsBusy = false; }
    }

    public async Task OnProgressStepTapped(int index)
    {
        try
        {
            if (PageIndex == index) return;
            StopAudio();
            await Shell.Current.GoToAsync($"Page{index + 1}?PageId={index + 1}");
        }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
    }

    public async Task PlayAudio()
    {
        try
        {
            if (AudioFiles.Count == 0) return;
            var audio = ServiceLocator.GetService<IAudioService>();
            if (audio != null) await audio.Play(AudioFiles);
        }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
    }

    public void PauseAudio()
    {
        try { ServiceLocator.GetService<IAudioService>()?.Pause(); }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
    }

    public void StopAudio()
    {
        try { ServiceLocator.GetService<IAudioService>()?.Stop(); }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
    }

    public async Task ToggleAppTheme()
    {
        try
        {
            var settings = ServiceLocator.GetService<SettingsViewModel>();
            if (settings == null) return;
            settings.SelectedTheme = settings.SelectedTheme == "Light" ? "Dark" : "Light";
            var themeHelper = ServiceLocator.GetService<IThemeService>();
            if (themeHelper != null)
            {
                DefaultStyle = themeHelper.GetDefaultStyleTheme(settings.SelectedAppTheme);
                themeHelper.UpdateAppThemes(DefaultStyle);
            }
            ThemeChanged?.Invoke();
        }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
        await Task.CompletedTask;
    }

    public async Task IncreaseFont()
    {
        await Task.Run(() =>
        {
            IsBusy = true;
            try
            {
                var settings = ServiceLocator.GetService<SettingsViewModel>();
                if (settings == null) return;
                if (settings.CurrentFontSize < MaxFontDelta)
                {
                    settings.CurrentFontSize++;
                    CurrentFontDelta = settings.CurrentFontSize;
                    FontSizeChanged?.Invoke();
                }
            }
            catch (Exception ex) { ExceptionHandler.Capture(ex); }
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
                var settings = ServiceLocator.GetService<SettingsViewModel>();
                if (settings == null) return;
                if (settings.CurrentFontSize > MinFontDelta)
                {
                    settings.CurrentFontSize--;
                    CurrentFontDelta = settings.CurrentFontSize;
                    FontSizeChanged?.Invoke();
                }
            }
            catch (Exception ex) { ExceptionHandler.Capture(ex); }
            finally { IsBusy = false; }
        });
    }

    private async Task ProcessRequestToGoHome()
    {
        try
        {
            StopAudio();
            await Shell.Current.GoToAsync("///MainView");
        }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
    }
}
