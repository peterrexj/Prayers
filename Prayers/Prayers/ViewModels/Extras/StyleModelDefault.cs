using MagicGradients;
using Prayers.Models;
using Prayers.Services;
using Xamarin.Forms;

namespace Prayers.ViewModels.Extras
{
    public class StyleModelDefault : BasePropertyChangeModel
    {
        public IGradientSource DefaultMgPageBgSource { get; set; }
        public Color PageBgColorConverted => SharedServices.ToColorConverter.ToColorFromHex(AppShellBackgroundColor);

        //Box
        public string BoxFontColor { get; set; }

        //Highlight Box
        public IGradientSource BoxBgSource { get; set; }
        public string BoxBorderColor { get; set; }

        //Button
        public string ButtonFgColor { get; set; }
        public string ButtonBgColor { get; set; }
        public string ThemeButtonBgColor { get; set; }

        //Progress Bar
        public string ProgressBarTrackColor { get; set; }
        public string ProgressBarInProgressMarkerFillColor { get; set; }
        public string ProgressBarInProgressMarkerStrokeColor { get; set; }
        public string ProgressBarCompletedBgColor { get; set; }
        public string ProgressBarIconMainBgColor { get; set; }


        //Fonts
        public string DefaultFontColor { get; set; }


        //App Theme Base
        public AppThemes AppTheme { get; set; }
        public string AppShellBackgroundColor { get; set; }
        public string AppShellBgColor { get; set; }
        public string AppShellFgColor { get; set; }
        public string AppShellTitleColor { get; set; }
        public string AppShellDisabledColor { get; set; }
        public string AppShellUnselectedColor { get; set; }
        public string AppShellTabBarBackgroundColor { get; set; }
        public string AppShellTabBarForegroundColor { get; set; }
        public string AppShellTabBarUnselectedColor { get; set; }
        public string AppShellTabBarDisabledColor { get; set; }
        public string AppShellTabBarTitleColor { get; set; }
    }
}
