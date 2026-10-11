using ObjCRuntime;
using UIKit;

namespace CatholicPrayers.Maui.Platforms.iOS;

public class Program
{
    [STAThread]
    static void Main(string[] args)
        => UIApplication.Main(args, null, typeof(AppDelegate));
}
