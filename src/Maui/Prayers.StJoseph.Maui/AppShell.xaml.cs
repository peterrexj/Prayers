using Prayers.StJoseph.Maui.Views;

namespace Prayers.StJoseph.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register prayer page routes dynamically for all 9 pages
        for (int i = 1; i <= 20; i++)
            Routing.RegisterRoute($"Page{i}", typeof(PrayerView));
    }
}
