using NUnit.Framework;
using Prayers.StJoseph.Maui.Models;
using Prayers.StJoseph.Maui.Services;
using Prayers.StJoseph.Maui.ViewModels;

namespace Prayers.StJoseph.Maui.Tests;

[TestFixture]
public class ThemeServiceTests
{
    private ThemeService CreateService(AppThemes theme = AppThemes.Dark)
    {
        var settings = new SettingsViewModel { SelectedTheme = theme.ToString() };
        return new ThemeService(settings);
    }

    [Test]
    public void GetDefaultStyleTheme_Dark_ReturnsCorrectColors()
    {
        var svc = CreateService(AppThemes.Dark);
        var style = svc.GetDefaultStyleTheme(AppThemes.Dark);
        Assert.That(style.AppTheme, Is.EqualTo(AppThemes.Dark));
        Assert.That(style.ButtonBgColor, Is.EqualTo("#0B5345"));
        Assert.That(style.DefaultFontColor, Is.EqualTo("#BFC9CA"));
    }

    [Test]
    public void GetDefaultStyleTheme_Light_ReturnsCorrectColors()
    {
        var svc = CreateService(AppThemes.Light);
        var style = svc.GetDefaultStyleTheme(AppThemes.Light);
        Assert.That(style.AppTheme, Is.EqualTo(AppThemes.Light));
        Assert.That(style.ButtonBgColor, Is.EqualTo("#1A5276"));
        Assert.That(style.DefaultFontColor, Is.EqualTo("#17202A"));
    }

    [Test]
    public void GetDefaultStyleTheme_FireBreather_FallsBackToDark()
    {
        var svc = CreateService(AppThemes.FireBreather);
        var style = svc.GetDefaultStyleTheme(AppThemes.FireBreather);
        Assert.That(style.AppTheme, Is.EqualTo(AppThemes.Dark));
    }

    [TestCase(AppThemes.Dark, "#1B2631")]
    [TestCase(AppThemes.Light, "#B3B6B7")]
    public void GetDefaultStyleTheme_ShellBgColor_IsCorrect(AppThemes theme, string expected)
    {
        var svc = CreateService(theme);
        var style = svc.GetDefaultStyleTheme(theme);
        Assert.That(style.AppShellBgColor, Is.EqualTo(expected));
    }
}
