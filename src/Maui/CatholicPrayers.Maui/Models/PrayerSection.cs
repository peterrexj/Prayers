namespace CatholicPrayers.Maui.Models;

/// <summary>
/// A logical section (e.g. "Day 1", "Opening Prayer") within a prayer.
/// A prayer can be a single section or multiple pages.
/// </summary>
public class PrayerSection
{
    public int SectionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public ContentBlock[] Blocks { get; set; } = [];
}
