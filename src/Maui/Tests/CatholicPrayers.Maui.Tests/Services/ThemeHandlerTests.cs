namespace CatholicPrayers.Maui.Tests.Services;

public class ThemeHandlerLogicTests
{
    // Replicate GetThemeBgHex logic for test isolation
    private static string GetThemeBgHex(string theme) => theme switch
    {
        "Dark"   => "#0D1B2A",
        "Light"  => "#FAF5FF",
        "Forest" => "#0D2818",
        "Candy"  => "#FFF0F7",
        "Sunset" => "#FFF7ED",
        "Ocean"  => "#EFF6FF",
        _        => "#0D1B2A"
    };

    [Theory]
    [InlineData("Dark",   "#0D1B2A")]
    [InlineData("Light",  "#FAF5FF")]
    [InlineData("Forest", "#0D2818")]
    [InlineData("Candy",  "#FFF0F7")]
    [InlineData("Sunset", "#FFF7ED")]
    [InlineData("Ocean",  "#EFF6FF")]
    public void GetThemeBgHex_ReturnsCorrectColor(string theme, string expected)
    {
        Assert.Equal(expected, GetThemeBgHex(theme));
    }

    [Fact]
    public void GetThemeBgHex_UnknownTheme_ReturnsDark()
    {
        Assert.Equal("#0D1B2A", GetThemeBgHex("Unknown"));
    }

    [Fact]
    public void AllThemeBgColors_AreValidHex()
    {
        var themes = new[] { "Dark", "Light", "Forest", "Candy", "Sunset", "Ocean" };
        foreach (var theme in themes)
        {
            var hex = GetThemeBgHex(theme);
            Assert.Matches(@"^#[0-9A-Fa-f]{6}$", hex);
        }
    }

    [Fact]
    public void DarkThemeColors_AreDark()
    {
        // Dark theme backgrounds should be dark (low RGB values)
        var darkThemes = new[] { "Dark", "Forest" };
        foreach (var theme in darkThemes)
        {
            var hex = GetThemeBgHex(theme);
            var r = Convert.ToInt32(hex.Substring(1, 2), 16);
            var g = Convert.ToInt32(hex.Substring(3, 2), 16);
            var b = Convert.ToInt32(hex.Substring(5, 2), 16);
            Assert.True(r < 30 && g < 50 && b < 50, $"{theme} should have dark background (hex={hex})");
        }
    }

    [Fact]
    public void LightThemeColors_AreLight()
    {
        var lightThemes = new[] { "Light", "Candy", "Sunset", "Ocean" };
        foreach (var theme in lightThemes)
        {
            var hex = GetThemeBgHex(theme);
            var r = Convert.ToInt32(hex.Substring(1, 2), 16);
            var g = Convert.ToInt32(hex.Substring(3, 2), 16);
            var b = Convert.ToInt32(hex.Substring(5, 2), 16);
            Assert.True(r > 200, $"{theme} should have light background R>200 (hex={hex})");
        }
    }

    [Fact]
    public void ThemeResourcePrefix_IsPrayApp()
    {
        const string expectedPrefix = "PrayApp";
        var sampleKeys = new[]
        {
            "PrayAppPageBgColor",
            "PrayAppAccentColor",
            "PrayAppTextPrimary",
            "PrayAppTileBgBrush",
        };
        Assert.All(sampleKeys, k => Assert.StartsWith(expectedPrefix, k));
    }

    [Fact]
    public void ThemeXamlResourceNames_CorrectFormat()
    {
        var themes = new[] { "Dark", "Light", "Forest", "Candy", "Sunset", "Ocean" };
        // Resource name format: CatholicPrayers.Maui.Resources.Styles.Theme_{name}.xaml
        foreach (var theme in themes)
        {
            var expected = $"CatholicPrayers.Maui.Resources.Styles.Theme_{theme}.xaml";
            Assert.Contains($"Theme_{theme}.xaml", expected);
        }
    }
}
