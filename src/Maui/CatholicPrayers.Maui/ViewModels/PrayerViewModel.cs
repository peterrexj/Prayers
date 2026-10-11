using System.Collections.ObjectModel;
using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.ViewModels;

public class PrayerViewModel : BaseViewModel
{
    private readonly IPrayerService _prayerService;
    private readonly IFavouriteService _favouriteService;

    private string _prayerId = string.Empty;
    private bool _isFavourite;
    private PrayerData? _prayerData;
    private PrayerMasterEntry? _masterEntry;
    private int _currentSectionIndex;
    private string _accentColor = "#4A90D9";
    private double _fontSizeDelta;
    private ObservableCollection<PrayerMasterEntry> _relatedPrayers = [];
    private PrayerMasterEntry? _sourcePrayer;

    public PrayerData? PrayerData
    {
        get => _prayerData;
        private set
        {
            SetProperty(ref _prayerData, value);
            OnPropertyChanged(nameof(HasMultipleSections));
            OnPropertyChanged(nameof(CurrentSection));
            OnPropertyChanged(nameof(SectionCount));
            OnPropertyChanged(nameof(CurrentSectionNumber));
            OnPropertyChanged(nameof(SectionProgress));
            OnPropertyChanged(nameof(PageTitle));
            OnPropertyChanged(nameof(PageSubtitle));
            OnPropertyChanged(nameof(BackgroundImageSource));
            OnPropertyChanged(nameof(HeroImageFile));
            OnPropertyChanged(nameof(HeroImagePosition));
        }
    }

    public PrayerMasterEntry? MasterEntry
    {
        get => _masterEntry;
        private set => SetProperty(ref _masterEntry, value);
    }

    public int CurrentSectionIndex
    {
        get => _currentSectionIndex;
        set
        {
            SetProperty(ref _currentSectionIndex, value);
            OnPropertyChanged(nameof(CurrentSection));
            OnPropertyChanged(nameof(CurrentSectionNumber));
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
            OnPropertyChanged(nameof(IsLastSection));
            OnPropertyChanged(nameof(SectionProgress));
        }
    }

    public int CurrentSectionNumber => _currentSectionIndex + 1;

    public PrayerSection? CurrentSection
        => _prayerData?.Sections.ElementAtOrDefault(_currentSectionIndex);

    public bool HasMultipleSections
        => (_prayerData?.Sections.Length ?? 0) > 1;

    public int SectionCount => _prayerData?.Sections.Length ?? 1;

    public double SectionProgress
        => SectionCount <= 1 ? 1.0 : (double)(_currentSectionIndex + 1) / SectionCount;

    public bool CanGoPrevious => _currentSectionIndex > 0;
    public bool CanGoNext => _currentSectionIndex < (_prayerData?.Sections.Length ?? 1) - 1;
    public bool IsLastSection => !CanGoNext;

    public string PageTitle => _prayerData?.Title ?? string.Empty;
    public string PageSubtitle => _prayerData?.Subtitle ?? string.Empty;

    public ImageSource BackgroundImageSource
    {
        get
        {
            var file = _prayerData?.HeaderImageFile;
            if (!string.IsNullOrWhiteSpace(file))
                return ImageSource.FromFile(file);
            return ImageSource.FromFile("prayer_bg_default.svg");
        }
    }

    // Non-null only when the header image is a real photo (not SVG) — shown as hero card
    public string? HeroImageFile
    {
        get
        {
            var file = _prayerData?.HeaderImageFile;
            if (!string.IsNullOrWhiteSpace(file) && !file.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                return file;
            return null;
        }
    }

    // "Top" | "Bottom" — where to inject the hero card relative to prayer content
    public string HeroImagePosition => _prayerData?.HeaderImagePosition ?? "Top";

    public string AccentColor
    {
        get => _accentColor;
        private set => SetProperty(ref _accentColor, value);
    }

    public double FontSizeDelta
    {
        get => _fontSizeDelta;
        set => SetProperty(ref _fontSizeDelta, Math.Clamp(value, -2, 8));
    }

    public ObservableCollection<PrayerMasterEntry> RelatedPrayers
    {
        get => _relatedPrayers;
        private set => SetProperty(ref _relatedPrayers, value);
    }

    public PrayerMasterEntry? SourcePrayer
    {
        get => _sourcePrayer;
        private set => SetProperty(ref _sourcePrayer, value);
    }

    public bool IsFavourite
    {
        get => _isFavourite;
        private set => SetProperty(ref _isFavourite, value);
    }

    public event Action? SectionChanged;
    public event Action? FontSizeChanged;
    public event Action? AmenRequested;

    public Command GoHomeCommand { get; }
    public Command GoBackCommand { get; }
    public Command ToggleFavouriteCommand { get; }
    public Command GoPreviousCommand { get; }
    public Command GoNextCommand { get; }
    public Command ShowAmenCommand { get; }
    public Command IncreaseFontCommand { get; }
    public Command DecreaseFontCommand { get; }
    public Command<PrayerMasterEntry> NavigateToRelatedCommand { get; }

    public PrayerViewModel(IPrayerService prayerService, IFavouriteService favouriteService)
    {
        _prayerService = prayerService;
        _favouriteService = favouriteService;
        ToggleFavouriteCommand = new Command(async () =>
        {
            IsFavourite = !IsFavourite;
            await _favouriteService.SetAsync(_prayerId, IsFavourite);
        });
        GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
        GoBackCommand = new Command(async () =>
        {
            var location = Shell.Current.CurrentState.Location.OriginalString;
            var depth = location.Split('/').Count(s => s == "PrayerPage");
            var pops = string.Join("/", Enumerable.Repeat("..", Math.Max(depth, 1)));
            await Shell.Current.GoToAsync(pops);
        });
        GoPreviousCommand = new Command(GoPrevious, () => CanGoPrevious);
        GoNextCommand = new Command(GoNext, () => CanGoNext);
        ShowAmenCommand = new Command(() => AmenRequested?.Invoke());
        IncreaseFontCommand = new Command(() => { FontSizeDelta += 1; FontSizeChanged?.Invoke(); });
        DecreaseFontCommand = new Command(() => { FontSizeDelta -= 1; FontSizeChanged?.Invoke(); });
        NavigateToRelatedCommand = new Command<PrayerMasterEntry>(async p =>
            await Shell.Current.GoToAsync($"PrayerPage?PrayerId={p.Id}&FromPrayerId={MasterEntry?.Id ?? string.Empty}"));
    }

    public async Task LoadAsync(string prayerId, string? fromPrayerId = null)
    {
        IsBusy = true;
        try
        {
            _prayerId = prayerId;
            _currentSectionIndex = 0;
            FontSizeDelta = SharedServices.AppSettings.FontSizeDelta;

            var master = await _prayerService.GetMasterDataAsync();
            MasterEntry = master.Prayers.FirstOrDefault(p => p.Id == prayerId)
                       ?? master.Sections.FirstOrDefault(s => s.Id == prayerId);

            if (MasterEntry == null) return;

            AccentColor = MasterEntry.AccentColor;
            PrayerData = await _prayerService.GetPrayerDataAsync(MasterEntry.DataFile);
            IsFavourite = await _favouriteService.IsFavouriteAsync(prayerId);

            // Split related into forward (not where we came from) and source (where we came from)
            var relatedIds = MasterEntry.RelatedPrayerIds ?? [];
            var allRelated = master.Prayers.Where(p => relatedIds.Contains(p.Id)).ToList();

            SourcePrayer = string.IsNullOrEmpty(fromPrayerId)
                ? null
                : allRelated.FirstOrDefault(p => p.Id == fromPrayerId)
                  ?? master.Prayers.FirstOrDefault(p => p.Id == fromPrayerId);

            RelatedPrayers = new ObservableCollection<PrayerMasterEntry>(
                allRelated.Where(p => p.Id != fromPrayerId));

            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
            OnPropertyChanged(nameof(IsLastSection));
            RefreshCommands();
        }
        finally { IsBusy = false; }
    }

    private void GoPrevious()
    {
        if (!CanGoPrevious) return;
        CurrentSectionIndex--;
        SectionChanged?.Invoke();
        RefreshCommands();
    }

    private void GoNext()
    {
        if (!CanGoNext) return;
        CurrentSectionIndex++;
        SectionChanged?.Invoke();
        RefreshCommands();
    }

    public void NavigateToSection(int index)
    {
        if (index < 0 || index >= SectionCount) return;
        CurrentSectionIndex = index;
        SectionChanged?.Invoke();
        RefreshCommands();
    }

    private void RefreshCommands()
    {
        GoPreviousCommand.ChangeCanExecute();
        GoNextCommand.ChangeCanExecute();
    }
}
