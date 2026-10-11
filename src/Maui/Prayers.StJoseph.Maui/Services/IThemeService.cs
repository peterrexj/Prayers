using Prayers.StJoseph.Maui.Models;
using Prayers.StJoseph.Maui.ViewModels;

namespace Prayers.StJoseph.Maui.Services;

public interface IThemeService
{
    StyleModelDefault GetDefaultStyleTheme();
    StyleModelDefault GetDefaultStyleTheme(AppThemes appTheme);
    void UpdateAppThemes(StyleModelDefault? styleModel);
}
