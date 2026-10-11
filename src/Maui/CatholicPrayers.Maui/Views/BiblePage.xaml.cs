using Microsoft.Maui.Controls.Shapes;
using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.ViewModels;

namespace CatholicPrayers.Maui.Views;

public partial class BiblePage : ContentPage
{
    private readonly BibleViewModel _vm;
    private static readonly Color AccentGreen = Color.FromArgb("#27AE60");
    private static readonly Color FavRedColor = Color.FromArgb("#E53935");

    private List<View>? _otViews;
    private List<View>? _ntViews;
    private bool _loaded;
    private readonly Dictionary<string, Label> _favLabels = [];

    public BiblePage(BibleViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
        vm.PropertyChanged += OnVmPropertyChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _vm.PropertyChanged += OnVmPropertyChanged;
        try
        {
            if (!_loaded)
            {
                await _vm.LoadAsync();
                _otViews = BuildBookViews(_vm.OldTestament);
                _ntViews = BuildBookViews(_vm.NewTestament);
                _loaded = true;
            }
            else
            {
                await _vm.StampFavsAsync();
                RefreshFavLabels();
            }
            BuildBookList();
            UpdateToggleButtons();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[BiblePage.OnAppearing] {ex}");
        }
    }

    private void RefreshFavLabels()
    {
        foreach (var (bookId, label) in _favLabels)
        {
            var book = _vm.OldTestament.FirstOrDefault(b => b.Id == bookId)
                    ?? _vm.NewTestament.FirstOrDefault(b => b.Id == bookId);
            if (book != null)
                label.TextColor = book.IsFavourite ? FavRedColor : GetColor("PrayAppSubtitleTextColor");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm.PropertyChanged -= OnVmPropertyChanged;
    }

    private void OnVmPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BibleViewModel.ActiveTestament))
            MainThread.BeginInvokeOnMainThread(() => { BuildBookList(); UpdateToggleButtons(); });

        if (e.PropertyName == nameof(BibleViewModel.SearchResults))
            MainThread.BeginInvokeOnMainThread(BuildSearchResults);
    }

    private void UpdateToggleButtons()
    {
        var isOT = _vm.ActiveTestament == "OT";
        btnOT.BackgroundColor = isOT  ? AccentGreen : GetColor("PrayAppCardBgColor");
        btnOT.TextColor       = isOT  ? Colors.White : GetColor("PrayAppBodyTextColor");
        btnNT.BackgroundColor = !isOT ? AccentGreen : GetColor("PrayAppCardBgColor");
        btnNT.TextColor       = !isOT ? Colors.White : GetColor("PrayAppBodyTextColor");
    }

    private List<View> BuildBookViews(IEnumerable<BibleBook> books)
    {
        var views = new List<View>();
        string? lastCategory = null;
        foreach (var book in books)
        {
            if (book.Category != lastCategory)
            {
                lastCategory = book.Category;
                views.Add(BuildCategoryHeader(book.Category));
            }
            views.Add(BuildBookRow(book));
        }
        return views;
    }

    private void BuildBookList()
    {
        bookStack.Children.Clear();
        var views = _vm.ActiveTestament == "OT" ? _otViews : _ntViews;
        if (views == null) return;
        foreach (var v in views)
            bookStack.Children.Add(v);
    }

    private void BuildSearchResults()
    {
        searchStack.Children.Clear();
        var results = _vm.SearchResults;

        if (results.Count == 0)
        {
            searchStack.Children.Add(new Label
            {
                Text = "No results found.",
                FontSize = 15,
                TextColor = GetColor("PrayAppSubtitleTextColor"),
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 40)
            });
            return;
        }

        searchStack.Children.Add(new Label
        {
            Text = $"{results.Count} result{(results.Count == 1 ? "" : "s")}",
            FontSize = 12,
            FontAttributes = FontAttributes.Bold,
            TextColor = GetColor("PrayAppSubtitleTextColor"),
            Margin = new Thickness(4, 0, 0, 8)
        });

        foreach (var result in results)
            searchStack.Children.Add(BuildSearchResultRow(result));
    }

    private View BuildSearchResultRow(BibleSearchResult result)
    {
        var border = new Border
        {
            BackgroundColor = GetColor("PrayAppCardBgColor"),
            Stroke = new SolidColorBrush(GetColor("PrayAppCardBorderColor")),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = new Thickness(14, 10),
            Margin = new Thickness(0, 0, 0, 8)
        };

        var stack = new StackLayout { Spacing = 4 };

        stack.Add(new Label
        {
            Text = result.Reference,
            FontSize = 12,
            FontAttributes = FontAttributes.Bold,
            TextColor = AccentGreen
        });
        stack.Add(new Label
        {
            Text = result.Text,
            FontSize = 14,
            TextColor = GetColor("PrayAppBodyTextColor"),
            LineHeight = 1.45,
            LineBreakMode = LineBreakMode.WordWrap
        });

        border.Content = stack;

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => _vm.OpenSearchResultCommand.Execute(result);
        border.GestureRecognizers.Add(tap);

        return border;
    }

    private static View BuildCategoryHeader(string category)
        => new Label
        {
            Text = FormatCategory(category),
            FontSize = 11,
            FontAttributes = FontAttributes.Bold,
            TextColor = AccentGreen,
            Margin = new Thickness(4, 8, 0, 2),
            CharacterSpacing = 1
        };

    private View BuildBookRow(BibleBook book)
    {
        var border = new Border
        {
            BackgroundColor = GetColor("PrayAppCardBgColor"),
            Stroke = new SolidColorBrush(GetColor("PrayAppCardBorderColor")),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = new Thickness(14, 10)
        };

        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(44),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        grid.Add(new Label
        {
            Text = book.Abbreviation,
            FontSize = 11,
            FontAttributes = FontAttributes.Bold,
            TextColor = AccentGreen,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center
        }, 0, 0);

        var textStack = new StackLayout { Spacing = 1, VerticalOptions = LayoutOptions.Center };
        textStack.Add(new Label { Text = book.Title, FontSize = 15, TextColor = GetColor("PrayAppH2TextColor") });
        textStack.Add(new Label
        {
            Text = $"{book.ChapterCount} chapter{(book.ChapterCount == 1 ? "" : "s")}",
            FontSize = 12,
            TextColor = GetColor("PrayAppSubtitleTextColor")
        });
        grid.Add(textStack, 1, 0);

        var favLabel = new Label
        {
            Text = "",
            FontFamily = "FontAwesome5Solid",
            FontSize = 18,
            TextColor = book.IsFavourite ? FavRedColor : GetColor("PrayAppSubtitleTextColor"),
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(8, 0)
        };
        _favLabels[book.Id] = favLabel;
        var favTap = new TapGestureRecognizer();
        favTap.Tapped += (_, _) =>
        {
            _vm.ToggleBookFavCommand.Execute(book);
            favLabel.TextColor = book.IsFavourite ? FavRedColor : GetColor("PrayAppSubtitleTextColor");
        };
        favLabel.GestureRecognizers.Add(favTap);
        grid.Add(favLabel, 2, 0);

        grid.Add(new Label
        {
            Text = "",
            FontFamily = "FontAwesome5Solid",
            FontSize = 14,
            TextColor = GetColor("PrayAppSubtitleTextColor"),
            VerticalOptions = LayoutOptions.Center
        }, 3, 0);

        border.Content = grid;

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => _vm.OpenBookCommand.Execute(book);
        border.GestureRecognizers.Add(tap);

        return border;
    }

    private static string FormatCategory(string cat) => cat switch
    {
        "pentateuch"       => "PENTATEUCH",
        "historical"       => "HISTORICAL BOOKS",
        "wisdom"           => "WISDOM & POETRY",
        "deuterocanonical" => "DEUTEROCANONICAL",
        "prophetic"        => "PROPHETS",
        "gospel"           => "GOSPELS",
        "history"          => "ACTS OF THE APOSTLES",
        "epistle"          => "EPISTLES",
        "apocalyptic"      => "APOCALYPTIC",
        _                  => cat.ToUpperInvariant()
    };

    private static Color GetColor(string key)
    {
        if (Application.Current?.Resources?.TryGetValue(key, out var val) == true && val is Color c)
            return c;
        return Colors.Gray;
    }
}
