namespace CatholicPrayers.Maui.Models;

/// <summary>
/// One entry in content_index.json — prayers, collections, novenas, external links, bible.
/// </summary>
public class PrayerMasterEntry
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string ImageFile { get; set; } = string.Empty;
    public string DataFile { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsFeatured { get; set; }
    public string AccentColor { get; set; } = "#4A90D9";
    public string[] Tags { get; set; } = [];
    public string[] RelatedPrayerIds { get; set; } = [];

    // ── Content node type ────────────────────────────────────────────────────
    /// <summary>Prayer | Collection | Novena | ExternalLink | Bible</summary>
    public string NodeType { get; set; } = "Prayer";

    /// <summary>Child entry IDs for Collection / Novena nodes.</summary>
    public string[] ChildIds { get; set; } = [];

    /// <summary>ID of the section this entry lives under (for breadcrumb).</summary>
    public string? ParentId { get; set; }

    // ── Novena date metadata ─────────────────────────────────────────────────
    /// <summary>e.g. "May 13" — the feast day this novena leads to.</summary>
    public string? FeastDate { get; set; }

    /// <summary>e.g. "May 4" — day 1 of the 9-day novena.</summary>
    public string? StartDate { get; set; }

    // ── External link metadata ───────────────────────────────────────────────
    public string? ExternalAppUrl { get; set; }
    public string? ExternalAppDescription { get; set; }

    // ── Partner app metadata (NodeType = "PartnerApp") ───────────────────────
    public string? PartnerAppUrlAndroid { get; set; }
    public string? PartnerAppUrlIos { get; set; }
    public string? PartnerAppIconGlyph { get; set; }
    public string? PartnerAppIconImage { get; set; }
    public string? PartnerAppImage { get; set; }
    public string? PartnerAppShortDescription { get; set; }
    public string? PartnerAppFullDescription { get; set; }

    // ── Runtime-only ─────────────────────────────────────────────────────────
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsFavourite { get; set; }
}
