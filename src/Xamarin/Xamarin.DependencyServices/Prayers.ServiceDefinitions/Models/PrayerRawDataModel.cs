namespace Prayers.Models;

public class PrayerRawDataModel
{
    public int SequenceId { get; set; }
    public int? HeaderSequenceId { get; set; }
    public double? FontSize { get; set; }
    public string FontAttribute { get; set; }
    public string TextWrap { get; set; }
    public string FontAlign { get; set; }
    public int? SpaceBefore { get; set; }
    public string AdditionalData { get; set; }
    public string ContentType { get; set; }
    public string Content { get; set; }
}
