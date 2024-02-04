﻿using MagicGradients;
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
                        //Background
                        DefaultForegroundColor = "White",
                        DefaultMgPageBgSource = CssGradientSource.Parse("linear-gradient(116deg, rgba(232,232,232,0.0299999993294477) 0%, rgba(232,232,232,0.0299999993294477) 10%, rgba(14,14,14,0.0299999993294477) 10%, rgba(14,14,14,0.0299999993294477) 66%, rgba(232,232,232,0.0299999993294477) 66%, rgba(232,232,232,0.0299999993294477) 72%, rgba(44,44,44,0.0299999993294477) 72%, rgba(44,44,44,0.0299999993294477) 81%, rgba(51,51,51,0.0299999993294477) 81%, rgba(51,51,51,0.0299999993294477) 100%),linear-gradient(109deg, rgba(155,155,155,0.0299999993294477) 0%, rgba(155,155,155,0.0299999993294477) 23%, rgba(30,30,30,0.0299999993294477) 23%, rgba(30,30,30,0.0299999993294477) 63%, rgba(124,124,124,0.0299999993294477) 63%, rgba(124,124,124,0.0299999993294477) 73%, rgba(195,195,195,0.0299999993294477) 73%, rgba(195,195,195,0.0299999993294477) 84%, rgba(187,187,187,0.0299999993294477) 84%, rgba(187,187,187,0.0299999993294477) 100%),linear-gradient(79deg, rgba(254,254,254,0.0299999993294477) 0%, rgba(254,254,254,0.0299999993294477) 27%, rgba(180,180,180,0.0299999993294477) 27%, rgba(180,180,180,0.0299999993294477) 33%, rgba(167,167,167,0.0299999993294477) 33%, rgba(167,167,167,0.0299999993294477) 34%, rgba(68,68,68,0.0299999993294477) 34%, rgba(68,68,68,0.0299999993294477) 63%, rgba(171,171,171,0.0299999993294477) 63%, rgba(171,171,171,0.0299999993294477) 100%),linear-gradient(109deg, rgba(71,71,71,0.0299999993294477) 0%, rgba(71,71,71,0.0299999993294477) 3%, rgba(97,97,97,0.0299999993294477) 3%, rgba(97,97,97,0.0299999993294477) 40%, rgba(40,40,40,0.0299999993294477) 40%, rgba(40,40,40,0.0299999993294477) 55%, rgba(5,5,5,0.0299999993294477) 55%, rgba(5,5,5,0.0299999993294477) 73%, rgba(242,242,242,0.0299999993294477) 73%, rgba(242,242,242,0.0299999993294477) 100%),linear-gradient(271deg, rgba(70,70,70,0.0299999993294477) 0%, rgba(70,70,70,0.0299999993294477) 11%, rgba(178,178,178,0.0299999993294477) 11%, rgba(178,178,178,0.0299999993294477) 23%, rgba(28,28,28,0.0299999993294477) 23%, rgba(28,28,28,0.0299999993294477) 72%, rgba(152,152,152,0.0299999993294477) 72%, rgba(152,152,152,0.0299999993294477) 86%, rgba(43,43,43,0.0299999993294477) 86%, rgba(43,43,43,0.0299999993294477) 100%),linear-gradient(90deg, rgba(27,27,27,1) 0%, rgba(1,1,1,1) 100%)"),

                        //Box
                        BoxBgSource = CssGradientSource.Parse("linear-gradient(56deg, rgba(130,130,130,0.0500000007450581) 0%, rgba(130,130,130,0.0500000007450581) 33.33%, rgba(255,255,255,0.0500000007450581) 33.33%, rgba(255,255,255,0.0500000007450581) 66.67%, rgba(198,198,198,0.0500000007450581) 66.67%, rgba(198,198,198,0.0500000007450581) 100%),linear-gradient(29deg, rgba(94,94,94,0.0500000007450581) 0%, rgba(94,94,94,0.0500000007450581) 33.33%, rgba(185,185,185,0.0500000007450581) 33.33%, rgba(185,185,185,0.0500000007450581) 66.67%, rgba(113,113,113,0.0500000007450581) 66.67%, rgba(113,113,113,0.0500000007450581) 100%),linear-gradient(129deg, rgba(196,196,196,0.0500000007450581) 0%, rgba(196,196,196,0.0500000007450581) 33.33%, rgba(148,148,148,0.0500000007450581) 33.33%, rgba(148,148,148,0.0500000007450581) 66.67%, rgba(24,24,24,0.0500000007450581) 66.67%, rgba(24,24,24,0.0500000007450581) 100%),linear-gradient(76deg, rgba(19,19,19,0.0500000007450581) 0%, rgba(19,19,19,0.0500000007450581) 33.33%, rgba(159,159,159,0.0500000007450581) 33.33%, rgba(159,159,159,0.0500000007450581) 66.67%, rgba(108,108,108,0.0500000007450581) 66.67%, rgba(108,108,108,0.0500000007450581) 100%),linear-gradient(112deg, rgba(225,225,225,0.0500000007450581) 0%, rgba(225,225,225,0.0500000007450581) 33.33%, rgba(13,13,13,0.0500000007450581) 33.33%, rgba(13,13,13,0.0500000007450581) 66.67%, rgba(81,81,81,0.0500000007450581) 66.67%, rgba(81,81,81,0.0500000007450581) 100%),linear-gradient(90deg, rgba(75,2,28,1) 0%, rgba(175,60,142,1) 100%)"),
                        BoxBorderColor = "#F5EEF8",

                        //Top Highlight boxes for summary
                        HighlightBgSource = CssGradientSource.Parse("radial-gradient(circle farthest-corner at 19% 27%, rgba(255,255,255,0.00999999977648258) 0%, rgba(255,255,255,0.00999999977648258) 3%, rgba(0,0,0,0) 3%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 14% 15%, rgba(255,255,255,0.00999999977648258) 0%, rgba(255,255,255,0.00999999977648258) 3%, rgba(0,0,0,0) 3%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 52% 35%, rgba(255,255,255,0.0299999993294477) 0%, rgba(255,255,255,0.0299999993294477) 3%, rgba(0,0,0,0) 3%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 52% 35%, rgba(255,255,255,0.0299999993294477) 0%, rgba(255,255,255,0.0299999993294477) 3%, rgba(0,0,0,0) 3%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 57% 90%, rgba(255,255,255,0.0299999993294477) 0%, rgba(255,255,255,0.0299999993294477) 3%, rgba(0,0,0,0) 3%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 21% 88%, rgba(255,255,255,0.00999999977648258) 0%, rgba(255,255,255,0.00999999977648258) 7%, rgba(0,0,0,0) 7%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 15% 0%, rgba(255,255,255,0.0299999993294477) 0%, rgba(255,255,255,0.0299999993294477) 7%, rgba(0,0,0,0) 7%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 0% 66%, rgba(255,255,255,0.0299999993294477) 0%, rgba(255,255,255,0.0299999993294477) 7%, rgba(0,0,0,0) 7%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 41% 50%, rgba(255,255,255,0.0299999993294477) 0%, rgba(255,255,255,0.0299999993294477) 7%, rgba(0,0,0,0) 7%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 61% 49%, rgba(255,255,255,0.0199999995529652) 0%, rgba(255,255,255,0.0199999995529652) 7%, rgba(0,0,0,0) 7%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 62% 81%, rgba(255,255,255,0.00999999977648258) 0%, rgba(255,255,255,0.00999999977648258) 7%, rgba(0,0,0,0) 7%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 20% 47%, rgba(255,255,255,0.0199999995529652) 0%, rgba(255,255,255,0.0199999995529652) 7%, rgba(0,0,0,0) 7%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 26% 82%, rgba(255,255,255,0.0299999993294477) 0%, rgba(255,255,255,0.0299999993294477) 5%, rgba(0,0,0,0) 5%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 46% 9%, rgba(255,255,255,0.00999999977648258) 0%, rgba(255,255,255,0.00999999977648258) 5%, rgba(0,0,0,0) 5%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 47% 6%, rgba(255,255,255,0.0299999993294477) 0%, rgba(255,255,255,0.0299999993294477) 5%, rgba(0,0,0,0) 5%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 43% 28%, rgba(255,255,255,0.0199999995529652) 0%, rgba(255,255,255,0.0199999995529652) 5%, rgba(0,0,0,0) 5%, rgba(0,0,0,0) 100%),radial-gradient(circle farthest-corner at 67% 100%, rgba(255,255,255,0.00999999977648258) 0%, rgba(255,255,255,0.00999999977648258) 5%, rgba(0,0,0,0) 5%, rgba(0,0,0,0) 100%),linear-gradient(90deg, rgba(99,57,212,1) 0%, rgba(145,54,196,1) 100%)"),
                        HighlightBoxBorderColor = "#D2B4DE",
                        HighlightFgColor = "#1B2631",

                        //InputText
                        InputTextFgColor = "#BFC9CA",
                        InputTextWhenFocusedColor = "#A9DFBF",
                        InputTextWhenUnFocusedColor = "#808B96",
                        InputErrorTextFgColor = "#F5B7B1",

                        //Buttons
                        ButtonBgSource = CssGradientSource.Parse("radial-gradient(circle farthest-corner at 50% 50%, rgba(0,0,0,0) 0%, rgba(255,255,255,1) 100%),linear-gradient(309deg, rgba(90,90,90,0.0500000007450581) 0%, rgba(90,90,90,0.0500000007450581) 50%, rgba(206,206,206,0.0500000007450581) 50%, rgba(206,206,206,0.0500000007450581) 100%),linear-gradient(39deg, rgba(13,13,13,0.0500000007450581) 0%, rgba(13,13,13,0.0500000007450581) 50%, rgba(189,189,189,0.0500000007450581) 50%, rgba(189,189,189,0.0500000007450581) 100%),linear-gradient(144deg, rgba(249,249,249,0.0500000007450581) 0%, rgba(249,249,249,0.0500000007450581) 50%, rgba(111,111,111,0.0500000007450581) 50%, rgba(111,111,111,0.0500000007450581) 100%),linear-gradient(166deg, rgba(231,231,231,0.0500000007450581) 0%, rgba(231,231,231,0.0500000007450581) 50%, rgba(220,220,220,0.0500000007450581) 50%, rgba(220,220,220,0.0500000007450581) 100%),linear-gradient(212deg, rgba(80,80,80,0.0500000007450581) 0%, rgba(80,80,80,0.0500000007450581) 50%, rgba(243,243,243,0.0500000007450581) 50%, rgba(243,243,243,0.0500000007450581) 100%),radial-gradient(circle farthest-corner at 50% 50%, rgba(255,255,255,1) 0%, rgba(255,255,255,1) 100%)"),
                        ButtonFgColor = "Black",

                        //Fonts
                        DefaultFontFamily = "Calibri",
                        DefaultFontColor = "#BFC9CA",
                        BoxItemCurrencyFontSize = BoxCurrencyFontSize,
                        BoxItemHighlightNumberFontSize = BoxMainHighlightFontSize,

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
                    };
                case AppThemes.Light:
                    return new StyleModelDefault
                    {
                        DefaultForegroundColor = "Black",
                        DefaultMgPageBgSource = CssGradientSource.Parse("linear-gradient(45deg, rgba(255,255,255,1) 0%, rgba(0,0,0,0) 100%),linear-gradient(45deg, rgba(255,255,255,1) 0%, rgba(0,0,0,0) 100%),repeating-linear-gradient(135deg, rgba(175,175,175,0.150000005960464) 0%, rgba(175,175,175,0.150000005960464) 0.1915828%, rgba(0,0,0,0) 0.1915828%, rgba(0,0,0,0) 0.3831657%),linear-gradient(90deg, rgba(255,255,255,1) 0%, rgba(255,255,255,1) 100%)"),

                        //Box
                        BoxBgSource = CssGradientSource.Parse("linear-gradient(144deg, rgba(192,192,192,0.0299999993294477) 0%, rgba(192,192,192,0.0299999993294477) 23%, rgba(36,36,36,0.0299999993294477) 23%, rgba(36,36,36,0.0299999993294477) 29%, rgba(248,248,248,0.0299999993294477) 29%, rgba(248,248,248,0.0299999993294477) 44%, rgba(250,250,250,0.0299999993294477) 44%, rgba(250,250,250,0.0299999993294477) 61%, rgba(36,36,36,0.0299999993294477) 61%, rgba(36,36,36,0.0299999993294477) 100%),linear-gradient(200deg, rgba(81,81,81,0.0299999993294477) 0%, rgba(81,81,81,0.0299999993294477) 68%, rgba(36,36,36,0.0299999993294477) 68%, rgba(36,36,36,0.0299999993294477) 82%, rgba(187,187,187,0.0299999993294477) 82%, rgba(187,187,187,0.0299999993294477) 95%, rgba(13,13,13,0.0299999993294477) 95%, rgba(13,13,13,0.0299999993294477) 97%, rgba(107,107,107,0.0299999993294477) 97%, rgba(107,107,107,0.0299999993294477) 100%),linear-gradient(228deg, rgba(248,248,248,0.0299999993294477) 0%, rgba(248,248,248,0.0299999993294477) 25%, rgba(125,125,125,0.0299999993294477) 25%, rgba(125,125,125,0.0299999993294477) 28%, rgba(228,228,228,0.0299999993294477) 28%, rgba(228,228,228,0.0299999993294477) 32%, rgba(185,185,185,0.0299999993294477) 32%, rgba(185,185,185,0.0299999993294477) 84%, rgba(82,82,82,0.0299999993294477) 84%, rgba(82,82,82,0.0299999993294477) 100%),linear-gradient(244deg, rgba(15,15,15,0.0299999993294477) 0%, rgba(15,15,15,0.0299999993294477) 35%, rgba(224,224,224,0.0299999993294477) 35%, rgba(224,224,224,0.0299999993294477) 52%, rgba(251,251,251,0.0299999993294477) 52%, rgba(251,251,251,0.0299999993294477) 69%, rgba(83,83,83,0.0299999993294477) 69%, rgba(83,83,83,0.0299999993294477) 72%, rgba(219,219,219,0.0299999993294477) 72%, rgba(219,219,219,0.0299999993294477) 100%),linear-gradient(338deg, rgba(57,57,57,0.0299999993294477) 0%, rgba(57,57,57,0.0299999993294477) 32%, rgba(166,166,166,0.0299999993294477) 32%, rgba(166,166,166,0.0299999993294477) 54%, rgba(161,161,161,0.0299999993294477) 54%, rgba(161,161,161,0.0299999993294477) 56%, rgba(158,158,158,0.0299999993294477) 56%, rgba(158,158,158,0.0299999993294477) 61%, rgba(25,25,25,0.0299999993294477) 61%, rgba(25,25,25,0.0299999993294477) 100%),linear-gradient(90deg, rgba(225,225,225,1) 0%, rgba(231,231,231,1) 100%)"),
                        BoxBorderColor = "#F5EEF8",

                        //Top Highlight boxes for summary
                        HighlightBgSource = CssGradientSource.Parse("linear-gradient(135deg, rgba(19,176,223,0.259999990463257) 0%, rgba(19,176,223,0.259999990463257) 23%, rgba(16,160,197,0.259999990463257) 23%, rgba(16,160,197,0.259999990463257) 65%, rgba(13,144,172,0.259999990463257) 65%, rgba(13,144,172,0.259999990463257) 70%, rgba(9,129,146,0.259999990463257) 70%, rgba(9,129,146,0.259999990463257) 74%, rgba(6,113,121,0.259999990463257) 74%, rgba(6,113,121,0.259999990463257) 90%, rgba(3,97,95,0.259999990463257) 90%, rgba(3,97,95,0.259999990463257) 100%),linear-gradient(45deg, rgba(65,234,230,0.259999990463257) 0%, rgba(65,234,230,0.259999990463257) 28%, rgba(88,192,215,0.259999990463257) 28%, rgba(88,192,215,0.259999990463257) 55%, rgba(110,150,201,0.259999990463257) 55%, rgba(110,150,201,0.259999990463257) 66%, rgba(133,107,186,0.259999990463257) 66%, rgba(133,107,186,0.259999990463257) 80%, rgba(155,65,172,0.259999990463257) 80%, rgba(155,65,172,0.259999990463257) 85%, rgba(178,23,157,0.259999990463257) 85%, rgba(178,23,157,0.259999990463257) 100%),linear-gradient(90deg, rgba(27,194,246,1) 0%, rgba(27,194,246,1) 6%, rgba(39,174,237,1) 6%, rgba(39,174,237,1) 32%, rgba(50,155,229,1) 32%, rgba(50,155,229,1) 40%, rgba(62,135,220,1) 40%, rgba(62,135,220,1) 66%, rgba(74,116,211,1) 66%, rgba(74,116,211,1) 72%, rgba(86,96,202,1) 72%, rgba(86,96,202,1) 86%, rgba(97,77,194,1) 86%, rgba(97,77,194,1) 96%, rgba(109,57,185,1) 96%, rgba(109,57,185,1) 100%)"),
                        //HighlightBgSource = CssGradientSource.Parse("linear-gradient(12deg, rgba(193,193,193,0.0500000007450581) 0%, rgba(193,193,193,0.0500000007450581) 2%, rgba(129,129,129,0.0500000007450581) 2%, rgba(129,129,129,0.0500000007450581) 27%, rgba(185,185,185,0.0500000007450581) 27%, rgba(185,185,185,0.0500000007450581) 66%, rgba(83,83,83,0.0500000007450581) 66%, rgba(83,83,83,0.0500000007450581) 100%),linear-gradient(321deg, rgba(240,240,240,0.0500000007450581) 0%, rgba(240,240,240,0.0500000007450581) 13%, rgba(231,231,231,0.0500000007450581) 13%, rgba(231,231,231,0.0500000007450581) 34%, rgba(139,139,139,0.0500000007450581) 34%, rgba(139,139,139,0.0500000007450581) 71%, rgba(112,112,112,0.0500000007450581) 71%, rgba(112,112,112,0.0500000007450581) 100%),linear-gradient(236deg, rgba(189,189,189,0.0500000007450581) 0%, rgba(189,189,189,0.0500000007450581) 47%, rgba(138,138,138,0.0500000007450581) 47%, rgba(138,138,138,0.0500000007450581) 58%, rgba(108,108,108,0.0500000007450581) 58%, rgba(108,108,108,0.0500000007450581) 85%, rgba(143,143,143,0.0500000007450581) 85%, rgba(143,143,143,0.0500000007450581) 100%),linear-gradient(96deg, rgba(53,53,53,0.0500000007450581) 0%, rgba(53,53,53,0.0500000007450581) 53%, rgba(44,44,44,0.0500000007450581) 53%, rgba(44,44,44,0.0500000007450581) 82%, rgba(77,77,77,0.0500000007450581) 82%, rgba(77,77,77,0.0500000007450581) 98%, rgba(8,8,8,0.0500000007450581) 98%, rgba(8,8,8,0.0500000007450581) 100%),linear-gradient(334deg, rgba(5,5,5,1) 0%, rgba(5,5,5,1) 100%)"),
                        HighlightBoxBorderColor = "#D6EAF8",
                        HighlightFgColor = "White",

                        //InputText
                        InputTextFgColor = "#17202A",
                        InputTextWhenFocusedColor = "#566573",
                        InputTextWhenUnFocusedColor = "#17202A",
                        InputErrorTextFgColor = "#CA6F1E",

                        //Buttons
                        ButtonBgSource = CssGradientSource.Parse("linear-gradient(181deg, rgba(210,210,210,0.00999999977648258) 0%, rgba(210,210,210,0.00999999977648258) 12.5%, rgba(156,156,156,0.00999999977648258) 12.5%, rgba(156,156,156,0.00999999977648258) 25%, rgba(90,90,90,0.00999999977648258) 25%, rgba(90,90,90,0.00999999977648258) 37.5%, rgba(11,11,11,0.00999999977648258) 37.5%, rgba(11,11,11,0.00999999977648258) 50%, rgba(53,53,53,0.00999999977648258) 50%, rgba(53,53,53,0.00999999977648258) 62.5%, rgba(115,115,115,0.00999999977648258) 62.5%, rgba(115,115,115,0.00999999977648258) 75%, rgba(34,34,34,0.00999999977648258) 75%, rgba(34,34,34,0.00999999977648258) 87.5%, rgba(110,110,110,0.00999999977648258) 87.5%, rgba(110,110,110,0.00999999977648258) 100%),linear-gradient(120deg, rgba(197,197,197,0.0299999993294477) 0%, rgba(197,197,197,0.0299999993294477) 12.5%, rgba(16,16,16,0.0299999993294477) 12.5%, rgba(16,16,16,0.0299999993294477) 25%, rgba(44,44,44,0.0299999993294477) 25%, rgba(44,44,44,0.0299999993294477) 37.5%, rgba(244,244,244,0.0299999993294477) 37.5%, rgba(244,244,244,0.0299999993294477) 50%, rgba(88,88,88,0.0299999993294477) 50%, rgba(88,88,88,0.0299999993294477) 62.5%, rgba(101,101,101,0.0299999993294477) 62.5%, rgba(101,101,101,0.0299999993294477) 75%, rgba(44,44,44,0.0299999993294477) 75%, rgba(44,44,44,0.0299999993294477) 87.5%, rgba(212,212,212,0.0299999993294477) 87.5%, rgba(212,212,212,0.0299999993294477) 100%),linear-gradient(216deg, rgba(24,24,24,0.0199999995529652) 0%, rgba(24,24,24,0.0199999995529652) 20%, rgba(101,101,101,0.0199999995529652) 20%, rgba(101,101,101,0.0199999995529652) 40%, rgba(44,44,44,0.0199999995529652) 40%, rgba(44,44,44,0.0199999995529652) 60%, rgba(144,144,144,0.0199999995529652) 60%, rgba(144,144,144,0.0199999995529652) 80%, rgba(22,22,22,0.0199999995529652) 80%, rgba(22,22,22,0.0199999995529652) 100%),linear-gradient(74deg, rgba(211,211,211,0.0299999993294477) 0%, rgba(211,211,211,0.0299999993294477) 16.67%, rgba(91,91,91,0.0299999993294477) 16.67%, rgba(91,91,91,0.0299999993294477) 33.33%, rgba(14,14,14,0.0299999993294477) 33.33%, rgba(14,14,14,0.0299999993294477) 50%, rgba(239,239,239,0.0299999993294477) 50%, rgba(239,239,239,0.0299999993294477) 66.67%, rgba(250,250,250,0.0299999993294477) 66.67%, rgba(250,250,250,0.0299999993294477) 83.34%, rgba(146,146,146,0.0299999993294477) 83.34%, rgba(146,146,146,0.0299999993294477) 100%),linear-gradient(74deg, rgba(190,190,190,0.0299999993294477) 0%, rgba(190,190,190,0.0299999993294477) 14.29%, rgba(78,78,78,0.0299999993294477) 14.29%, rgba(78,78,78,0.0299999993294477) 28.57%, rgba(11,11,11,0.0299999993294477) 28.57%, rgba(11,11,11,0.0299999993294477) 42.86%, rgba(77,77,77,0.0299999993294477) 42.86%, rgba(77,77,77,0.0299999993294477) 57.14%, rgba(100,100,100,0.0299999993294477) 57.14%, rgba(100,100,100,0.0299999993294477) 71.43%, rgba(179,179,179,0.0299999993294477) 71.43%, rgba(179,179,179,0.0299999993294477) 85.72%, rgba(37,37,37,0.0299999993294477) 85.72%, rgba(37,37,37,0.0299999993294477) 100%),linear-gradient(72deg, rgba(159,159,159,0.0299999993294477) 0%, rgba(159,159,159,0.0299999993294477) 12.5%, rgba(231,231,231,0.0299999993294477) 12.5%, rgba(231,231,231,0.0299999993294477) 25%, rgba(26,26,26,0.0299999993294477) 25%, rgba(26,26,26,0.0299999993294477) 37.5%, rgba(121,121,121,0.0299999993294477) 37.5%, rgba(121,121,121,0.0299999993294477) 50%, rgba(144,144,144,0.0299999993294477) 50%, rgba(144,144,144,0.0299999993294477) 62.5%, rgba(211,211,211,0.0299999993294477) 62.5%, rgba(211,211,211,0.0299999993294477) 75%, rgba(234,234,234,0.0299999993294477) 75%, rgba(234,234,234,0.0299999993294477) 87.5%, rgba(224,224,224,0.0299999993294477) 87.5%, rgba(224,224,224,0.0299999993294477) 100%),linear-gradient(279deg, rgba(113,113,113,0.00999999977648258) 0%, rgba(113,113,113,0.00999999977648258) 12.5%, rgba(204,204,204,0.00999999977648258) 12.5%, rgba(204,204,204,0.00999999977648258) 25%, rgba(164,164,164,0.00999999977648258) 25%, rgba(164,164,164,0.00999999977648258) 37.5%, rgba(10,10,10,0.00999999977648258) 37.5%, rgba(10,10,10,0.00999999977648258) 50%, rgba(99,99,99,0.00999999977648258) 50%, rgba(99,99,99,0.00999999977648258) 62.5%, rgba(186,186,186,0.00999999977648258) 62.5%, rgba(186,186,186,0.00999999977648258) 75%, rgba(230,230,230,0.00999999977648258) 75%, rgba(230,230,230,0.00999999977648258) 87.5%, rgba(6,6,6,0.00999999977648258) 87.5%, rgba(6,6,6,0.00999999977648258) 100%),linear-gradient(11deg, rgba(241,241,241,0.0199999995529652) 0%, rgba(241,241,241,0.0199999995529652) 12.5%, rgba(51,51,51,0.0199999995529652) 12.5%, rgba(51,51,51,0.0199999995529652) 25%, rgba(101,101,101,0.0199999995529652) 25%, rgba(101,101,101,0.0199999995529652) 37.5%, rgba(107,107,107,0.0199999995529652) 37.5%, rgba(107,107,107,0.0199999995529652) 50%, rgba(197,197,197,0.0199999995529652) 50%, rgba(197,197,197,0.0199999995529652) 62.5%, rgba(226,226,226,0.0199999995529652) 62.5%, rgba(226,226,226,0.0199999995529652) 75%, rgba(100,100,100,0.0199999995529652) 75%, rgba(100,100,100,0.0199999995529652) 87.5%, rgba(216,216,216,0.0199999995529652) 87.5%, rgba(216,216,216,0.0199999995529652) 100%),linear-gradient(90deg, rgba(117,117,117,1) 0%, rgba(117,117,117,1) 100%)"),
                        ButtonFgColor = "#EBEDEF",

                        //Fonts
                        DefaultFontFamily = "Calibri",
                        DefaultFontColor = "#17202A",
                        BoxItemCurrencyFontSize = BoxCurrencyFontSize,
                        BoxItemHighlightNumberFontSize = BoxMainHighlightFontSize,

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
                    };
                default:
                    return null;
            }
        }

        private static double BoxCurrencyFontSize
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        return 18;
                    }
                    else { return 24; }
                }
                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        return 18;
                    }
                    else { return 24; }
                }
                return 24;
            }
        }

        private static double BoxMainHighlightFontSize
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        return 22;
                    }
                    else { return 30; }
                }
                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        return 22;
                    }
                    else { return 30; }
                }
                return 30;
            }
        }
    }
}
