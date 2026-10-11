using System.Collections.ObjectModel;
using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.ViewModels;

public class CollectionViewModel : BaseViewModel
{
    private readonly IPrayerService _prayerService;
    private readonly IFavouriteService _favouriteService;

    private string _sectionId = string.Empty;
    private string _pageTitle = string.Empty;
    private string _pageSubtitle = string.Empty;
    private string _accentColor = "#4A90D9";
    private ObservableCollection<PrayerMasterEntry> _items = [];
    private ObservableCollection<PrayerMasterEntry> _displayItems = [];
    private NovenaFilter _activeFilter = NovenaFilter.All;
    private bool _isNovenaSection;

    public enum NovenaFilter { All, ActiveNow, ByMonth }

    public string PageTitle
    {
        get => _pageTitle;
        private set => SetProperty(ref _pageTitle, value);
    }

    public string PageSubtitle
    {
        get => _pageSubtitle;
        private set => SetProperty(ref _pageSubtitle, value);
    }

    public string AccentColor
    {
        get => _accentColor;
        private set => SetProperty(ref _accentColor, value);
    }

    // All items (unfiltered)
    public ObservableCollection<PrayerMasterEntry> Items
    {
        get => _items;
        private set => SetProperty(ref _items, value);
    }

    // Filtered view shown by the page
    public ObservableCollection<PrayerMasterEntry> DisplayItems
    {
        get => _displayItems;
        private set => SetProperty(ref _displayItems, value);
    }

    public NovenaFilter ActiveFilter
    {
        get => _activeFilter;
        private set => SetProperty(ref _activeFilter, value);
    }

    public bool IsNovenaSection
    {
        get => _isNovenaSection;
        private set => SetProperty(ref _isNovenaSection, value);
    }

    // Expose active novenas for the "Active Now" header section
    public List<PrayerMasterEntry> ActiveNovenas =>
        _items.Where(NovenaDateHelper.IsActiveToday).ToList();

    public Command<PrayerMasterEntry> NavigateCommand { get; }
    public Command<PrayerMasterEntry> ToggleFavouriteCommand { get; }
    public Command GoBackCommand { get; }
    public Command GoHomeCommand { get; }
    public Command<string> SetFilterCommand { get; }

    public event Action? ItemsLoaded;

    public CollectionViewModel(IPrayerService prayerService, IFavouriteService favouriteService)
    {
        _prayerService = prayerService;
        _favouriteService = favouriteService;

        NavigateCommand = new Command<PrayerMasterEntry>(async e => await NavigateAsync(e));
        ToggleFavouriteCommand = new Command<PrayerMasterEntry>(async e => await ToggleFavouriteAsync(e));
        GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
        SetFilterCommand = new Command<string>(ApplyFilter);
    }

    public async Task LoadAsync(string sectionId)
    {
        if (IsBusy) return;
        IsBusy = true;
        _sectionId = sectionId;
        _activeFilter = NovenaFilter.All;
        try
        {
            var master = await _prayerService.GetMasterDataAsync();

            var section = master.Sections.FirstOrDefault(s => s.Id == sectionId)
                       ?? master.Prayers.FirstOrDefault(p => p.Id == sectionId);

            if (section != null)
            {
                PageTitle    = section.Title;
                PageSubtitle = section.Subtitle;
                AccentColor  = section.AccentColor;
            }

            var children = (await _prayerService.GetChildrenAsync(sectionId)).ToList();

            var favs = await _favouriteService.LoadAllAsync();
            foreach (var c in children)
                c.IsFavourite = favs.Contains(c.Id);

            Items = new ObservableCollection<PrayerMasterEntry>(children);
            IsNovenaSection = sectionId == "section_novenas";
            ApplyFilter(NovenaFilter.All.ToString());

            ItemsLoaded?.Invoke();
        }
        finally { IsBusy = false; }
    }

    private void ApplyFilter(string filterName)
    {
        if (!Enum.TryParse<NovenaFilter>(filterName, out var filter))
            filter = NovenaFilter.All;

        ActiveFilter = filter;

        IEnumerable<PrayerMasterEntry> source = filter switch
        {
            NovenaFilter.ActiveNow => _items.Where(NovenaDateHelper.IsActiveToday),
            NovenaFilter.ByMonth   => _items.OrderBy(GetSortMonth),
            _                      => _items
        };

        DisplayItems = new ObservableCollection<PrayerMasterEntry>(source);
        ItemsLoaded?.Invoke();
    }

    private static int GetSortMonth(PrayerMasterEntry e)
    {
        var window = NovenaDateHelper.GetWindow(e);
        return window?.Start.Month ?? 13; // moveable feasts sort last
    }

    private static async Task NavigateAsync(PrayerMasterEntry? entry)
    {
        if (entry == null) return;
        switch (entry.NodeType)
        {
            case "Prayer":
            case "Novena":
                await Shell.Current.GoToAsync($"PrayerPage?PrayerId={Uri.EscapeDataString(entry.Id)}");
                break;
            case "Collection":
                await Shell.Current.GoToAsync($"CollectionPage?SectionId={Uri.EscapeDataString(entry.Id)}");
                break;
            case "ExternalLink":
                await Shell.Current.GoToAsync($"LinkDetailPage?EntryId={Uri.EscapeDataString(entry.Id)}");
                break;
            case "Bible":
                await Shell.Current.GoToAsync("BiblePage");
                break;
        }
    }

    private async Task ToggleFavouriteAsync(PrayerMasterEntry? entry)
    {
        if (entry == null) return;
        await _favouriteService.SetAsync(entry.Id, entry.IsFavourite);
    }
}
