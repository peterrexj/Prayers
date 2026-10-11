using Prayers.StJoseph.Maui.Models;

namespace Prayers.StJoseph.Maui.ViewModels;

public class StyleModelDefault : BasePropertyChangeModel
{
    public string? DefaultPageBgGradient { get; set; }
    public string? BoxBgGradient { get; set; }

    public string? PageBgColor { get; set; }
    public string? BoxFontColor { get; set; }

    public string? ButtonFgColor { get; set; }
    public string? ButtonBgColor { get; set; }
    public string? ThemeButtonBgColor { get; set; }

    public string? ProgressBarTrackColor { get; set; }
    public string? ProgressBarInProgressMarkerFillColor { get; set; }
    public string? ProgressBarInProgressMarkerStrokeColor { get; set; }
    public string? ProgressBarCompletedBgColor { get; set; }
    public string? ProgressBarIconMainBgColor { get; set; }

    public string? DefaultFontColor { get; set; }

    public AppThemes AppTheme { get; set; }
    public string? AppShellBgColor { get; set; }
    public string? AppShellFgColor { get; set; }
    public string? AppShellTitleColor { get; set; }
    public string? AppShellDisabledColor { get; set; }
    public string? AppShellUnselectedColor { get; set; }
    public string? AppShellTabBarBackgroundColor { get; set; }
    public string? AppShellTabBarForegroundColor { get; set; }
    public string? AppShellTabBarUnselectedColor { get; set; }
    public string? AppShellTabBarDisabledColor { get; set; }
    public string? AppShellTabBarTitleColor { get; set; }
    public string? AppShellBackgroundColor { get; set; }
}
