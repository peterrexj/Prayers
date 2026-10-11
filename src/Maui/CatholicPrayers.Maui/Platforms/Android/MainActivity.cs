using Android.App;
using Android.Content.PM;
using Android.OS;

namespace CatholicPrayers.Maui.Platforms.Android;

[Activity(Theme = "@style/Maui.SplashTheme",
           MainLauncher = true,
           LaunchMode = LaunchMode.SingleTop,
           ConfigurationChanges = ConfigChanges.ScreenSize
                                | ConfigChanges.Orientation
                                | ConfigChanges.UiMode
                                | ConfigChanges.ScreenLayout
                                | ConfigChanges.SmallestScreenSize
                                | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Tint status bar with persisted theme bg colour (set before MAUI loads)
        var hex = Preferences.Get("AppThemeBg", "#0D1B2A");
        try
        {
            var color = global::Android.Graphics.Color.ParseColor(hex);
            Window?.SetStatusBarColor(color);
        }
        catch { }
    }
}
