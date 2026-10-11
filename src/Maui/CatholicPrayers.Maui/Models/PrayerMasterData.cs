namespace CatholicPrayers.Maui.Models;

/// <summary>
/// Root of content_index.json (previously prayers_master.json).
/// Sections = top-level dashboard cards; Prayers = flat lookup of all entries.
/// </summary>
public class PrayerMasterData
{
    public string AppVersion { get; set; } = "1.0";

    /// <summary>Top-level section cards (Prayers, Novenas, Stations, Saints, Bible).</summary>
    public PrayerMasterEntry[] Sections { get; set; } = [];

    /// <summary>Flat lookup — every prayer, novena, saint, and link entry.</summary>
    public PrayerMasterEntry[] Prayers { get; set; } = [];
}
