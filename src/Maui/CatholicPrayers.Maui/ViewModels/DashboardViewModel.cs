using System.Collections.ObjectModel;
using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private readonly IPrayerService _prayerService;
    private readonly IFavouriteService _favouriteService;
    private readonly IBibleService _bibleService;

    private string _searchText = string.Empty;
    private ObservableCollection<PrayerMasterEntry> _filteredPrayers = [];
    private ObservableCollection<PrayerMasterEntry> _featuredPrayers = [];
    private ObservableCollection<CategoryGroup> _categoryGroups = [];
    private ObservableCollection<PrayerMasterEntry> _sections = [];
    private ObservableCollection<FavouriteDisplayItem> _favouriteItems = [];
    private PrayerMasterData? _masterData;
    private bool _isSearchActive;
    private bool _hasFeatured;
    private bool _showFavouritesOnly;

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
                _ = ApplySearchAsync(value);
        }
    }

    public ObservableCollection<PrayerMasterEntry> FilteredPrayers
    {
        get => _filteredPrayers;
        private set => SetProperty(ref _filteredPrayers, value);
    }

    public ObservableCollection<PrayerMasterEntry> FeaturedPrayers
    {
        get => _featuredPrayers;
        private set => SetProperty(ref _featuredPrayers, value);
    }

    public ObservableCollection<CategoryGroup> CategoryGroups
    {
        get => _categoryGroups;
        private set => SetProperty(ref _categoryGroups, value);
    }

    public bool IsSearchActive
    {
        get => _isSearchActive;
        set => SetProperty(ref _isSearchActive, value);
    }

    public bool HasFeatured
    {
        get => _hasFeatured;
        private set => SetProperty(ref _hasFeatured, value);
    }

    public ObservableCollection<PrayerMasterEntry> Sections
    {
        get => _sections;
        private set => SetProperty(ref _sections, value);
    }

    public bool ShowFavouritesOnly
    {
        get => _showFavouritesOnly;
        set
        {
            if (SetProperty(ref _showFavouritesOnly, value))
            {
                if (value)
                    _ = LoadFavouriteItemsAsync();
                else
                    RebuildCategoryGroups();
            }
        }
    }

    public ObservableCollection<FavouriteDisplayItem> FavouriteItems
    {
        get => _favouriteItems;
        private set => SetProperty(ref _favouriteItems, value);
    }

    public Command<PrayerMasterEntry> NavigateToPrayerCommand { get; }
    public Command<PrayerMasterEntry> ToggleFavouriteCommand { get; }
    public Command ToggleFavouritesFilterCommand { get; }
    public Command ClearSearchCommand { get; }
    public Command RefreshCommand { get; }
    public Command NavigateToSettingsCommand { get; set; }

    public event Action? HasFeaturedChanged;
    public event Action? PrayersLoaded;

    public DashboardViewModel(IPrayerService prayerService, IFavouriteService favouriteService, IBibleService bibleService)
    {
        _prayerService = prayerService;
        _favouriteService = favouriteService;
        _bibleService = bibleService;
        NavigateToPrayerCommand = new Command<PrayerMasterEntry>(async p => await NavigateToPrayerAsync(p));
        ToggleFavouriteCommand = new Command<PrayerMasterEntry>(async p => await ToggleFavouriteAsync(p));
        ToggleFavouritesFilterCommand = new Command(() => ShowFavouritesOnly = !ShowFavouritesOnly);
        ClearSearchCommand = new Command(() => SearchText = string.Empty);
        RefreshCommand = new Command(async () => await LoadAsync());
        NavigateToSettingsCommand = new Command(async () =>
            await Shell.Current.GoToAsync("SettingsPage"));
    }

    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            _masterData = await _prayerService.GetMasterDataAsync();
            var all = _masterData.Prayers.OrderBy(p => p.SortOrder).ToList();

            // Top-level section cards
            Sections = new ObservableCollection<PrayerMasterEntry>(
                _masterData.Sections.OrderBy(s => s.SortOrder));

            // Stamp IsFavourite from persisted store
            var favs = await _favouriteService.LoadAllAsync();
            foreach (var p in all)
                p.IsFavourite = favs.Contains(p.Id);

            FeaturedPrayers = new ObservableCollection<PrayerMasterEntry>(all.Where(p => p.IsFeatured));
            HasFeatured = FeaturedPrayers.Count > 0;
            HasFeaturedChanged?.Invoke();

            FilteredPrayers = new ObservableCollection<PrayerMasterEntry>(all);
            RebuildCategoryGroups();
            PrayersLoaded?.Invoke();
        }
        finally { IsBusy = false; }
    }

    public async Task LoadFavouriteItemsAsync()
    {
        var favIds = await _favouriteService.LoadAllAsync();
        if (favIds.Count == 0)
        {
            FavouriteItems = [];
            PrayersLoaded?.Invoke();
            return;
        }

        var items = new List<FavouriteDisplayItem>();

        // Prayer / Novena favs
        if (_masterData != null)
        {
            foreach (var id in favIds.Where(f => !f.StartsWith("bible_")))
            {
                var entry = _masterData.Prayers.FirstOrDefault(p => p.Id == id);
                if (entry == null) continue;
                items.Add(new FavouriteDisplayItem
                {
                    Id         = id,
                    Type       = entry.NodeType == "Novena" ? "Novena" : "Prayer",
                    Title      = entry.Title,
                    Subtitle   = entry.Subtitle,
                    Icon       = entry.NodeType == "Novena" ? "" : "",
                    AccentColor = entry.AccentColor,
                    Route      = $"PrayerPage?PrayerId={Uri.EscapeDataString(id)}"
                });
            }
        }

        // Bible Book favs: "bible_book_{bookId}"
        var bookIds = favIds
            .Where(f => f.StartsWith("bible_book_"))
            .Select(f => f["bible_book_".Length..])
            .ToList();

        // Bible Chapter favs: "bible_chapter_{bookId}_{chapter}"
        var chapterEntries = favIds
            .Where(f => f.StartsWith("bible_chapter_"))
            .Select(f =>
            {
                var rest = f["bible_chapter_".Length..];
                var lastUnderscore = rest.LastIndexOf('_');
                if (lastUnderscore < 0) return (string?)null;
                var bId = rest[..lastUnderscore];
                if (!int.TryParse(rest[(lastUnderscore + 1)..], out var ch)) return null;
                return $"{f}|{bId}|{ch}";
            })
            .Where(x => x != null)
            .Select(x => { var p = x!.Split('|'); return (FavId: p[0], BookId: p[1], Chapter: int.Parse(p[2])); })
            .ToList();

        if (bookIds.Count > 0 || chapterEntries.Count > 0)
        {
            var books = await _bibleService.GetBooksAsync();
            var bookMap = books.ToDictionary(b => b.Id);

            foreach (var bookId in bookIds)
            {
                if (!bookMap.TryGetValue(bookId, out var book)) continue;
                items.Add(new FavouriteDisplayItem
                {
                    Id         = $"bible_book_{bookId}",
                    Type       = "Bible Book",
                    Title      = book.Title,
                    Subtitle   = $"{book.ChapterCount} chapter{(book.ChapterCount == 1 ? "" : "s")}",
                    Icon       = "",
                    AccentColor = "#27AE60",
                    Route      = $"BookPage?BookId={Uri.EscapeDataString(bookId)}"
                });
            }

            foreach (var entry in chapterEntries)
            {
                if (!bookMap.TryGetValue(entry.BookId, out var book)) continue;
                items.Add(new FavouriteDisplayItem
                {
                    Id         = entry.FavId,
                    Type       = "Bible Chapter",
                    Title      = $"{book.Title} · Chapter {entry.Chapter}",
                    Subtitle   = book.Abbreviation,
                    Icon       = "",
                    AccentColor = "#27AE60",
                    Route      = $"BookPage?BookId={Uri.EscapeDataString(entry.BookId)}&Chapter={entry.Chapter.ToString()}"
                });
            }
        }

        FavouriteItems = new ObservableCollection<FavouriteDisplayItem>(items);
        PrayersLoaded?.Invoke();
    }

    private void RebuildCategoryGroups()
    {
        if (_masterData == null) return;
        var all = _masterData.Prayers.OrderBy(p => p.SortOrder).ToList();
        var source = _showFavouritesOnly ? all.Where(p => p.IsFavourite).ToList() : all;

        // Featured section also respects the filter
        FeaturedPrayers = new ObservableCollection<PrayerMasterEntry>(source.Where(p => p.IsFeatured));
        HasFeatured = FeaturedPrayers.Count > 0;

        var groups = source
            .GroupBy(p => p.Category)
            .OrderBy(g => g.Key)
            .Select(g => new CategoryGroup(g.Key, g.ToList()));
        CategoryGroups = new ObservableCollection<CategoryGroup>(groups);
        PrayersLoaded?.Invoke();
    }

    private async Task ToggleFavouriteAsync(PrayerMasterEntry? entry)
    {
        if (entry == null) return;
        // IsFavourite is already the new value (tile toggled it before firing the event)
        await _favouriteService.SetAsync(entry.Id, entry.IsFavourite);

        // If filtering by favs and we just un-favourited, rebuild so it disappears
        if (_showFavouritesOnly && !entry.IsFavourite)
            RebuildCategoryGroups();
    }

    private async Task ApplySearchAsync(string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                IsSearchActive = false;
                if (_masterData != null)
                    FilteredPrayers = new ObservableCollection<PrayerMasterEntry>(
                        _masterData.Prayers.OrderBy(p => p.SortOrder));
                return;
            }

            IsSearchActive = true;
            var results = await _prayerService.SearchAsync(query);
            FilteredPrayers = new ObservableCollection<PrayerMasterEntry>(results);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Dashboard] ApplySearchAsync failed: {ex.Message}");
        }
    }

    private static async Task NavigateToPrayerAsync(PrayerMasterEntry? entry)
    {
        if (entry == null) return;
        switch (entry.NodeType)
        {
            case "Collection":
                await Shell.Current.GoToAsync($"CollectionPage?SectionId={Uri.EscapeDataString(entry.Id)}");
                break;
            case "Bible":
                await Shell.Current.GoToAsync("BiblePage");
                break;
            case "ExternalLink":
                await Shell.Current.GoToAsync($"LinkDetailPage?EntryId={Uri.EscapeDataString(entry.Id)}");
                break;
            case "PartnerApp":
                await Shell.Current.GoToAsync($"PartnerAppPage?EntryId={Uri.EscapeDataString(entry.Id)}");
                break;
            default:
                await Shell.Current.GoToAsync($"PrayerPage?PrayerId={Uri.EscapeDataString(entry.Id)}");
                break;
        }
    }
}

public class CategoryGroup : List<PrayerMasterEntry>
{
    public string Category { get; }
    public CategoryGroup(string category, IList<PrayerMasterEntry> items) : base(items)
        => Category = category;
}
