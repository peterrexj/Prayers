using System.Collections.ObjectModel;
using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.ViewModels;

public class BibleViewModel : BaseViewModel
{
    private readonly IBibleService _bibleService;
    private readonly IFavouriteService _favouriteService;

    private ObservableCollection<BibleBook> _oldTestament = [];
    private ObservableCollection<BibleBook> _newTestament = [];
    private ObservableCollection<BibleSearchResult> _searchResults = [];
    private string _testament = "OT";
    private string _searchText = string.Empty;
    private bool _isSearchActive;

    public ObservableCollection<BibleBook> OldTestament
    {
        get => _oldTestament;
        private set => SetProperty(ref _oldTestament, value);
    }

    public ObservableCollection<BibleBook> NewTestament
    {
        get => _newTestament;
        private set => SetProperty(ref _newTestament, value);
    }

    public ObservableCollection<BibleSearchResult> SearchResults
    {
        get => _searchResults;
        private set => SetProperty(ref _searchResults, value);
    }

    public string ActiveTestament
    {
        get => _testament;
        private set => SetProperty(ref _testament, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
                _ = ApplySearchAsync(value);
        }
    }

    public bool IsSearchActive
    {
        get => _isSearchActive;
        private set => SetProperty(ref _isSearchActive, value);
    }

    public bool ShowOT => _testament == "OT";
    public bool ShowNT => _testament == "NT";

    public Command GoBackCommand { get; }
    public Command<BibleBook> OpenBookCommand { get; }
    public Command<BibleBook> ToggleBookFavCommand { get; }
    public Command<string> SetTestamentCommand { get; }
    public Command ClearSearchCommand { get; }
    public Command<BibleSearchResult> OpenSearchResultCommand { get; }

    public BibleViewModel(IBibleService bibleService, IFavouriteService favouriteService)
    {
        _bibleService = bibleService;
        _favouriteService = favouriteService;
        GoBackCommand           = new Command(async () => await Shell.Current.GoToAsync(".."));
        OpenBookCommand         = new Command<BibleBook>(async b => await OpenBookAsync(b));
        ToggleBookFavCommand    = new Command<BibleBook>(async b => await ToggleBookFavAsync(b));
        ClearSearchCommand      = new Command(() => SearchText = string.Empty);
        OpenSearchResultCommand = new Command<BibleSearchResult>(async r => await OpenSearchResultAsync(r));
        SetTestamentCommand     = new Command<string>(t =>
        {
            ActiveTestament = t;
            OnPropertyChanged(nameof(ShowOT));
            OnPropertyChanged(nameof(ShowNT));
        });
    }

    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var books = await _bibleService.GetBooksAsync();
            var favs = await _favouriteService.LoadAllAsync();
            foreach (var b in books)
                b.IsFavourite = favs.Contains($"bible_book_{b.Id}");
            OldTestament = new ObservableCollection<BibleBook>(
                books.Where(b => b.Testament == "OT").OrderBy(b => b.SortOrder));
            NewTestament = new ObservableCollection<BibleBook>(
                books.Where(b => b.Testament == "NT").OrderBy(b => b.SortOrder));
        }
        finally { IsBusy = false; }
    }

    public async Task StampFavsAsync()
    {
        var favs = await _favouriteService.LoadAllAsync();
        foreach (var b in OldTestament.Concat(NewTestament))
            b.IsFavourite = favs.Contains($"bible_book_{b.Id}");
    }

    private async Task ApplySearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            IsSearchActive = false;
            SearchResults = [];
            return;
        }

        IsSearchActive = true;
        try
        {
            var results = await _bibleService.SearchAsync(query);
            SearchResults = new ObservableCollection<BibleSearchResult>(results);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[BibleViewModel] SearchAsync failed: {ex.Message}");
        }
    }

    private async Task ToggleBookFavAsync(BibleBook? book)
    {
        if (book == null) return;
        book.IsFavourite = !book.IsFavourite;
        await _favouriteService.SetAsync($"bible_book_{book.Id}", book.IsFavourite);
    }

    private static async Task OpenBookAsync(BibleBook? book)
    {
        if (book == null) return;
        await Shell.Current.GoToAsync($"BookPage?BookId={Uri.EscapeDataString(book.Id)}");
    }

    private static async Task OpenSearchResultAsync(BibleSearchResult? result)
    {
        if (result == null) return;
        await Shell.Current.GoToAsync(
            $"BookPage?BookId={Uri.EscapeDataString(result.BookId)}&Chapter={result.Chapter}&Verse={result.Verse}");
    }
}
