namespace CatholicPrayers.Maui.Models;

/// <summary>
/// Root of a prayer's individual JSON file (e.g. our_father.json).
/// </summary>
public class PrayerData
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>
    /// SinglePage | MultiPage | Novena | Rosary | Litany
    /// Drives overall layout (progress steps for MultiPage/Novena; no steps for SinglePage).
    /// </summary>
    public string Format { get; set; } = "SinglePage";

    public string? HeaderImageFile { get; set; }

    /// <summary>Top | Bottom — where the hero image card appears in the content. Defaults to Top.</summary>
    public string HeaderImagePosition { get; set; } = "Top";
    public string? IntroText { get; set; }
    public bool HasAudio { get; set; }
    public bool HasExternalLinks { get; set; }
    public string AccentColor { get; set; } = "#4A90D9";

    public PrayerSection[] Sections { get; set; } = [];
    public string[] RelatedPrayerIds { get; set; } = [];
}
