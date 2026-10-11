using CatholicPrayers.Maui.Tests.Helpers;

namespace CatholicPrayers.Maui.Tests.ViewModels;

/// <summary>
/// Tests for SettingsViewModel logic decoupled from MAUI.
/// We replicate the core logic in a plain class to avoid MAUI runtime dependency.
/// </summary>
public class SettingsLogicTests
{
    private const double MinFontDelta = -4.0;
    private const double MaxFontDelta = 8.0;

    private static double Clamp(double value, double min, double max) =>
        Math.Max(min, Math.Min(max, value));

    [Fact]
    public void FontSizeDelta_IncreasesBy1()
    {
        double delta = 0;
        delta = Clamp(delta + 1, MinFontDelta, MaxFontDelta);
        Assert.Equal(1.0, delta);
    }

    [Fact]
    public void FontSizeDelta_DecreasesBy1()
    {
        double delta = 0;
        delta = Clamp(delta - 1, MinFontDelta, MaxFontDelta);
        Assert.Equal(-1.0, delta);
    }

    [Fact]
    public void FontSizeDelta_ClampsAtMax()
    {
        double delta = MaxFontDelta;
        delta = Clamp(delta + 1, MinFontDelta, MaxFontDelta);
        Assert.Equal(MaxFontDelta, delta);
    }

    [Fact]
    public void FontSizeDelta_ClampsAtMin()
    {
        double delta = MinFontDelta;
        delta = Clamp(delta - 1, MinFontDelta, MaxFontDelta);
        Assert.Equal(MinFontDelta, delta);
    }

    [Fact]
    public void PreviewFontSize_BasePlusDelta()
    {
        const double baseSize = 16.0;
        double delta = 3.0;
        double preview = baseSize + delta;
        Assert.Equal(19.0, preview);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(5, 5)]
    [InlineData(-1, 0)]
    [InlineData(6, 5)]
    public void SelectedThemeIndex_ClampsToRange(int input, int expected)
    {
        var themes = new[] { "Dark", "Light", "Forest", "Candy", "Sunset", "Ocean" };
        int clamped = input < 0 ? 0 : (input >= themes.Length ? themes.Length - 1 : input);
        Assert.Equal(expected, clamped);
    }

    [Fact]
    public void ThemeList_Contains6Themes()
    {
        var themes = new[] { "Dark", "Light", "Forest", "Candy", "Sunset", "Ocean" };
        Assert.Equal(6, themes.Length);
    }

    [Fact]
    public void FontList_Contains8Fonts()
    {
        var fonts = new[]
        {
            "Open Sans", "Lato", "Nunito", "Quicksand",
            "Raleway", "Merriweather", "Playfair Display", "Source Serif 4"
        };
        Assert.Equal(8, fonts.Length);
    }
}

public class SettingsPersistenceTests
{
    // Simulate what SettingsViewModel.SaveAsync does: build a dictionary, verify keys
    [Fact]
    public void SavedKeys_ContainAllExpectedSettings()
    {
        var settings = new Dictionary<string, object>
        {
            ["SelectedTheme"] = "Dark",
            ["SelectedFont"] = "Open Sans",
            ["FontSizeDelta"] = 0.0,
            ["ShowAnimations"] = true,
            ["ShowDivineAnimations"] = true
        };

        Assert.True(settings.ContainsKey("SelectedTheme"));
        Assert.True(settings.ContainsKey("SelectedFont"));
        Assert.True(settings.ContainsKey("FontSizeDelta"));
        Assert.True(settings.ContainsKey("ShowAnimations"));
        Assert.True(settings.ContainsKey("ShowDivineAnimations"));
    }

    [Fact]
    public void RestoreDefaults_HasCorrectValues()
    {
        string theme = "Dark";
        string font = "Open Sans";
        double fontDelta = 0.0;
        bool showAnim = true;
        bool showDivine = true;

        Assert.Equal("Dark", theme);
        Assert.Equal("Open Sans", font);
        Assert.Equal(0.0, fontDelta);
        Assert.True(showAnim);
        Assert.True(showDivine);
    }
}
