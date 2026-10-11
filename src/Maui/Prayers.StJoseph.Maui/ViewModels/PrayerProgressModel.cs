using Syncfusion.Maui.ProgressBar;

namespace Prayers.StJoseph.Maui.ViewModels;

public class PrayerProgressModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public StepStatus Status { get; set; }
    public int ProgressValue { get; set; }
}
