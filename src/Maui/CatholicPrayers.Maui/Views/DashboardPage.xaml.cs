using CatholicPrayers.Maui.Controls;
using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.ViewModels;

namespace CatholicPrayers.Maui.Views;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _vm;
    private bool _loaded;

    public DashboardPage(DashboardViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;

        vm.NavigateToSettingsCommand = new Command(async () =>
            await Shell.Current.GoToAsync("SettingsPage"));

        vm.PrayersLoaded += OnPrayersLoaded;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(vm.ShowFavouritesOnly))
                UpdateFavFilterButton();
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (!_loaded)
            {
                _loaded = true;
                await _vm.LoadAsync();
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(BuildDashboard);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DashboardPage.OnAppearing] {ex}");
        }
    }

    // ── Build tiles after data loads ─────────────────────────────────────────

    private void OnPrayersLoaded()
    {
        MainThread.BeginInvokeOnMainThread(BuildDashboard);
    }

    private void BuildDashboard()
    {
        if (_vm.ShowFavouritesOnly)
        {
            featuredSection.IsVisible = false;
            BuildFavouritesList();
        }
        else
        {
            featuredSection.IsVisible = true;
            BuildFeatured();
            BuildSections();
        }
    }

    private void BuildFavouritesList()
    {
        sectionStack.Children.Clear();
        var items = _vm.FavouriteItems;

        if (items.Count == 0)
        {
            sectionStack.Children.Add(new Label
            {
                Text = "No favourites yet — tap ♡ on any prayer, novena, or Bible book.",
                FontSize = 15,
                TextColor = GetDynamicColor("PrayAppSubtitleTextColor"),
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(24, 48)
            });
            return;
        }

        foreach (var item in items)
            sectionStack.Children.Add(BuildFavouriteCard(item));
    }

    private View BuildFavouriteCard(FavouriteDisplayItem item)
    {
        Color accent;
        try { accent = Color.FromArgb(item.AccentColor); }
        catch { accent = GetDynamicColor("PrayAppAccentColor"); }

        var border = new Border
        {
            BackgroundColor = GetDynamicColor("PrayAppCardBgColor"),
            Stroke = new SolidColorBrush(GetDynamicColor("PrayAppCardBorderColor")),
            StrokeThickness = 1.5,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 14 },
            Padding = new Thickness(16, 14),
            Margin = new Thickness(0, 0, 0, 10)
        };

        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(44),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        // Icon circle
        grid.Add(new Border
        {
            BackgroundColor = accent.WithAlpha(0.15f),
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 11 },
            WidthRequest = 38,
            HeightRequest = 38,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Content = new Label
            {
                Text = item.Icon,
                FontFamily = "FontAwesome5Solid",
                FontSize = 16,
                TextColor = accent,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            }
        }, 0, 0);

        // Title + type badge
        var textStack = new StackLayout { Spacing = 2, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(10, 0, 0, 0) };
        textStack.Add(new Label
        {
            Text = item.Title,
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            TextColor = GetDynamicColor("PrayAppH1TextColor"),
            LineBreakMode = LineBreakMode.TailTruncation
        });

        var typePill = new Border
        {
            BackgroundColor = accent.WithAlpha(0.18f),
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 8 },
            Padding = new Thickness(6, 2),
            HorizontalOptions = LayoutOptions.Start,
            Content = new Label
            {
                Text = item.Type,
                FontSize = 10,
                TextColor = accent,
                FontAttributes = FontAttributes.Bold
            }
        };
        textStack.Add(typePill);
        grid.Add(textStack, 1, 0);

        // Chevron
        grid.Add(new Label
        {
            Text = "",
            FontFamily = "FontAwesome5Solid",
            FontSize = 13,
            TextColor = accent,
            VerticalOptions = LayoutOptions.Center
        }, 2, 0);

        border.Content = grid;

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (_, _) => await Shell.Current.GoToAsync(item.Route);
        border.GestureRecognizers.Add(tap);

        return border;
    }

    private void BuildFeatured()
    {
        featuredStack.Children.Clear();
        foreach (var prayer in _vm.FeaturedPrayers)
        {
            var tile = MakeTile(prayer, width: 130, height: 160, isFeatured: true);
            featuredStack.Children.Add(tile);
        }
    }

    private void BuildSections()
    {
        sectionStack.Children.Clear();
        foreach (var section in _vm.Sections)
            sectionStack.Children.Add(BuildSectionCard(section));
    }

    private View BuildSectionCard(PrayerMasterEntry section)
    {
        Color accent;
        try { accent = Color.FromArgb(section.AccentColor); }
        catch { accent = GetDynamicColor("PrayAppAccentColor"); }

        var border = new Border
        {
            BackgroundColor = GetDynamicColor("PrayAppCardBgColor"),
            Stroke = new SolidColorBrush(GetDynamicColor("PrayAppCardBorderColor")),
            StrokeThickness = 1.5,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 18 },
            Padding = new Thickness(20, 18),
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(accent.WithAlpha(0.25f)),
                Offset = new Point(0, 4),
                Radius = 12,
                Opacity = 0.7f
            }
        };

        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(56),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        // Icon — real image if available (PartnerApp icon or section ImageFile), FA5 glyph otherwise
        var sectionIconImage = section.NodeType == "PartnerApp"
            ? section.PartnerAppIconImage
            : (!string.IsNullOrEmpty(section.ImageFile) ? section.ImageFile : null);

        var iconBorderStyle = new Border
        {
            BackgroundColor = accent.WithAlpha(0.15f),
            Stroke = new SolidColorBrush(accent.WithAlpha(0.50f)),
            StrokeThickness = 1.5,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 14 },
            WidthRequest = 48,
            HeightRequest = 48,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(accent.WithAlpha(0.60f)),
                Offset = new Point(0, 2),
                Radius = 10,
                Opacity = 0.85f
            }
        };

        View iconView;
        if (!string.IsNullOrEmpty(sectionIconImage))
        {
            iconBorderStyle.Content = new Image
            {
                Source = sectionIconImage,
                Aspect = Aspect.AspectFit,
                WidthRequest = 36,
                HeightRequest = 36,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            iconView = iconBorderStyle;
        }
        else
        {
            iconBorderStyle.Content = new Label
            {
                Text = section.Icon,
                FontFamily = "FontAwesome5Solid",
                FontSize = 20,
                TextColor = accent,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            iconView = iconBorderStyle;
        }
        grid.Add(iconView, 0, 0);

        // Title + subtitle
        var textStack = new StackLayout { Spacing = 3, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(12, 0, 0, 0) };
        textStack.Add(new Label
        {
            Text = section.Title,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = GetDynamicColor("PrayAppH1TextColor")
        });

        var subtitleText = SectionSubtitle(section);
        if (!string.IsNullOrEmpty(subtitleText))
            textStack.Add(new Label
            {
                Text = subtitleText,
                FontSize = 12,
                TextColor = GetDynamicColor("PrayAppSubtitleTextColor")
            });
        grid.Add(textStack, 1, 0);

        // Chevron
        grid.Add(new Label
        {
            Text = "",
            FontFamily = "FontAwesome5Solid",
            FontSize = 14,
            TextColor = accent,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.End
        }, 2, 0);

        border.Content = grid;

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => _vm.NavigateToPrayerCommand.Execute(section);
        border.GestureRecognizers.Add(tap);

        return border;
    }

    private static string SectionSubtitle(PrayerMasterEntry section) => section.Id switch
    {
        "section_novenas"  => $"{section.Subtitle}",
        "section_prayers"  => section.Subtitle,
        _                  => section.Subtitle
    };

    private View MakeTile(PrayerMasterEntry prayer, double height = 150, double width = -1, bool isFeatured = false)
    {
        var captured = prayer;
        // Use the prayer image if it's a real photo (not SVG), otherwise fall back to glyph
        var imageFile = !string.IsNullOrEmpty(prayer.ImageFile) &&
                        !prayer.ImageFile.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)
            ? prayer.ImageFile : null;

        var tile = new PrayerTileView
        {
            TileTitle = prayer.Title,
            TileSubtitle = prayer.Subtitle ?? string.Empty,
            Icon = prayer.Icon,
            TileImage = imageFile,
            IsFeatured = isFeatured,
            IsFavourite = prayer.IsFavourite,
            HeightRequest = height
        };
        if (width > 0) tile.WidthRequest = width;

        // Tap tile → open prayer
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => _vm.NavigateToPrayerCommand.Execute(captured);
        tile.GestureRecognizers.Add(tap);

        // Tap heart badge → toggle favourite
        tile.FavouriteTapped += (_, _) =>
        {
            captured.IsFavourite = tile.IsFavourite;
            _vm.ToggleFavouriteCommand.Execute(captured);
        };

        return tile;
    }

    private void UpdateFavFilterButton()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var isActive = _vm.ShowFavouritesOnly;
            favFilterButton.TextColor = isActive
                ? Color.FromArgb("#E53935")
                : GetDynamicColor("PrayAppSubtitleTextColor");
        });
    }

    private Color GetDynamicColor(string key)
    {
        if (Application.Current?.Resources?.TryGetValue(key, out var val) == true && val is Color c)
            return c;
        return Colors.Gray;
    }

    private void OnHamburgerClicked(object sender, EventArgs e)
        => Shell.Current.FlyoutIsPresented = true;

    // ── Search focus handling ────────────────────────────────────────────────

    private void OnSearchFocused(object sender, FocusEventArgs e)
    {
        // Entry is now active; keyboard will appear automatically
    }

    private void OnSearchUnfocused(object sender, FocusEventArgs e)
    {
        // Entry lost focus — nothing extra needed
    }

    private void OnClearSearchClicked(object sender, EventArgs e)
    {
        searchEntry.Unfocus();
    }

    // Make tapping anywhere on the search Border immediately focus the Entry
    private void OnSearchBarTapped(object sender, TappedEventArgs e)
    {
        searchEntry.Focus();
    }
}
