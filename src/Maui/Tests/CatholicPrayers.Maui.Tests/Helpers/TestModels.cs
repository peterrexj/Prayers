// Lightweight copies of the main project models for test isolation
// (the main project targets MAUI TFs; tests target net9.0)

using System.Text.Json.Serialization;

namespace CatholicPrayers.Maui.Tests.Helpers;

public record ContentBlockModel
{
    public string Type { get; set; } = "Paragraph";
    public string? Text { get; set; }
    public string? BulletLabel { get; set; }
    public bool IsResponse { get; set; }
    public string? FontStyle { get; set; }
    public string? Alignment { get; set; }
    public double FontSizeOffset { get; set; }
    public int Order { get; set; }
}

public record PrayerSectionModel
{
    public string Title { get; set; } = string.Empty;
    public ContentBlockModel[] Blocks { get; set; } = [];
}

public record PrayerDataModel
{
    public string Format { get; set; } = "SinglePage";
    public PrayerSectionModel[] Sections { get; set; } = [];
    public string[] RelatedPrayerIds { get; set; } = [];
}

public record PrayerMasterEntryModel
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string Icon { get; set; } = "✝";
    public string DataFile { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsFeatured { get; set; }
    public string AccentColor { get; set; } = "#9D7DE8";
    public string[] Tags { get; set; } = [];
    public string[] RelatedPrayerIds { get; set; } = [];
}

public class PrayerMasterDataModel
{
    [JsonPropertyName("Prayers")]
    public PrayerMasterEntryModel[] Prayers { get; set; } = [];
}
