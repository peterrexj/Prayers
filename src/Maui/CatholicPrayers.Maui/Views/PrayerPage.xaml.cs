using Microsoft.Maui.Controls.Shapes;
using CatholicPrayers.Maui.Controls;
using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.Services;
using CatholicPrayers.Maui.ViewModels;

namespace CatholicPrayers.Maui.Views;

[QueryProperty(nameof(PrayerId), nameof(PrayerId))]
[QueryProperty(nameof(FromPrayerId), nameof(FromPrayerId))]
public partial class PrayerPage : ContentPage
{
    private readonly PrayerViewModel _vm;
    private string? _prayerId;
    private string? _fromPrayerId;
    private readonly List<ContentBlockView> _blockViews = [];
    private System.Timers.Timer? _divineTimer;

    // Step progress colors — updated on section/theme change
    private Color _stepCompleted  = Color.FromArgb("#9D7DE8");
    private Color _stepActive     = Color.FromArgb("#FFD580");
    private Color _stepInactive   = Color.FromArgb("#2E3F6A");
    private Color _stepConnector  = Color.FromArgb("#2E3F6A");

    public string? PrayerId
    {
        get => _prayerId;
        set { _prayerId = value; }
    }

    public string? FromPrayerId
    {
        get => _fromPrayerId;
        set { _fromPrayerId = value; }
    }

    public PrayerPage(PrayerViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
        vm.SectionChanged += OnSectionChanged;
        vm.FontSizeChanged += OnFontSizeChanged;
        vm.AmenRequested += OnAmenRequested;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (string.IsNullOrEmpty(_prayerId)) return;
            RefreshStepColors();
            await _vm.LoadAsync(_prayerId, _fromPrayerId);
            RebuildContent();
            BuildProgressDots();
            StartDivineTimer();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[PrayerPage.OnAppearing] {ex}");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        StopDivineTimer();
        _vm.SectionChanged -= OnSectionChanged;
        _vm.FontSizeChanged -= OnFontSizeChanged;
        _vm.AmenRequested -= OnAmenRequested;
    }

    // ── Content builder ──────────────────────────────────────────────────────

    private void RebuildContent()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            contentStack.Children.Clear();
            _blockViews.Clear();

            var section = _vm.CurrentSection;
            if (section == null) return;

            bool isFirstSection = _vm.CurrentSectionIndex == 0;
            bool isLastSection  = _vm.CurrentSectionIndex == _vm.SectionCount - 1;
            var heroFile = isFirstSection ? _vm.HeroImageFile : null;
            bool heroAtTop = string.Equals(_vm.HeroImagePosition, "Top", StringComparison.OrdinalIgnoreCase);

            // Hero at top
            if (heroFile != null && heroAtTop)
                contentStack.Children.Add(BuildHeroImage(heroFile));

            var blocks = section.Blocks.OrderBy(b => b.Order);
            foreach (var block in blocks)
            {
                var view = new ContentBlockView
                {
                    Block = block,
                    FontSizeDelta = _vm.FontSizeDelta
                };
                _blockViews.Add(view);
                contentStack.Children.Add(view);
            }

            // Hero at bottom
            if (heroFile != null && !heroAtTop)
                contentStack.Children.Add(BuildHeroImage(heroFile));

            // Related prayers at the end of last section
            if (isLastSection && _vm.RelatedPrayers.Count > 0)
                AppendRelatedPrayers();
        });
    }

    private View BuildHeroImage(string imageFile)
    {
        Color accent;
        try { accent = Color.FromArgb(_vm.AccentColor); }
        catch { accent = GetColor("PrayAppAccentColor"); }

        var card = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = 20 },
            Stroke = new SolidColorBrush(accent.WithAlpha(0.60f)),
            StrokeThickness = 2,
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Fill,
            Margin = new Thickness(0, 4, 0, 20),
            Padding = 0,
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(accent.WithAlpha(0.45f)),
                Offset = new Point(0, 6),
                Radius = 24,
                Opacity = 0.8f
            }
        };

        card.Content = new Image
        {
            Source = ImageSource.FromFile(imageFile),
            Aspect = Aspect.AspectFill,
            HeightRequest = 220,
            HorizontalOptions = LayoutOptions.Fill
        };

        return card;
    }

    private void AppendRelatedPrayers()
    {
        bool hasForward = _vm.RelatedPrayers.Count > 0;
        bool hasSource  = _vm.SourcePrayer != null;

        if (!hasForward && !hasSource) return;

        contentStack.Children.Add(new BoxView
        {
            HeightRequest = 1,
            BackgroundColor = GetColor("PrayAppDividerColor"),
            Margin = new Thickness(0, 24, 0, 16)
        });

        if (hasForward)
        {
            contentStack.Children.Add(new Label
            {
                Text = "Continue With",
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
                TextColor = GetColor("PrayAppH2TextColor"),
                Margin = new Thickness(0, 0, 0, 8)
            });

            foreach (var entry in _vm.RelatedPrayers)
                contentStack.Children.Add(BuildRelatedCard(entry, isSource: false));
        }

        if (hasSource)
        {
            contentStack.Children.Add(new Label
            {
                Text = "Came From",
                FontSize = 13,
                TextColor = GetColor("PrayAppSubtitleTextColor"),
                Margin = new Thickness(0, hasForward ? 20 : 0, 0, 8)
            });

            contentStack.Children.Add(BuildRelatedCard(_vm.SourcePrayer!, isSource: true));
        }
    }

    private Border BuildRelatedCard(PrayerMasterEntry entry, bool isSource)
    {
        var border = new Border
        {
            BackgroundColor = GetColor("PrayAppCardBgColor"),
            Stroke = new SolidColorBrush(isSource
                ? GetColor("PrayAppDividerColor")
                : GetColor("PrayAppCardBorderColor")),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Margin = new Thickness(0, 4),
            Padding = new Thickness(14, 12),
            Opacity = isSource ? 0.7 : 1.0
        };

        var grid = new Grid { ColumnDefinitions = { new ColumnDefinition(36), new ColumnDefinition(GridLength.Star) } };

        grid.Add(new Label
        {
            Text = entry.Icon,
            FontFamily = "FontAwesome5Solid",
            FontSize = 20,
            TextColor = GetColor("PrayAppAccentColor"),
            VerticalOptions = LayoutOptions.Center
        }, 0, 0);

        var textStack = new StackLayout { Spacing = 2, Margin = new Thickness(8, 0, 0, 0) };
        textStack.Add(new Label { Text = entry.Title, FontSize = 15, FontAttributes = FontAttributes.Bold, TextColor = GetColor("PrayAppH2TextColor") });
        textStack.Add(new Label { Text = entry.Category, FontSize = 12, TextColor = GetColor("PrayAppSubtitleTextColor") });
        grid.Add(textStack, 1, 0);

        border.Content = grid;
        border.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = _vm.NavigateToRelatedCommand,
            CommandParameter = entry
        });

        return border;
    }

    // ── Section progress dots ─────────────────────────────────────────────────

    private void BuildProgressDots()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            progressGrid.Children.Clear();
            progressGrid.ColumnDefinitions.Clear();

            int count = _vm.SectionCount;
            if (count <= 1) return;

            int active = _vm.CurrentSectionIndex;
            const double dotSize = 18;
            const double ringExtra = 6;

            for (int i = 0; i < count; i++)
            {
                progressGrid.ColumnDefinitions.Add(new ColumnDefinition(dotSize + ringExtra));
                if (i < count - 1)
                    progressGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            }

            for (int i = 0; i < count; i++)
            {
                int col = i * 2;
                bool completed = i < active;
                bool inProgress = i == active;

                var overlay = new Grid
                {
                    WidthRequest = dotSize + ringExtra,
                    HeightRequest = dotSize + ringExtra,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                if (inProgress)
                {
                    overlay.Children.Add(new Ellipse
                    {
                        WidthRequest = dotSize + ringExtra,
                        HeightRequest = dotSize + ringExtra,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Fill = Brush.Transparent,
                        Stroke = new SolidColorBrush(_stepActive),
                        StrokeThickness = 2
                    });
                }

                overlay.Children.Add(new Ellipse
                {
                    WidthRequest = dotSize,
                    HeightRequest = dotSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Fill = new SolidColorBrush(completed ? _stepCompleted : inProgress ? _stepActive : _stepInactive)
                });

                int idx = i;
                overlay.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => _vm.NavigateToSection(idx))
                });

                Grid.SetColumn(overlay, col);
                progressGrid.Children.Add(overlay);

                if (i < count - 1)
                {
                    var connector = new BoxView
                    {
                        Color = i < active ? _stepCompleted : _stepConnector,
                        HeightRequest = 2,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Center
                    };
                    Grid.SetColumn(connector, col + 1);
                    progressGrid.Children.Add(connector);
                }
            }
        });
    }

    private void RefreshStepColors()
    {
        _stepCompleted = GetColor("PrayAppStepCompletedColor");
        _stepActive    = GetColor("PrayAppStepActiveColor");
        _stepInactive  = GetColor("PrayAppStepInactiveColor");
        _stepConnector = GetColor("PrayAppStepConnectorColor");
    }

    // ── Event handlers ────────────────────────────────────────────────────────

    private void OnSectionChanged()
    {
        RebuildContent();
        BuildProgressDots();
        MainThread.BeginInvokeOnMainThread(async () =>
            await contentScroll.ScrollToAsync(0, 0, false));
    }

    private void OnFontSizeChanged()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            foreach (var view in _blockViews)
                view.FontSizeDelta = _vm.FontSizeDelta;
        });
    }

    protected override bool OnBackButtonPressed()
    {
        if (_vm.CanGoPrevious)
            _vm.GoPreviousCommand.Execute(null);
        else
            _ = Shell.Current.GoToAsync("//DashboardPage");
        return true;
    }

    // ── Amen completion overlay ───────────────────────────────────────────────

    private void OnAmenRequested()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try { await ShowAmenOverlayAsync(); }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[Amen] {ex.Message}"); }
        });
    }

    private async Task ShowAmenOverlayAsync()
    {
        StopDivineTimer();

        // Pick a contextual blessing based on the prayer title
        amenBlessingLabel.Text = GetAmenBlessing(_vm.PageTitle);

        amenOverlay.Opacity = 0;
        amenOverlay.IsVisible = true;

        // Scale-in the card
        var card = amenOverlay.Children[0] as VisualElement;
        if (card != null)
        {
            card.Scale = 0.8;
            await Task.WhenAll(
                amenOverlay.FadeTo(1, 280, Easing.SinOut),
                card.ScaleTo(1.0, 280, Easing.SpringOut));
        }
        else
        {
            await amenOverlay.FadeTo(1, 280, Easing.SinOut);
        }
    }

    private async void OnAmenGoHomeClicked(object sender, EventArgs e)
    {
        await amenOverlay.FadeTo(0, 200, Easing.SinIn);
        amenOverlay.IsVisible = false;
        await Shell.Current.GoToAsync("//DashboardPage");
    }

    private static string GetAmenBlessing(string prayerTitle) => prayerTitle switch
    {
        var t when t.Contains("Rosary",       StringComparison.OrdinalIgnoreCase) => "May Our Lady intercede for you and your loved ones.",
        var t when t.Contains("Mercy",        StringComparison.OrdinalIgnoreCase) => "Trust in Divine Mercy — Jesus, I trust in You.",
        var t when t.Contains("Hail Mary",    StringComparison.OrdinalIgnoreCase) => "May Mary, our Mother, keep you in her care.",
        var t when t.Contains("Morning",      StringComparison.OrdinalIgnoreCase) => "May this day be blessed in all you do.",
        var t when t.Contains("Contrition",   StringComparison.OrdinalIgnoreCase) => "Go in peace — your sins are forgiven.",
        var t when t.Contains("Creed",        StringComparison.OrdinalIgnoreCase) => "Blessed are those who believe without seeing.",
        var t when t.Contains("Michael",      StringComparison.OrdinalIgnoreCase) => "May St Michael guard you against all evil.",
        var t when t.Contains("Guardian",     StringComparison.OrdinalIgnoreCase) => "Your guardian angel walks with you always.",
        var t when t.Contains("Holy Spirit",  StringComparison.OrdinalIgnoreCase) => "Come, Holy Spirit, fill the hearts of the faithful.",
        var t when t.Contains("Meal",         StringComparison.OrdinalIgnoreCase) => "Bless this food, and all who share it.",
        _ => "May God bless you and keep you in His peace."
    };

    // ── Divine animation timer ────────────────────────────────────────────────

    private void StartDivineTimer()
    {
        if (!SharedServices.AppSettings.ShowDivineAnimations) return;
        _divineTimer = new System.Timers.Timer(25000); // every 25s
        _divineTimer.Elapsed += (_, _) =>
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try { await divineFlyView.TriggerAsync(); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[DivineFly] {ex.Message}"); }
            });
        _divineTimer.AutoReset = true;
        _divineTimer.Start();
    }

    private void StopDivineTimer()
    {
        _divineTimer?.Stop();
        _divineTimer?.Dispose();
        _divineTimer = null;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static Color GetColor(string key)
    {
        if (Application.Current?.Resources?.TryGetValue(key, out var val) == true && val is Color c)
            return c;
        return Colors.Gray;
    }
}
