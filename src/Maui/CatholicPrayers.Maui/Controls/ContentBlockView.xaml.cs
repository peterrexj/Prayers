using CatholicPrayers.Maui.Models;

namespace CatholicPrayers.Maui.Controls;

public partial class ContentBlockView : ContentView
{
    public static readonly BindableProperty BlockProperty =
        BindableProperty.Create(nameof(Block), typeof(ContentBlock), typeof(ContentBlockView), null,
            propertyChanged: (b, _, _) => ((ContentBlockView)b).ApplyBlock());

    public static readonly BindableProperty FontSizeDeltaProperty =
        BindableProperty.Create(nameof(FontSizeDelta), typeof(double), typeof(ContentBlockView), 0.0,
            propertyChanged: (b, _, _) => ((ContentBlockView)b).ApplyBlock());

    public ContentBlock? Block
    {
        get => (ContentBlock?)GetValue(BlockProperty);
        set => SetValue(BlockProperty, value);
    }

    public double FontSizeDelta
    {
        get => (double)GetValue(FontSizeDeltaProperty);
        set => SetValue(FontSizeDeltaProperty, value);
    }

    private CancellationTokenSource? _animCts;

    public ContentBlockView()
    {
        InitializeComponent();
    }

    private void ApplyBlock()
    {
        var block = Block;
        HideAll();
        if (block == null) return;

        double baseSize = 17 + FontSizeDelta + block.FontSizeOffset;
        var bodyColor = GetThemeColor("PrayAppBodyTextColor", Colors.White);
        var accentColor = GetThemeColor("PrayAppAccentColor", Colors.Gold);
        var responseColor = GetThemeColor("PrayAppResponseTextColor", Colors.Gold);

        switch (block.Type)
        {
            case "H1":
                h1Label.Text = block.Text;
                h1Label.IsVisible = true;
                break;
            case "H2":
                h2Label.Text = block.Text;
                h2Label.IsVisible = true;
                break;
            case "H3":
                h3Label.Text = block.Text;
                h3Label.IsVisible = true;
                break;
            case "Paragraph":
                paraLabel.Text = block.Text;
                paraLabel.FontSize = baseSize;
                paraLabel.FontAttributes = ParseFontAttributes(block.FontStyle);
                paraLabel.HorizontalTextAlignment = ParseAlignment(block.Alignment);
                paraLabel.TextColor = bodyColor;
                paraLabel.IsVisible = true;
                break;
            case "Quote":
                quoteLabel.Text = block.Text;
                quoteLabel.IsVisible = true;
                break;
            case "BulletItem":
            case "NumberedItem":
            {
                var markerColor = block.IsResponse ? responseColor : accentColor;
                var textColor   = block.IsResponse ? responseColor : bodyColor;
                var marker = (block.BulletLabel ?? "•").PadRight(5);
                bulletGrid.FontSize = baseSize;
                bulletGrid.FormattedText = new FormattedString
                {
                    Spans =
                    {
                        new Span
                        {
                            Text = marker,
                            TextColor = markerColor,
                            FontAttributes = block.IsResponse ? FontAttributes.Bold : FontAttributes.None,
                            FontSize = baseSize
                        },
                        new Span
                        {
                            Text = block.Text,
                            TextColor = textColor,
                            FontAttributes = block.IsResponse ? FontAttributes.Italic : FontAttributes.None,
                            FontSize = baseSize
                        }
                    }
                };
                bulletGrid.IsVisible = true;
                break;
            }
            case "Image":
                if (!string.IsNullOrWhiteSpace(block.ImageFile))
                {
                    inlineImage.Source = ImageSource.FromFile(block.ImageFile);
                    if (!string.IsNullOrWhiteSpace(block.ImageCaption))
                    {
                        imageCaptionLabel.Text = block.ImageCaption;
                        imageCaptionLabel.TextColor = bodyColor;
                        imageCaptionLabel.IsVisible = true;
                    }
                    imageContainer.IsVisible = true;
                    StartDivineEffects();
                }
                break;
            case "Divider":
                dividerView.IsVisible = true;
                break;
            case "Spacer":
                spacerView.IsVisible = true;
                break;
        }
    }

    private void StartDivineEffects()
    {
        _animCts?.Cancel();
        _animCts = new CancellationTokenSource();
        var token = _animCts.Token;

        // Run all effects concurrently, ignore individual cancellations
        _ = Task.Run(async () =>
        {
            await Task.Delay(120, token); // let layout settle
            if (token.IsCancellationRequested) return;
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                _ = BreathingScaleAsync(token);
                _ = SpawnParticlesAsync(token);
            });
        }, token);
    }

    // ── Gentle breathing scale on the card ───────────────────────────────────

    private async Task BreathingScaleAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await imageBorder.ScaleTo(1.025, 2200, Easing.SinInOut);
            if (token.IsCancellationRequested) break;
            await imageBorder.ScaleTo(1.000, 2200, Easing.SinInOut);
        }
        imageBorder.Scale = 1.0;
    }

    // ── Floating gold particle sparks ────────────────────────────────────────

    private async Task SpawnParticlesAsync(CancellationToken token)
    {
        var rng = new Random();

        while (!token.IsCancellationRequested)
        {
            // Stagger spawning — one particle every 380–700ms
            await Task.Delay(rng.Next(380, 700), token);
            if (token.IsCancellationRequested) break;

            // Build a tiny gold spark
            var size = rng.Next(4, 9);
            var spark = new BoxView
            {
                WidthRequest  = size,
                HeightRequest = size,
                CornerRadius  = size / 2f,
                Color         = Color.FromArgb(GoldShade(rng)),
                Opacity       = 0,
                InputTransparent = true
            };

            // Random X within the card bounds, start near the bottom third
            double canvasW = particleCanvas.Width > 0 ? particleCanvas.Width : 260;
            double canvasH = particleCanvas.Height > 0 ? particleCanvas.Height : 370;
            double x = rng.NextDouble() * (canvasW - 16) + 8;
            double y = canvasH * (0.55 + rng.NextDouble() * 0.35);

            AbsoluteLayout.SetLayoutBounds(spark, new Rect(x, y, size, size));
            particleCanvas.Children.Add(spark);

            // Animate: fade in, float up, fade out — then remove
            _ = AnimateParticleAsync(spark, rng, canvasH, token);
        }
    }

    private static async Task AnimateParticleAsync(BoxView spark, Random rng,
        double canvasH, CancellationToken token)
    {
        double rise  = canvasH * (0.25 + rng.NextDouble() * 0.25);
        double drift = (rng.NextDouble() - 0.5) * 30;
        uint   dur   = (uint)rng.Next(1400, 2600);

        try
        {
            await spark.FadeTo(0.85, dur / 4);
            var move = spark.TranslateTo(drift, -rise, dur, Easing.CubicOut);
            var fade = spark.FadeTo(0, dur, Easing.CubicIn);
            await Task.WhenAll(move, fade);
        }
        catch { /* view may have been removed */ }
        finally
        {
            if (!token.IsCancellationRequested)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    try { ((AbsoluteLayout)spark.Parent)?.Children.Remove(spark); } catch { }
                });
        }
    }

    private static string GoldShade(Random rng) => rng.Next(3) switch
    {
        0 => "#FFD700",  // gold
        1 => "#FFF0A0",  // pale gold
        _ => "#FFA500"   // amber
    };

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void HideAll()
    {
        _animCts?.Cancel();
        _animCts = null;

        h1Label.IsVisible        = false;
        h2Label.IsVisible        = false;
        h3Label.IsVisible        = false;
        paraLabel.IsVisible      = false;
        quoteLabel.IsVisible     = false;
        bulletGrid.IsVisible     = false;
        imageContainer.IsVisible = false;
        imageCaptionLabel.IsVisible = false;
        dividerView.IsVisible    = false;
        spacerView.IsVisible     = false;

        imageBorder.Scale = 1.0;
        particleCanvas.Children.Clear();
    }

    private static Color GetThemeColor(string key, Color fallback)
    {
        if (Application.Current?.Resources?.TryGetValue(key, out var val) == true && val is Color c)
            return c;
        return fallback;
    }

    private static FontAttributes ParseFontAttributes(string? style) => style switch
    {
        "Bold" => FontAttributes.Bold,
        "Italic" => FontAttributes.Italic,
        "BoldItalic" => FontAttributes.Bold | FontAttributes.Italic,
        _ => FontAttributes.None
    };

    private static TextAlignment ParseAlignment(string? alignment) => alignment switch
    {
        "Center" => TextAlignment.Center,
        "Right" => TextAlignment.End,
        _ => TextAlignment.Start
    };
}
