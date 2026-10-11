using Foundation;
using UIKit;

namespace CatholicPrayers.Maui.Platforms.iOS;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        var result = base.FinishedLaunching(application, launchOptions);

        // Apply persisted theme background to the iOS window
        var hex = Preferences.Get("AppThemeBg", "#0D1B2A");
        try
        {
            var color = Color.FromArgb(hex);
            var uiColor = UIColor.FromRGB(
                (nfloat)color.Red, (nfloat)color.Green, (nfloat)color.Blue);
            foreach (var scene in UIApplication.SharedApplication.ConnectedScenes)
            {
                if (scene is UIWindowScene ws)
                    foreach (var win in ws.Windows)
                        win.BackgroundColor = uiColor;
            }
        }
        catch { }

        return result;
    }
}
