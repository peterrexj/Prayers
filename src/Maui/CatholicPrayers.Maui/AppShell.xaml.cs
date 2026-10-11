using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.Services;
using CatholicPrayers.Maui.Views;

namespace CatholicPrayers.Maui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        Routing.RegisterRoute(nameof(PrayerPage), typeof(PrayerPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(CollectionPage), typeof(CollectionPage));
        Routing.RegisterRoute(nameof(LinkDetailPage), typeof(LinkDetailPage));
        Routing.RegisterRoute(nameof(PartnerAppPage), typeof(PartnerAppPage));
        Routing.RegisterRoute(nameof(BiblePage), typeof(BiblePage));
        Routing.RegisterRoute(nameof(BookPage), typeof(BookPage));

        _ = LoadPrayerMenuAsync();
    }

    private async Task LoadPrayerMenuAsync()
    {
        try
        {
            var prayerService = ServiceLocator.Get<IPrayerService>();
            if (prayerService == null) return;

            var master = await prayerService.GetMasterDataAsync();
            var categories = prayerService.GetCategories(master);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                foreach (var category in categories)
                {
                    var prayers = master.Prayers
                        .Where(p => p.Category == category)
                        .OrderBy(p => p.SortOrder)
                        .ToList();

                    foreach (var prayer in prayers)
                    {
                        var prayerEntry = prayer;
                        var menuItem = new MenuItem
                        {
                            Text = prayerEntry.Title,
                            IconImageSource = new FontImageSource
                            {
                                FontFamily = "FontAwesome5Solid",
                                Glyph = prayerEntry.Icon,
                                Size = 18,
                                Color = Application.Current?.Resources.TryGetValue("PrayAppAccentColor", out var accentColor) == true
                                    ? (Color)accentColor
                                    : Color.FromArgb("#9D7DE8")
                            }
                        };
                        menuItem.Clicked += async (_, _) =>
                        {
                            await Current.GoToAsync($"{nameof(PrayerPage)}?PrayerId={prayerEntry.Id}");
                            Current.FlyoutIsPresented = false;
                        };
                        Items.Add(menuItem);
                    }
                }
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Shell menu error: {ex.Message}");
        }
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        FlyoutIsPresented = false;
        await GoToAsync(nameof(SettingsPage));
    }
}
