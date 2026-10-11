using Microsoft.Maui.Controls.Shapes;
using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.ViewModels;

namespace CatholicPrayers.Maui.Views;

[QueryProperty(nameof(SectionId), nameof(SectionId))]
public partial class CollectionPage : ContentPage
{
    private readonly CollectionViewModel _vm;
    private string? _sectionId;

    // Filter chip labels in the novena header
    private Label? _chipAll, _chipActive, _chipMonth;

    public string? SectionId
    {
        get => _sectionId;
        set { _sectionId = value; }
    }

    public CollectionPage(CollectionViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
        vm.ItemsLoaded += OnItemsLoaded;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _vm.ItemsLoaded -= OnItemsLoaded;
        _vm.ItemsLoaded += OnItemsLoaded;
        try
        {
            if (!string.IsNullOrEmpty(_sectionId))
                await _vm.LoadAsync(_sectionId);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[CollectionPage.OnAppearing] {ex}");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm.ItemsLoaded -= OnItemsLoaded;
    }

    private void OnItemsLoaded() =>
        MainThread.BeginInvokeOnMainThread(BuildList);

    private void BuildList()
    {
        itemStack.Children.Clear();

        if (_vm.IsNovenaSection)
            BuildNovenaHeader();

        foreach (var entry in _vm.DisplayItems)
            itemStack.Children.Add(BuildRow(entry));
    }

    // ── Novena-specific header ────────────────────────────────────────────────

    private void BuildNovenaHeader()
    {
        var activeNovenas = _vm.ActiveNovenas;
        var currentFilter = _vm.ActiveFilter;

        // ── "Active Now" highlight section ───────────────────────────────────
        if (activeNovenas.Count > 0
            && currentFilter != CollectionViewModel.NovenaFilter.ActiveNow)
        {
            var activeBorder = new Border
            {
                BackgroundColor = Color.FromArgb("#1A4CAF50"),
                Stroke = new SolidColorBrush(Color.FromArgb("#4CAF50")),
                StrokeThickness = 1.5,
                StrokeShape = new RoundRectangle { CornerRadius = 12 },
                Padding = new Thickness(14, 10),
                Margin = new Thickness(0, 0, 0, 4)
            };

            var activeStack = new StackLayout { Spacing = 6 };

            activeStack.Add(new Label
            {
                Text = $"  Active Now ({activeNovenas.Count})",
                FontFamily = "FontAwesome5Solid",
                FontSize = 13,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#4CAF50")
            });

            foreach (var n in activeNovenas)
            {
                var row = BuildCompactActiveRow(n);
                activeStack.Add(row);
            }

            activeBorder.Content = activeStack;
            itemStack.Children.Add(activeBorder);

            // Spacer between active section and main list
            itemStack.Children.Add(new BoxView { HeightRequest = 4, Color = Colors.Transparent });
        }

        // ── Filter chips ──────────────────────────────────────────────────────
        var chipRow = new HorizontalStackLayout
        {
            Spacing = 8,
            Margin = new Thickness(0, 0, 0, 8)
        };

        _chipAll    = BuildChip("All",        currentFilter == CollectionViewModel.NovenaFilter.All);
        _chipActive = BuildChip("Active Now", currentFilter == CollectionViewModel.NovenaFilter.ActiveNow);
        _chipMonth  = BuildChip("By Month",   currentFilter == CollectionViewModel.NovenaFilter.ByMonth);

        AttachChipTap(_chipAll,    CollectionViewModel.NovenaFilter.All);
        AttachChipTap(_chipActive, CollectionViewModel.NovenaFilter.ActiveNow);
        AttachChipTap(_chipMonth,  CollectionViewModel.NovenaFilter.ByMonth);

        chipRow.Add(WrapChip(_chipAll));
        chipRow.Add(WrapChip(_chipActive));
        chipRow.Add(WrapChip(_chipMonth));

        itemStack.Children.Add(chipRow);
    }

    private void AttachChipTap(Label chip, CollectionViewModel.NovenaFilter filter)
    {
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) =>
        {
            _vm.SetFilterCommand.Execute(filter.ToString());
            // BuildList is called via ItemsLoaded event
        };
        chip.GestureRecognizers.Add(tap);
    }

    private View BuildCompactActiveRow(PrayerMasterEntry entry)
    {
        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
            Margin = new Thickness(4, 2, 0, 2)
        };

        grid.Add(new Label
        {
            Text = entry.Title,
            FontSize = 13,
            TextColor = GetColor("PrayAppBodyTextColor"),
            VerticalOptions = LayoutOptions.Center
        }, 0, 0);

        if (!string.IsNullOrEmpty(entry.FeastDate))
            grid.Add(new Label
            {
                Text = $"Feast: {entry.FeastDate}",
                FontSize = 11,
                TextColor = Color.FromArgb("#4CAF50"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            }, 1, 0);

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => _vm.NavigateCommand.Execute(entry);
        grid.GestureRecognizers.Add(tap);
        return grid;
    }

    private static Label BuildChip(string text, bool selected)
    {
        return new Label
        {
            Text = text,
            FontSize = 13,
            FontAttributes = selected ? FontAttributes.Bold : FontAttributes.None,
            TextColor = selected ? Colors.White : GetColor("PrayAppBodyTextColor"),
            BackgroundColor = selected ? Color.FromArgb("#4A90D9") : GetColor("PrayAppCardBgColor"),
            Padding = new Thickness(12, 6),
            VerticalOptions = LayoutOptions.Center
        };
    }

    private static Border WrapChip(Label label)
    {
        return new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            Stroke = new SolidColorBrush(GetColor("PrayAppCardBorderColor")),
            StrokeThickness = 1,
            BackgroundColor = label.BackgroundColor,
            Padding = 0,
            Content = label
        };
    }

    // ── Main row builder ─────────────────────────────────────────────────────

    private View BuildRow(PrayerMasterEntry entry)
    {
        var accent = ParseColor(entry.AccentColor);
        var imageFile = !string.IsNullOrEmpty(entry.ImageFile) &&
                        !entry.ImageFile.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)
            ? entry.ImageFile : null;

        var border = new Border
        {
            BackgroundColor = GetColor("PrayAppCardBgColor"),
            Stroke = new SolidColorBrush(GetColor("PrayAppCardBorderColor")),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 14 },
            Padding = imageFile != null
                ? new Thickness(10, 10, 16, 10)
                : new Thickness(16, 14)
        };

        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(imageFile != null ? 52 : 40),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(36)
            },
            ColumnSpacing = 0
        };

        // Icon — real photo if available (non-SVG ImageFile), otherwise FA5 glyph
        View iconView;
        if (imageFile != null)
        {
            iconView = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = 10 },
                Stroke = new SolidColorBrush(accent.WithAlpha(0.55f)),
                StrokeThickness = 1.5,
                BackgroundColor = Colors.Transparent,
                WidthRequest = 44,
                HeightRequest = 44,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Padding = 0,
                Shadow = new Shadow
                {
                    Brush = new SolidColorBrush(accent.WithAlpha(0.55f)),
                    Offset = new Point(0, 2),
                    Radius = 10,
                    Opacity = 0.80f
                },
                Content = new Image
                {
                    Source = ImageSource.FromFile(imageFile),
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill
                }
            };
        }
        else
        {
            iconView = new Label
            {
                Text = string.IsNullOrEmpty(entry.Icon) ? NodeTypeIcon(entry.NodeType) : entry.Icon,
                FontFamily = "FontAwesome5Solid",
                FontSize = 22,
                TextColor = accent,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
        }
        grid.Add(iconView, 0, 0);

        // Title + subtitle + optional active badge
        var textStack = new StackLayout
        {
            Spacing = 2,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(imageFile != null ? 10 : 0, 0, 0, 0)
        };

        // Title row — with "Active" badge for novenas that are running now
        if (entry.NodeType == "Novena" && NovenaDateHelper.IsActiveToday(entry))
        {
            var titleRow = new HorizontalStackLayout { Spacing = 6 };
            titleRow.Add(new Label
            {
                Text = entry.Title,
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
                TextColor = GetColor("PrayAppH2TextColor"),
                MaxLines = 1,
                LineBreakMode = LineBreakMode.TailTruncation,
                VerticalOptions = LayoutOptions.Center
            });
            titleRow.Add(new Border
            {
                BackgroundColor = Color.FromArgb("#4CAF50"),
                StrokeShape = new RoundRectangle { CornerRadius = 6 },
                StrokeThickness = 0,
                Padding = new Thickness(5, 1),
                Content = new Label
                {
                    Text = "Active",
                    FontSize = 10,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White,
                    VerticalOptions = LayoutOptions.Center
                }
            });
            textStack.Add(titleRow);
        }
        else
        {
            textStack.Add(new Label
            {
                Text = entry.Title,
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
                TextColor = GetColor("PrayAppH2TextColor"),
                MaxLines = 1,
                LineBreakMode = LineBreakMode.TailTruncation
            });
        }

        var subtitleText = entry.NodeType == "Novena" && !string.IsNullOrEmpty(entry.FeastDate)
            ? $"Feast: {entry.FeastDate}"
            : entry.Subtitle;

        if (!string.IsNullOrEmpty(subtitleText))
            textStack.Add(new Label
            {
                Text = subtitleText,
                FontSize = 12,
                TextColor = GetColor("PrayAppSubtitleTextColor"),
                MaxLines = 1,
                LineBreakMode = LineBreakMode.TailTruncation
            });

        grid.Add(textStack, 1, 0);

        // Right: heart (favouritable) or chevron
        var canFav = entry.NodeType is "Prayer" or "Novena";
        if (canFav)
        {
            var heartLabel = new Label
            {
                Text = "",
                FontFamily = "FontAwesome5Solid",
                FontSize = 20,
                TextColor = entry.IsFavourite ? Color.FromArgb("#E53935") : GetColor("PrayAppSubtitleTextColor"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(4, 0)
            };
            var heartTap = new TapGestureRecognizer();
            heartTap.Tapped += (_, _) =>
            {
                entry.IsFavourite = !entry.IsFavourite;
                heartLabel.TextColor = entry.IsFavourite
                    ? Color.FromArgb("#E53935")
                    : GetColor("PrayAppSubtitleTextColor");
                _vm.ToggleFavouriteCommand.Execute(entry);
            };
            heartLabel.GestureRecognizers.Add(heartTap);
            grid.Add(heartLabel, 2, 0);
        }
        else
        {
            grid.Add(new Label
            {
                Text = "",
                FontFamily = "FontAwesome5Solid",
                FontSize = 14,
                TextColor = GetColor("PrayAppSubtitleTextColor"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            }, 2, 0);
        }

        border.Content = grid;

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => _vm.NavigateCommand.Execute(entry);
        border.GestureRecognizers.Add(tap);

        return border;
    }

    private static string NodeTypeIcon(string nodeType) => nodeType switch
    {
        "Collection"   => "", // folder
        "Novena"       => "", // clock
        "Bible"        => "", // book-open
        "ExternalLink" => "", // external-link-alt
        _              => ""  // key (fallback)
    };

    private static Color ParseColor(string hex)
    {
        try { return Color.FromArgb(hex); }
        catch { return Colors.Gray; }
    }

    private static Color GetColor(string key)
    {
        if (Application.Current?.Resources?.TryGetValue(key, out var val) == true && val is Color c)
            return c;
        return Colors.Gray;
    }
}
