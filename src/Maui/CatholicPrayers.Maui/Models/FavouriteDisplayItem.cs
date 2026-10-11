namespace CatholicPrayers.Maui.Models;

public class FavouriteDisplayItem
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;   // "Prayer" | "Novena" | "Bible Book" | "Bible Chapter"
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string AccentColor { get; set; } = "#4A90D9";
    public string Route { get; set; } = string.Empty;

    public bool IsBible => Type is "Bible Book" or "Bible Chapter";
}
