using MagicGradients;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Prayers.ViewModels.Extras
{
    public class StyleModelDefault : BasePropertyChangeModel
    {
        public string DefaultForegroundColor { get; set; }
        public IGradientSource DefaultMgPageBgSource { get; set; }

        //Box
        public IGradientSource BoxBgSource { get; set; }
        public string BoxBorderColor { get; set; }

        //Highlight Box
        public IGradientSource HighlightBgSource { get; set; }
        public string HighlightBoxBorderColor { get; set; }
        public string HighlightFgColor { get; set; }

        //Input Text
        public string InputTextFgColor { get; set; }
        public string InputTextWhenFocusedColor { get; set; }
        public string InputTextWhenUnFocusedColor { get; set; }
        public string InputErrorTextFgColor { get; set; }

        //Button
        public IGradientSource ButtonBgSource { get; set; }
        public string ButtonFgColor { get; set; }
        public string ButtonBgColor { get; set; }

        //Fonts
        public string DefaultFontFamily { get; set; }
        public string DefaultFontColor { get; set; }
        public double BoxItemCurrencyFontSize { get; set; }
        public double BoxItemHighlightNumberFontSize { get; set; }


        //App Theme Base
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
