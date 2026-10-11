using System.Collections.ObjectModel;
using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.ViewModels;

public class BookViewModel : BaseViewModel
{
    private readonly IBibleService _bibleService;
    private readonly IFavouriteService _favouriteService;

    private string _bookId = string.Empty;
    private bool _isChapterFavourite;
    public HashSet<int> FavouritedChapters { get; private set; } = [];
    private BibleBook? _bookMeta;
    private string _pageTitle = string.Empty;
    private string _pageSubtitle = string.Empty;
    private ObservableCollection<int> _chapterNumbers = [];
    private List<BibleVerse> _currentVerses = [];
    private List<BibleCrossRef> _currentCrossRefs = [];
    private int _selectedChapter;
    private bool _showChapterList = true;

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

    public ObservableCollection<int> ChapterNumbers
    {
        get => _chapterNumbers;
        private set => SetProperty(ref _chapterNumbers, value);
    }

    public List<BibleVerse> CurrentVerses
    {
        get => _currentVerses;
        private set => SetProperty(ref _currentVerses, value);
    }

    public List<BibleCrossRef> CurrentCrossRefs
    {
        get => _currentCrossRefs;
        private set => SetProperty(ref _currentCrossRefs, value);
    }

    public int SelectedChapter
    {
        get => _selectedChapter;
        private set => SetProperty(ref _selectedChapter, value);
    }

    public bool ShowChapterList
    {
        get => _showChapterList;
        private set
        {
            SetProperty(ref _showChapterList, value);
            OnPropertyChanged(nameof(ShowVerseView));
        }
    }

    public bool ShowVerseView => !_showChapterList;

    public bool IsChapterFavourite
    {
        get => _isChapterFavourite;
        private set => SetProperty(ref _isChapterFavourite, value);
    }

    public Command GoBackCommand { get; }
    public Command ToggleChapterFavCommand { get; }
    public Command<int> OpenChapterCommand { get; }
    public Command BackToChaptersCommand { get; }
    public Command GoToBibleHomeCommand { get; }
    public Command GoToAppHomeCommand { get; }

    public BookViewModel(IBibleService bibleService, IFavouriteService favouriteService)
    {
        _bibleService         = bibleService;
        _favouriteService     = favouriteService;
        GoBackCommand         = new Command(async () =>
        {
            if (ShowVerseView) { ShowChapterList = true; SelectedChapter = 0; IsChapterFavourite = false; }
            else await Shell.Current.GoToAsync("..");
        });
        OpenChapterCommand    = new Command<int>(async ch => await SelectChapterAsync(ch));
        BackToChaptersCommand = new Command(() => { ShowChapterList = true; SelectedChapter = 0; IsChapterFavourite = false; });
        ToggleChapterFavCommand = new Command(async () =>
        {
            IsChapterFavourite = !IsChapterFavourite;
            if (IsChapterFavourite)
                FavouritedChapters.Add(SelectedChapter);
            else
                FavouritedChapters.Remove(SelectedChapter);
            await _favouriteService.SetAsync($"bible_chapter_{_bookId}_{SelectedChapter}", IsChapterFavourite);
        });
        GoToBibleHomeCommand  = new Command(async () =>
        {
            if (ShowVerseView) ShowChapterList = true;
            // Count how many BookPage levels are in the stack and pop them all
            var location = Shell.Current.CurrentState.Location.OriginalString;
            var depth = location.Split('/').Count(s => s == "BookPage");
            var pops = string.Join("/", Enumerable.Repeat("..", Math.Max(depth, 1)));
            await Shell.Current.GoToAsync(pops);
        });
        GoToAppHomeCommand    = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
    }

    public async Task LoadAsync(string bookId, int jumpToChapter = 0)
    {
        if (IsBusy) return;
        IsBusy = true;
        _bookId = bookId;
        ShowChapterList = true;
        SelectedChapter = 0;
        try
        {
            var books = await _bibleService.GetBooksAsync();
            _bookMeta = books.FirstOrDefault(b => b.Id == bookId);

            PageTitle    = _bookMeta?.Title ?? bookId;
            PageSubtitle = _bookMeta != null
                ? $"{_bookMeta.Abbreviation} · {_bookMeta.ChapterCount} chapter{(_bookMeta.ChapterCount == 1 ? "" : "s")}"
                : string.Empty;

            var count = _bookMeta?.ChapterCount ?? 0;
            ChapterNumbers = new ObservableCollection<int>(Enumerable.Range(1, count));

            // Pre-load all favourited chapters for this book so the grid can show hearts
            var favs = await _favouriteService.LoadAllAsync();
            var prefix = $"bible_chapter_{bookId}_";
            FavouritedChapters = favs
                .Where(f => f.StartsWith(prefix))
                .Select(f => int.TryParse(f[prefix.Length..], out var ch) ? ch : 0)
                .Where(ch => ch > 0)
                .ToHashSet();

            if (jumpToChapter > 0)
                await SelectChapterAsync(jumpToChapter);
        }
        finally { IsBusy = false; }
    }

    public async Task SelectChapterAsync(int chapter)
    {
        if (chapter <= 0) return;
        IsBusy = true;
        try
        {
            var verses = await _bibleService.GetChapterAsync(_bookId, chapter);
            CurrentVerses       = verses;
            SelectedChapter     = chapter;
            ShowChapterList     = false;
            IsChapterFavourite  = await _favouriteService.IsFavouriteAsync($"bible_chapter_{_bookId}_{chapter}");

            // Load cross-refs for verse 1 of the chapter (chapter-level refs)
            try
            {
                var refs = await _bibleService.GetCrossRefsAsync(_bookId, chapter, 1);
                CurrentCrossRefs = refs;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BookViewModel] CrossRefs failed: {ex.Message}");
                CurrentCrossRefs = [];
            }
        }
        finally { IsBusy = false; }
    }
}
