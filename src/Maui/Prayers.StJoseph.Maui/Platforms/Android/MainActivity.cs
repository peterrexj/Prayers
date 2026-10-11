using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Prayers.StJoseph.Maui;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode
        | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ApplyStatusBarColor();
    }

    private void ApplyStatusBarColor()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop) return;
        Window?.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
        Window?.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
        Window?.SetStatusBarColor(Android.Graphics.Color.ParseColor("#1B2631"));
    }
}
