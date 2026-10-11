using CatholicPrayers.Maui.Models;

namespace CatholicPrayers.Maui.Services;

public interface IThemeHandler
{
    AppThemes CurrentTheme { get; }
    void LoadDefaultStyle();
    void LoadDefaultStyle(AppThemes appTheme);
    Task LoadDefaultStyleAsync(AppThemes? theme = null);
    Task SaveThemeAsync(AppThemes theme);
    Task<AppThemes> LoadSavedThemeAsync();
}
