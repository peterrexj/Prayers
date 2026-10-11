using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Shapes;
using Prayers.StJoseph.Maui.Controls;
using Prayers.StJoseph.Maui.Extensions;
using Prayers.StJoseph.Maui.Models;
using Prayers.StJoseph.Maui.Services;
using Prayers.StJoseph.Maui.ViewModels;

namespace Prayers.StJoseph.Maui.Views;

[QueryProperty(nameof(PageId), nameof(PageId))]
public partial class PrayerView : ContentPage
{
    private PrayerViewModel _viewModel = null!;
    private readonly List<ParaContentView> _paraContentViews = [];
    private readonly List<ParaBulletView> _paraBulletViews = [];

    // Colors for the custom step indicator
    private Color _stepCompleted  = Color.FromArgb("#0B5345");
    private Color _stepInProgress = Color.FromArgb("#1A5276");
    private Color _stepNotStarted = Color.FromArgb("#2D3E50");
    private Color _connectorColor = Color.FromArgb("#5D6D7E");

    // FontAwesome5Solid glyphs
    private const string GlyphPray  = ""; // fa-pray
    private const string GlyphCheck = ""; // fa-check

    public string? PageId { get; set; }

    public PrayerView()
    {
        InitializeComponent();
        _viewModel = new PrayerViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        _viewModel.IsBusy = true;
        try
        {
            await SharedServices.EnsureLoadedAsync();

            _viewModel.PrayerMainTitle = SharedServices.PrayerData.MainTitle;

            var theme = ServiceLocator.GetService<IThemeService>();
            if (theme != null)
            {
                _viewModel.DefaultStyle = theme.GetDefaultStyleTheme();
                UpdateStepColors(_viewModel.DefaultStyle);
            }

            _paraContentViews.Clear();
            _paraBulletViews.Clear();
            contentStack.Children.Clear();
            _viewModel.AudioFiles.Clear();

            base.OnAppearing();

            int currentPageId = int.TryParse(PageId, out var pid) ? pid
                : SharedServices.PrayerData.SinglePageDataModels.FirstOrDefault()?.PageId ?? 1;

            var pageData = SharedServices.PrayerData.SinglePageDataModels
                .FirstOrDefault(p => p.PageId == currentPageId);

            if (pageData == null) return;

            _viewModel.SinglePageDataModel = pageData;

            BuildStepGrid(_viewModel.PageIndex);
            if (_viewModel.DefaultStyle != null)
                ApplyPillColors(_viewModel.DefaultStyle);

            var settings = ServiceLocator.GetService<SettingsViewModel>();
            double fontDelta = settings?.CurrentFontSize ?? 0;
            _viewModel.CurrentFontDelta = fontDelta;

            var fontColor = _viewModel.DefaultStyle?.DefaultFontColor;
            var textColor = string.IsNullOrEmpty(fontColor) ? Colors.Black : Color.FromArgb(fontColor);
            var boxColor = _viewModel.DefaultStyle?.BoxFontColor;
            var headerTextColor = string.IsNullOrEmpty(boxColor) ? Colors.White : Color.FromArgb(boxColor);
            var borderColor = _viewModel.DefaultStyle?.ButtonBgColor;
            var headerBorderColor = string.IsNullOrEmpty(borderColor) ? Colors.Gray : Color.FromArgb(borderColor);
            var isDark = _viewModel.DefaultStyle?.AppTheme == AppThemes.Dark;
            var headerBgColor = isDark ? Color.FromArgb("#2C3E50") : Color.FromArgb("#1A5276");

            foreach (var item in pageData.Content)
            {
                if (item.ContentType == "H")
                {
                    contentStack.Children.Add(new ParaHeaderView
                    {
                        ParaHeaderContent = item.Content,
                        FontSize = item.FontSize,
                        TextColor = headerTextColor,
                        BorderColor = headerBorderColor,
                        BoxBackgroundColor = headerBgColor,
                    });
                }
                else if (item.ContentType == "P")
                {
                    var para = new ParaContentView
                    {
                        ParaContent = item.Content,
                        FontSize = item.FontSize + fontDelta,
                        TextWrap = item.TextWrap,
                        FontAlign = item.FontAlign,
                        FontCustomAttributes = item.FontAttribute,
                        TextColor = textColor,
                    };
                    _paraContentViews.Add(para);
                    contentStack.Children.Add(para);
                }
                else if (item.ContentType == "P_Bullet")
                {
                    var para = new ParaBulletView
                    {
                        ParaContent = item.Content,
                        ParaNumber = item.AdditionalData,
                        FontSize = item.FontSize + fontDelta,
                        TextWrap = item.TextWrap,
                        FontAlign = item.FontAlign,
                        FontCustomAttributes = item.FontAttribute,
                        TextColor = textColor,
                    };
                    _paraBulletViews.Add(para);
                    contentStack.Children.Add(para);
                }
                else if (item.ContentType == "Sound" && !string.IsNullOrWhiteSpace(item.Content))
                {
                    _viewModel.AudioFiles.Add(item.Content.Trim());
                }
            }

            _viewModel.FontSizeChanged -= OnFontSizeChanged;
            _viewModel.FontSizeChanged += OnFontSizeChanged;
            _viewModel.ThemeChanged -= OnThemeChanged;
            _viewModel.ThemeChanged += OnThemeChanged;
        }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
        finally { _viewModel.IsBusy = false; }
    }

    // Build a Grid-based step indicator.
    // Layout: [dot(fixed)] [connector(*)] [dot(fixed)] ...
    // Star columns guarantee all steps fit with no overflow.
    private void BuildStepGrid(int activeIndex)
    {
        try
        {
            var pages = SharedServices.PrayerData.SinglePageDataModels;
            int count = pages.Count;
            if (count == 0) return;

            stepProgressGrid.ColumnDefinitions.Clear();
            stepProgressGrid.Children.Clear();

            const double dotSize   = 20;   // inner circle diameter
            const double ringExtra = 4;    // extra diameter for the active-step ring
            const double connH     = 2;

            for (int i = 0; i < count; i++)
            {
                stepProgressGrid.ColumnDefinitions.Add(new ColumnDefinition(dotSize + ringExtra));
                if (i < count - 1)
                    stepProgressGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            }

            for (int i = 0; i < count; i++)
            {
                int col = i * 2;
                var idx = i;

                bool completed  = i < activeIndex;
                bool inProgress = i == activeIndex;

                var overlay = new Grid
                {
                    WidthRequest      = dotSize + ringExtra,
                    HeightRequest     = dotSize + ringExtra,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions   = LayoutOptions.Center,
                };

                // Outer ring — only visible on the active step
                if (inProgress)
                {
                    var ring = new Ellipse
                    {
                        WidthRequest      = dotSize + ringExtra,
                        HeightRequest     = dotSize + ringExtra,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions   = LayoutOptions.Center,
                        Fill              = new SolidColorBrush(Colors.Transparent),
                        Stroke            = new SolidColorBrush(_stepInProgress),
                        StrokeThickness   = 2,
                    };
                    overlay.Add(ring);
                }

                // Inner filled dot
                var dot = new Ellipse
                {
                    WidthRequest      = dotSize,
                    HeightRequest     = dotSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions   = LayoutOptions.Center,
                    Fill = completed  ? new SolidColorBrush(_stepCompleted) :
                           inProgress ? new SolidColorBrush(_stepInProgress) :
                                        new SolidColorBrush(_stepNotStarted),
                };
                overlay.Add(dot);

                // Icon label: check for completed, pray for in-progress / not-started
                var iconLabel = new Label
                {
                    Text              = completed ? GlyphCheck : GlyphPray,
                    FontFamily        = "FontAwesome5Solid",
                    FontSize          = 10,
                    TextColor         = Colors.White,
                    HorizontalOptions       = LayoutOptions.Center,
                    VerticalOptions         = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment   = TextAlignment.Center,
                    InputTransparent        = true,
                };
                overlay.Add(iconLabel);

                var tapOverlay = new TapGestureRecognizer();
                tapOverlay.Tapped += (_, _) =>
                    Task.Run(async () => await _viewModel.OnProgressStepTapped(idx));
                overlay.GestureRecognizers.Add(tapOverlay);

                Grid.SetColumn(overlay, col);
                stepProgressGrid.Add(overlay);

                // Connector line
                if (i < count - 1)
                {
                    var line = new BoxView
                    {
                        Color             = i < activeIndex ? _stepCompleted : _connectorColor,
                        HeightRequest     = connH,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions   = LayoutOptions.Center,
                    };
                    Grid.SetColumn(line, col + 1);
                    stepProgressGrid.Add(line);
                }
            }
        }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
    }

    private void UpdateStepColors(ViewModels.StyleModelDefault? style)
    {
        if (style == null) return;
        static Color Parse(string? hex, Color fallback) =>
            string.IsNullOrEmpty(hex) ? fallback : Color.FromArgb(hex);

        _stepCompleted  = Parse(style.ProgressBarCompletedBgColor,          Color.FromArgb("#0B5345"));
        _stepInProgress = Parse(style.ProgressBarInProgressMarkerFillColor, Color.FromArgb("#1A5276"));
        _stepNotStarted = Parse(style.ProgressBarIconMainBgColor,           Color.FromArgb("#2D3E50"));
        _connectorColor = Parse(style.ProgressBarTrackColor,                Color.FromArgb("#5D6D7E"));
    }

    private void OnFontSizeChanged()
    {
        var settings = ServiceLocator.GetService<SettingsViewModel>();
        if (settings == null) return;
        double fontDelta = settings.CurrentFontSize;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            var pageData = _viewModel.SinglePageDataModel;
            if (pageData == null) return;
            foreach (var para in _paraContentViews)
            {
                var baseSize = pageData.Content
                    .FirstOrDefault(c => c.Content == para.ParaContent)?.FontSize ?? 12;
                para.FontSize = baseSize + fontDelta;
            }
            foreach (var para in _paraBulletViews)
            {
                var baseSize = pageData.Content
                    .FirstOrDefault(c => c.Content == para.ParaContent)?.FontSize ?? 12;
                para.FontSize = baseSize + fontDelta;
            }
        });
    }

    protected override bool OnBackButtonPressed()
    {
        try
        {
            if (_viewModel.CanGoPrevious)
                Task.Run(async () => await _viewModel.GoPrevious());
            else
                Task.Run(async () => await Shell.Current.GoToAsync("///MainView"));
        }
        catch (Exception ex) { ExceptionHandler.Capture(ex); }
        return true;
    }

    private void ApplyPillColors(StyleModelDefault style)
    {
        if (!string.IsNullOrEmpty(style.ButtonBgColor))
            pillBg.Color = Color.FromArgb(style.ButtonBgColor);

        if (!string.IsNullOrEmpty(style.ButtonFgColor))
        {
            var fg = Color.FromArgb(style.ButtonFgColor);
            foreach (var btn in pillGrid.Children.OfType<Syncfusion.Maui.Buttons.SfButton>())
                btn.TextColor = fg;
            pillDivider1.Color = fg;
            pillDivider2.Color = fg;
        }
    }

    private void OnThemeChanged()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var style = _viewModel.DefaultStyle;
            if (style == null) return;

            UpdateStepColors(style);
            BuildStepGrid(_viewModel.PageIndex);
            ApplyPillColors(style);

            var fontColor = style.DefaultFontColor;
            var textColor = string.IsNullOrEmpty(fontColor) ? Colors.Black : Color.FromArgb(fontColor);

            foreach (var para in _paraContentViews)
                para.TextColor = textColor;
            foreach (var para in _paraBulletViews)
                para.TextColor = textColor;

            var headerTextColor = string.IsNullOrEmpty(style.BoxFontColor)
                ? Colors.White : Color.FromArgb(style.BoxFontColor);
            var headerBorderColor = string.IsNullOrEmpty(style.ButtonBgColor)
                ? Colors.Gray : Color.FromArgb(style.ButtonBgColor);
            var isDark = style.AppTheme == AppThemes.Dark;
            var headerBgColor = isDark ? Color.FromArgb("#2C3E50") : Color.FromArgb("#1A5276");

            foreach (var child in contentStack.Children.OfType<ParaHeaderView>())
            {
                child.TextColor = headerTextColor;
                child.BorderColor = headerBorderColor;
                child.BoxBackgroundColor = headerBgColor;
            }
        });
    }

    protected override void OnDisappearing()
    {
        _viewModel.FontSizeChanged -= OnFontSizeChanged;
        _viewModel.ThemeChanged -= OnThemeChanged;
        _viewModel.StopAudio();
        base.OnDisappearing();
    }
}
