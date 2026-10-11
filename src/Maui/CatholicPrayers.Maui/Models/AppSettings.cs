namespace CatholicPrayers.Maui.Models;

public class AppSettings
{
    public AppThemes Theme { get; set; } = AppThemes.Dark;
    public string FontFamily { get; set; } = "Lato";
    public double FontSizeDelta { get; set; } = 0;
    public bool ShowDivineAnimations { get; set; } = true;

    /// <summary>Content language: "en" (default), "la" (Latin), "es" (Spanish)</summary>
    public string ContentLanguage { get; set; } = "en";
}
