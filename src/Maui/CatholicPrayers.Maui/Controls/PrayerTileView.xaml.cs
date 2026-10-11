using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.Controls;

public partial class PrayerTileView : ContentView
{
    public static readonly BindableProperty TileTitleProperty =
        BindableProperty.Create(nameof(TileTitle), typeof(string), typeof(PrayerTileView), string.Empty);
    public static readonly BindableProperty TileSubtitleProperty =
        BindableProperty.Create(nameof(TileSubtitle), typeof(string), typeof(PrayerTileView), string.Empty,
            propertyChanged: (b, _, _) => ((PrayerTileView)b).OnPropertyChanged(nameof(HasSubtitle)));
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(PrayerTileView), FA.Cross);
    public static readonly BindableProperty TileImageProperty =
        BindableProperty.Create(nameof(TileImage), typeof(string), typeof(PrayerTileView), null,
            propertyChanged: (b, _, _) => ((PrayerTileView)b).UpdateTileImage());
    public static readonly BindableProperty GlowColorProperty =
        BindableProperty.Create(nameof(GlowColor), typeof(Color), typeof(PrayerTileView), Color.FromArgb("#409D7DE8"),
            propertyChanged: (b, _, _) => ((PrayerTileView)b).UpdateGlow());
    public static readonly BindableProperty IsFeaturedProperty =
        BindableProperty.Create(nameof(IsFeatured), typeof(bool), typeof(PrayerTileView), false,
            propertyChanged: (b, _, _) => ((PrayerTileView)b).UpdateFeatured());
    public static readonly BindableProperty IsFavouriteProperty =
        BindableProperty.Create(nameof(IsFavourite), typeof(bool), typeof(PrayerTileView), false,
            propertyChanged: (b, _, _) => ((PrayerTileView)b).UpdateFavourite());

    public string TileTitle
    {
        get => (string)GetValue(TileTitleProperty);
        set => SetValue(TileTitleProperty, value);
    }

    public string TileSubtitle
    {
        get => (string)GetValue(TileSubtitleProperty);
        set => SetValue(TileSubtitleProperty, value);
    }

    public bool HasSubtitle => !string.IsNullOrEmpty(TileSubtitle);

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string? TileImage
    {
        get => (string?)GetValue(TileImageProperty);
        set => SetValue(TileImageProperty, value);
    }

    public Color GlowColor
    {
        get => (Color)GetValue(GlowColorProperty);
        set => SetValue(GlowColorProperty, value);
    }

    public bool IsFeatured
    {
        get => (bool)GetValue(IsFeaturedProperty);
        set => SetValue(IsFeaturedProperty, value);
    }

    public bool IsFavourite
    {
        get => (bool)GetValue(IsFavouriteProperty);
        set => SetValue(IsFavouriteProperty, value);
    }

    public event EventHandler? FavouriteTapped;

    public PrayerTileView()
    {
        InitializeComponent();

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) =>
        {
            IsFavourite = !IsFavourite;
            FavouriteTapped?.Invoke(this, EventArgs.Empty);
        };
        favTapArea.GestureRecognizers.Add(tap);
    }

    private void UpdateTileImage()
    {
        if (tileImage == null || iconLabel == null) return;
        var hasImage = !string.IsNullOrEmpty(TileImage);
        if (hasImage)
            tileImage.Source = ImageSource.FromFile(TileImage!);
        tileImage.IsVisible = hasImage;
        iconLabel.IsVisible = !hasImage;
    }

    private void UpdateGlow()
    {
        if (tileBorder?.Shadow != null)
            tileBorder.Shadow.Brush = new SolidColorBrush(GlowColor);
    }

    private void UpdateFeatured()
    {
        if (tileBorder == null) return;
        tileBorder.StrokeThickness = IsFeatured ? 2.5 : 1.5;
    }

    private void UpdateFavourite()
    {
        if (favHeartLabel == null) return;
        favHeartLabel.TextColor = IsFavourite
            ? Color.FromArgb("#E53935")
            : (Color)(Application.Current?.Resources?["PrayAppSubtitleTextColor"] ?? Colors.Gray);
    }

    public async Task AnimateGlowPulseAsync()
    {
        if (tileBorder?.Shadow == null) return;
        await tileBorder.FadeTo(0.85, 350, Easing.SinInOut);
        await tileBorder.FadeTo(1.0, 350, Easing.SinInOut);
    }
}
