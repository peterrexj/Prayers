namespace CatholicPrayers.Maui.Models;

/// <summary>
/// Root of content_index.json — top-level sections + flat prayer lookup table.
/// Replaces the flat PrayerMasterData structure.
/// </summary>
public class ContentIndex
{
    public string AppVersion { get; set; } = "1.0";

    /// <summary>Ordered top-level sections shown as cards on the dashboard.</summary>
    public PrayerMasterEntry[] Sections { get; set; } = [];

    /// <summary>Flat lookup table of every prayer / novena / saint / link entry.</summary>
    public PrayerMasterEntry[] Prayers { get; set; } = [];
}
