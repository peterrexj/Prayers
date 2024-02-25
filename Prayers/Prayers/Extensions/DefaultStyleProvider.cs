using MagicGradients;
using Prayers.Models;
using Prayers.ViewModels.Extras;
using Xamarin.Forms;

namespace Prayers.Extensions
{
    public class DefaultStyleProvider
    {
        public static StyleModelDefault LoadDefaultStyle(AppThemes appTheme)
        {
            switch (appTheme)
            {
                case AppThemes.Dark:
                    return new StyleModelDefault
                    {
                        AppTheme = appTheme,

                        //Background
                        DefaultMgPageBgSource = CssGradientSource.Parse("linear-gradient(116deg, rgba(232,232,232,0.0299999993294477) 0%, rgba(232,232,232,0.0299999993294477) 10%, rgba(14,14,14,0.0299999993294477) 10%, rgba(14,14,14,0.0299999993294477) 66%, rgba(232,232,232,0.0299999993294477) 66%, rgba(232,232,232,0.0299999993294477) 72%, rgba(44,44,44,0.0299999993294477) 72%, rgba(44,44,44,0.0299999993294477) 81%, rgba(51,51,51,0.0299999993294477) 81%, rgba(51,51,51,0.0299999993294477) 100%),linear-gradient(109deg, rgba(155,155,155,0.0299999993294477) 0%, rgba(155,155,155,0.0299999993294477) 23%, rgba(30,30,30,0.0299999993294477) 23%, rgba(30,30,30,0.0299999993294477) 63%, rgba(124,124,124,0.0299999993294477) 63%, rgba(124,124,124,0.0299999993294477) 73%, rgba(195,195,195,0.0299999993294477) 73%, rgba(195,195,195,0.0299999993294477) 84%, rgba(187,187,187,0.0299999993294477) 84%, rgba(187,187,187,0.0299999993294477) 100%),linear-gradient(79deg, rgba(254,254,254,0.0299999993294477) 0%, rgba(254,254,254,0.0299999993294477) 27%, rgba(180,180,180,0.0299999993294477) 27%, rgba(180,180,180,0.0299999993294477) 33%, rgba(167,167,167,0.0299999993294477) 33%, rgba(167,167,167,0.0299999993294477) 34%, rgba(68,68,68,0.0299999993294477) 34%, rgba(68,68,68,0.0299999993294477) 63%, rgba(171,171,171,0.0299999993294477) 63%, rgba(171,171,171,0.0299999993294477) 100%),linear-gradient(109deg, rgba(71,71,71,0.0299999993294477) 0%, rgba(71,71,71,0.0299999993294477) 3%, rgba(97,97,97,0.0299999993294477) 3%, rgba(97,97,97,0.0299999993294477) 40%, rgba(40,40,40,0.0299999993294477) 40%, rgba(40,40,40,0.0299999993294477) 55%, rgba(5,5,5,0.0299999993294477) 55%, rgba(5,5,5,0.0299999993294477) 73%, rgba(242,242,242,0.0299999993294477) 73%, rgba(242,242,242,0.0299999993294477) 100%),linear-gradient(271deg, rgba(70,70,70,0.0299999993294477) 0%, rgba(70,70,70,0.0299999993294477) 11%, rgba(178,178,178,0.0299999993294477) 11%, rgba(178,178,178,0.0299999993294477) 23%, rgba(28,28,28,0.0299999993294477) 23%, rgba(28,28,28,0.0299999993294477) 72%, rgba(152,152,152,0.0299999993294477) 72%, rgba(152,152,152,0.0299999993294477) 86%, rgba(43,43,43,0.0299999993294477) 86%, rgba(43,43,43,0.0299999993294477) 100%),linear-gradient(90deg, rgba(27,27,27,1) 0%, rgba(1,1,1,1) 100%)"),

                        //Box
                        BoxFontColor = "#2C3E50",

                        //Top Highlight boxes for summary
                        BoxBgSource = CssGradientSource.Parse("linear-gradient(135deg, rgba(101,215,91,0.259999990463257) 0%, rgba(101,215,91,0.259999990463257) 2%, rgba(87,200,111,0.259999990463257) 2%, rgba(87,200,111,0.259999990463257) 7%, rgba(72,185,131,0.259999990463257) 7%, rgba(72,185,131,0.259999990463257) 16%, rgba(58,170,151,0.259999990463257) 16%, rgba(58,170,151,0.259999990463257) 24%, rgba(43,154,170,0.259999990463257) 24%, rgba(43,154,170,0.259999990463257) 67%, rgba(29,139,190,0.259999990463257) 67%, rgba(29,139,190,0.259999990463257) 72%, rgba(14,124,210,0.259999990463257) 72%, rgba(14,124,210,0.259999990463257) 100%),linear-gradient(135deg, rgba(96,227,174,0.259999990463257) 0%, rgba(96,227,174,0.259999990463257) 32%, rgba(111,197,164,0.259999990463257) 32%, rgba(111,197,164,0.259999990463257) 55%, rgba(126,168,154,0.259999990463257) 55%, rgba(126,168,154,0.259999990463257) 69%, rgba(141,138,145,0.259999990463257) 69%, rgba(141,138,145,0.259999990463257) 79%, rgba(156,108,135,0.259999990463257) 79%, rgba(156,108,135,0.259999990463257) 82%, rgba(171,79,125,0.259999990463257) 82%, rgba(171,79,125,0.259999990463257) 84%, rgba(186,49,115,0.259999990463257) 84%, rgba(186,49,115,0.259999990463257) 100%),linear-gradient(90deg, rgba(60,34,11,1) 0%, rgba(60,34,11,1) 3%, rgba(60,51,15,1) 3%, rgba(60,51,15,1) 4%, rgba(60,68,19,1) 4%, rgba(60,68,19,1) 12%, rgba(60,85,24,1) 12%, rgba(60,85,24,1) 40%, rgba(60,101,28,1) 40%, rgba(60,101,28,1) 69%, rgba(60,118,32,1) 69%, rgba(60,118,32,1) 88%, rgba(60,135,36,1) 88%, rgba(60,135,36,1) 100%)"),
                        BoxBorderColor = "#D0D3D4",

                        //Buttons
                        ButtonFgColor = "#D4EFDF",
                        ButtonBgColor = "#0B5345",
                        ThemeButtonBgColor = "#EAEDED",

                        //Progress
                        ProgressBarTrackColor = "#0B5345",
                        ProgressBarInProgressMarkerFillColor = "#717D7E",
                        ProgressBarInProgressMarkerStrokeColor = "#1A5276",
                        ProgressBarCompletedBgColor = "#0B5345",
                        ProgressBarIconMainBgColor = "#D4EFDF",

                        //Fonts
                        DefaultFontColor = "#BFC9CA",

                        //App Theme Base
                        AppShellBgColor = "#1B2631",
                        AppShellFgColor = "#B3B6B7",
                        AppShellTitleColor = "",
                        AppShellDisabledColor = "",
                        AppShellUnselectedColor = "",
                        AppShellTabBarBackgroundColor = "#1B2631",
                        AppShellTabBarForegroundColor = "#B3B6B7",
                        AppShellTabBarUnselectedColor = "",
                        AppShellTabBarDisabledColor = "",
                        AppShellTabBarTitleColor = "",
                        AppShellBackgroundColor = "#447e58",
                    };
                case AppThemes.Light:
                    return new StyleModelDefault
                    {
                        AppTheme = appTheme,

                        DefaultMgPageBgSource = CssGradientSource.Parse("linear-gradient(45deg, rgba(255,255,255,1) 0%, rgba(0,0,0,0) 100%),linear-gradient(45deg, rgba(255,255,255,1) 0%, rgba(0,0,0,0) 100%),repeating-linear-gradient(135deg, rgba(175,175,175,0.150000005960464) 0%, rgba(175,175,175,0.150000005960464) 0.1915828%, rgba(0,0,0,0) 0.1915828%, rgba(0,0,0,0) 0.3831657%),linear-gradient(90deg, rgba(255,255,255,1) 0%, rgba(255,255,255,1) 100%)"),

                        //Box
                        BoxFontColor = "#1A5276",

                        //Top Highlight boxes for summary
                        BoxBgSource = CssGradientSource.Parse("linear-gradient(135deg, rgba(19,176,223,0.259999990463257) 0%, rgba(19,176,223,0.259999990463257) 23%, rgba(16,160,197,0.259999990463257) 23%, rgba(16,160,197,0.259999990463257) 65%, rgba(13,144,172,0.259999990463257) 65%, rgba(13,144,172,0.259999990463257) 70%, rgba(9,129,146,0.259999990463257) 70%, rgba(9,129,146,0.259999990463257) 74%, rgba(6,113,121,0.259999990463257) 74%, rgba(6,113,121,0.259999990463257) 90%, rgba(3,97,95,0.259999990463257) 90%, rgba(3,97,95,0.259999990463257) 100%),linear-gradient(45deg, rgba(65,234,230,0.259999990463257) 0%, rgba(65,234,230,0.259999990463257) 28%, rgba(88,192,215,0.259999990463257) 28%, rgba(88,192,215,0.259999990463257) 55%, rgba(110,150,201,0.259999990463257) 55%, rgba(110,150,201,0.259999990463257) 66%, rgba(133,107,186,0.259999990463257) 66%, rgba(133,107,186,0.259999990463257) 80%),linear-gradient(90deg, rgba(27,194,246,1) 0%, rgba(27,194,246,1) 24.17609%, rgba(39,174,237,1) 39.94703%)"),
                        BoxBorderColor = "#D6EAF8",

                        //Buttons
                        ButtonFgColor = "#FBFCFC",
                        ButtonBgColor = "#1A5276",
                        ThemeButtonBgColor = "#2E4053",

                        //Progress
                        //ProgressBarNotStartedMarkerContentFillColor = "#1A5276",
                        //ProgressBarNotStartedMarkerStrokeColor = "#1A5276",
                        ProgressBarTrackColor = "#1A5276",
                        ProgressBarInProgressMarkerFillColor = "#FBFCFC",
                        ProgressBarInProgressMarkerStrokeColor = "#1A5276",
                        ProgressBarCompletedBgColor = "#AED6F1",
                        ProgressBarIconMainBgColor = "#1A5276",

                        //Fonts
                        DefaultFontColor = "#17202A",

                        //App Theme Base
                        AppShellBgColor = "#B3B6B7",
                        AppShellFgColor = "#283747",
                        AppShellTitleColor = "#283747",
                        AppShellDisabledColor = "#979A9A", //does not Works
                        AppShellUnselectedColor = "#5D6D7E",
                        AppShellTabBarBackgroundColor = "#B3B6B7", //background
                        AppShellTabBarForegroundColor = "#283747", //does not Works
                        AppShellTabBarUnselectedColor = "#5D6D7E",
                        AppShellTabBarDisabledColor = "#979A9A", //does not Works
                        AppShellTabBarTitleColor = "#21618C", //selected foreground color
                        AppShellBackgroundColor = "#3091c4",
                    };
                default:
                    return null;
            }
        }
    }
}
