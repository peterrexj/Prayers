using Microsoft.Maui.Controls.Shapes;
using CatholicPrayers.Maui.Services;
using CatholicPrayers.Maui.ViewModels;

namespace CatholicPrayers.Maui.Views;

[QueryProperty(nameof(BookId),  nameof(BookId))]
[QueryProperty(nameof(Chapter), nameof(Chapter))]
[QueryProperty(nameof(Verse),   nameof(Verse))]
public partial class BookPage : ContentPage
{
    private readonly BookViewModel _vm;
    private readonly IFavouriteService _favouriteService;
    private string? _bookId;
    private string _bookIdLocal = string.Empty;
    private int _jumpChapter;
    private int _jumpVerse;

    public string? BookId  { get => _bookId; set => _bookId = value; }
    public int Chapter     { get => _jumpChapter; set => _jumpChapter = value; }
    public int Verse       { get => _jumpVerse;   set => _jumpVerse   = value; }

    public BookPage(BookViewModel vm, IFavouriteService favouriteService)
    {
        InitializeComponent();
        _vm = vm;
        _favouriteService = favouriteService;
        BindingContext = vm;
        vm.PropertyChanged += OnVmPropertyChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (!string.IsNullOrEmpty(_bookId))
            {
                _bookIdLocal = _bookId;
                await _vm.LoadAsync(_bookId, _jumpChapter);
                BuildChapterGrid();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[BookPage.OnAppearing] {ex}");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm.PropertyChanged -= OnVmPropertyChanged;
    }

    private void OnVmPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BookViewModel.CurrentVerses) ||
            e.PropertyName == nameof(BookViewModel.SelectedChapter))
            MainThread.BeginInvokeOnMainThread(BuildVerseView);

        if (e.PropertyName == nameof(BookViewModel.CurrentCrossRefs))
            MainThread.BeginInvokeOnMainThread(AppendCrossRefs);

        if (e.PropertyName == nameof(BookViewModel.ChapterNumbers))
            MainThread.BeginInvokeOnMainThread(BuildChapterGrid);
    }

    // ── Chapter grid ─────────────────────────────────────────────────────────

    private void BuildChapterGrid()
    {
        chapterStack.Children.Clear();

        var numbers = _vm.ChapterNumbers;
        if (!numbers.Any())
        {
            chapterStack.Children.Add(new Label
            {
                Text = "Content loading…",
                FontSize = 15,
                TextColor = GetColor("PrayAppSubtitleTextColor"),
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 40)
            });
            return;
        }

        const int cols = 4;
        var rows = (int)Math.Ceiling(numbers.Count / (double)cols);

        for (int row = 0; row < rows; row++)
        {
            var grid = new Grid
            {
                ColumnSpacing = 10,
                Margin = new Thickness(0, 0, 0, 10)
            };
            for (int c = 0; c < cols; c++)
                grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

            for (int col = 0; col < cols; col++)
            {
                var idx = row * cols + col;
                if (idx >= numbers.Count) break;
                var tile = BuildChapterTile(numbers[idx]);
                Grid.SetColumn(tile, col);
                grid.Children.Add(tile);
            }
            chapterStack.Children.Add(grid);
        }
    }

    private View BuildChapterTile(int chapterNumber)
    {
        var isFav = _vm.FavouritedChapters.Contains(chapterNumber);
        var accentGreen = Color.FromArgb("#27AE60");
        var favRed = Color.FromArgb("#E53935");
        var subtleColor = GetColor("PrayAppSubtitleTextColor");

        var tile = new Border
        {
            HeightRequest   = 72,
            BackgroundColor = GetColor("PrayAppCardBgColor"),
            Stroke          = new SolidColorBrush(accentGreen),
            StrokeThickness = 1.5,
            StrokeShape     = new RoundRectangle { CornerRadius = 12 },
            Padding         = new Thickness(4)
        };

        var heartLabel = new Label
        {
            Text                    = "",
            FontFamily              = "FontAwesome5Solid",
            FontSize                = 11,
            TextColor               = isFav ? favRed : subtleColor,
            HorizontalOptions       = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center
        };

        tile.Content = new StackLayout
        {
            VerticalOptions   = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Spacing           = 2,
            Children =
            {
                new Label
                {
                    Text                    = chapterNumber.ToString(),
                    FontSize                = 20,
                    FontAttributes          = FontAttributes.Bold,
                    TextColor               = accentGreen,
                    HorizontalOptions       = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                },
                heartLabel
            }
        };

        var heartTap = new TapGestureRecognizer();
        heartTap.Tapped += async (_, _) =>
        {
            var nowFav = !_vm.FavouritedChapters.Contains(chapterNumber);
            if (nowFav)
                _vm.FavouritedChapters.Add(chapterNumber);
            else
                _vm.FavouritedChapters.Remove(chapterNumber);
            heartLabel.TextColor = nowFav ? favRed : subtleColor;
            await _favouriteService.SetAsync($"bible_chapter_{_bookIdLocal}_{chapterNumber}", nowFav);
        };
        heartLabel.GestureRecognizers.Add(heartTap);

        var tileTap = new TapGestureRecognizer();
        tileTap.Tapped += (_, _) => _vm.OpenChapterCommand.Execute(chapterNumber);
        tile.GestureRecognizers.Add(tileTap);

        return tile;
    }

    // ── Verse view ────────────────────────────────────────────────────────────

    private void BuildVerseView()
    {
        verseStack.Children.Clear();
        var verses = _vm.CurrentVerses;
        if (verses == null || verses.Count == 0) return;

        verseStack.Children.Add(new Label
        {
            Text           = $"Chapter {_vm.SelectedChapter}",
            FontSize       = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor      = Color.FromArgb("#27AE60"),
            Margin         = new Thickness(0, 0, 0, 16)
        });

        foreach (var verse in verses.OrderBy(v => v.VerseNumber))
        {
            var row = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(32) },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                Margin = new Thickness(0, 0, 0, 12)
            };

            var numLabel = new Label
            {
                Text                    = verse.VerseNumber.ToString(),
                FontSize                = 11,
                FontAttributes          = FontAttributes.Bold,
                TextColor               = Color.FromArgb("#27AE60"),
                VerticalOptions         = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.End,
                Margin                  = new Thickness(0, 3, 6, 0)
            };
            Grid.SetColumn(numLabel, 0);

            var textLabel = new Label
            {
                Text              = verse.Text,
                FontSize          = 16,
                TextColor         = GetColor("PrayAppBodyTextColor"),
                LineHeight        = 1.55,
                LineBreakMode     = LineBreakMode.WordWrap,
                HorizontalOptions = LayoutOptions.Fill
            };
            Grid.SetColumn(textLabel, 1);

            row.Children.Add(numLabel);
            row.Children.Add(textLabel);
            verseStack.Children.Add(row);
        }
    }

    private void AppendCrossRefs()
    {
        var refs = _vm.CurrentCrossRefs;
        if (refs == null || refs.Count == 0) return;

        // Remove any existing cross-ref section (tagged by AutomationId)
        var existing = verseStack.Children
            .FirstOrDefault(v => v is VisualElement ve && ve.AutomationId == "xref-section");
        if (existing != null)
            verseStack.Children.Remove(existing);

        var section = new StackLayout
        {
            AutomationId = "xref-section",
            Spacing      = 8,
            Margin       = new Thickness(0, 24, 0, 8)
        };

        section.Add(new BoxView
        {
            HeightRequest   = 1,
            Color           = GetColor("PrayAppCardBorderColor"),
            Margin          = new Thickness(0, 0, 0, 8)
        });

        section.Add(new Label
        {
            Text           = "Related Verses",
            FontSize       = 13,
            FontAttributes = FontAttributes.Bold,
            TextColor      = Color.FromArgb("#27AE60"),
            CharacterSpacing = 0.5,
            Margin         = new Thickness(0, 0, 0, 4)
        });

        foreach (var xref in refs)
        {
            var row = new Border
            {
                BackgroundColor = GetColor("PrayAppCardBgColor"),
                Stroke          = new SolidColorBrush(GetColor("PrayAppCardBorderColor")),
                StrokeThickness = 1,
                StrokeShape     = new RoundRectangle { CornerRadius = 8 },
                Padding         = new Thickness(12, 8),
                Margin          = new Thickness(0, 0, 0, 6)
            };

            var inner = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };

            inner.Add(new Label
            {
                Text            = xref.Reference,
                FontSize        = 14,
                FontAttributes  = FontAttributes.Bold,
                TextColor       = GetColor("PrayAppH2TextColor"),
                VerticalOptions = LayoutOptions.Center
            }, 0, 0);

            inner.Add(new Label
            {
                Text            = "",
                FontFamily      = "FontAwesome5Solid",
                FontSize        = 12,
                TextColor       = GetColor("PrayAppSubtitleTextColor"),
                VerticalOptions = LayoutOptions.Center
            }, 1, 0);

            row.Content = inner;

            var xrefCopy = xref;
            var tap = new TapGestureRecognizer();
            tap.Tapped += async (_, _) =>
            {
                if (string.IsNullOrEmpty(xrefCopy.ToBook)) return;
                await Shell.Current.GoToAsync(
                    $"BookPage?BookId={Uri.EscapeDataString(xrefCopy.ToBook)}&Chapter={xrefCopy.ToChapter}&Verse={xrefCopy.ToVerseStart}");
            };
            row.GestureRecognizers.Add(tap);
            section.Add(row);
        }

        verseStack.Children.Add(section);
    }

    private static Color GetColor(string key)
    {
        if (Application.Current?.Resources?.TryGetValue(key, out var val) == true && val is Color c)
            return c;
        return Colors.Gray;
    }
}
