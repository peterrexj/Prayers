using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.Controls;

// Non-interactive overlay that floats FA glyphs across the prayer page.
// Call TriggerAsync() to spawn one animation; IsVisible=true activates the layer.
public partial class DivineFlyView : ContentView
{
    private static readonly string[] Symbols = FA.FlySymbols;
    private readonly Random _rng = new();

    public DivineFlyView()
    {
        InitializeComponent();
    }

    public async Task TriggerAsync()
    {
        if (!Services.SharedServices.AppSettings.ShowDivineAnimations) return;

        IsVisible = true;
        flyLabel.Text = Symbols[_rng.Next(Symbols.Length)];
        flyLabel.Opacity = 0;

        // Random horizontal start
        double startX = _rng.NextDouble();
        flyLabel.HorizontalOptions = LayoutOptions.Start;
        flyLabel.Margin = new Thickness(Width * startX, Height * 0.7, 0, 0);

        // Float upward while fading in then out
        var fadeIn  = flyLabel.FadeTo(0.65, 500, Easing.SinIn);
        var moveUp  = flyLabel.TranslateTo(0, -Height * 0.5, 2500, Easing.SinOut);
        await Task.WhenAll(fadeIn, moveUp);
        await flyLabel.FadeTo(0, 600, Easing.SinOut);

        flyLabel.TranslationY = 0;
        IsVisible = false;
    }
}
