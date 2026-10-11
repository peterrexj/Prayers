namespace Prayers.StJoseph.Maui.Models;

public class PrayerAppModel
{
    public List<string> MainHeaders { get; set; } = [];
    public string? MainTitle { get; set; }
    public string? MainImage { get; set; }
    public List<SinglePageDataModel> SinglePageDataModels { get; set; } = [];
}
