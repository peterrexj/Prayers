using ObjCRuntime;
using UIKit;

namespace CatholicPrayers.Maui.Platforms.MacCatalyst;

public class Program
{
    [STAThread]
    static void Main(string[] args)
        => UIApplication.Main(args, null, typeof(AppDelegate));
}
