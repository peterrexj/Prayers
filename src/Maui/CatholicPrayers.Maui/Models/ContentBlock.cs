namespace CatholicPrayers.Maui.Models;

/// <summary>
/// A single renderable unit within a prayer page or section.
/// The "Type" field drives which control is used.
/// </summary>
public class ContentBlock
{
    /// <summary>
    /// H1 | H2 | H3 | Paragraph | BulletItem | NumberedItem | Quote | Image | Divider | Spacer | ExternalLink | Audio | Rubric
    /// </summary>
    public string Type { get; set; } = "Paragraph";
    public string? Text { get; set; }
    public string? ImageFile { get; set; }
    public string? ImageCaption { get; set; }
    public string? Url { get; set; }
    public string? AudioFile { get; set; }
    public string? BulletLabel { get; set; }    // e.g. "V." / "R." for versicle/response
    public bool IsResponse { get; set; }        // true → render in accent colour (R.)
    public string? FontStyle { get; set; }      // Bold | Italic | BoldItalic
    public string? Alignment { get; set; }      // Left | Center | Right
    public double FontSizeOffset { get; set; }  // +/- applied on top of theme base
    public int Order { get; set; }
}
