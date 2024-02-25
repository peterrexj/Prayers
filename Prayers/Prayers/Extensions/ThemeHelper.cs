using Pj.Library;
using Prayers.Models;
using Prayers.Services;
using Prayers.ViewModels.Extras;
using System;
using Xamarin.Forms;

namespace Prayers.Extensions
{
    public static class ThemeHelper
    {
        public static StyleModelDefault GetDefaultStyleTheme()
        {
            return GetDefaultStyleTheme(SettingsHelper.Model.SelectedAppTheme);
        }

        public static StyleModelDefault GetDefaultStyleTheme(AppThemes appThemes)
        {
            return DefaultStyleProvider.LoadDefaultStyle(appThemes);
        }

        public static void UpdateStatusBarBasedOnTheme(Color statusBarColor, bool isDarkTheme)
        {
            try
            {
                DependencyService.Get<IDeviceSpecificEnvironment>().SetStatusBarColor(statusBarColor, isDarkTheme);
            }
            catch (Exception ex)
            {
                ExceptionHandler.CaptureException(ex);
            }
        }

        public static void UpdateAppThemes()
        {
            UpdateAppThemes(GetDefaultStyleTheme());
        }
        public static void UpdateAppThemes(StyleModelDefault styleModel)
        {
            if (styleModel == null) return;
            UpdateStatusBarBasedOnTheme(styleModel.PageBgColorConverted, styleModel.AppTheme == AppThemes.Dark);

            ResourceDictionary appResources = Application.Current.Resources;
            if (appResources.ContainsKey("AppShellBgColor") && styleModel.AppShellBgColor.HasValue())
            {
                appResources["AppShellBgColor"] = styleModel.AppShellBgColor;
            }
            if (appResources.ContainsKey("AppShellFgColor") && styleModel.AppShellFgColor.HasValue())
            {
                appResources["AppShellFgColor"] = styleModel.AppShellFgColor;
            }
            if (appResources.ContainsKey("AppShellTitleColor") && styleModel.AppShellTitleColor.HasValue())
            {
                appResources["AppShellTitleColor"] = styleModel.AppShellTitleColor;
            }
            if (appResources.ContainsKey("AppShellDisabledColor") && styleModel.AppShellDisabledColor.HasValue())
            {
                appResources["AppShellDisabledColor"] = styleModel.AppShellDisabledColor;
            }
            if (appResources.ContainsKey("AppShellUnselectedColor") && styleModel.AppShellUnselectedColor.HasValue())
            {
                appResources["AppShellUnselectedColor"] = styleModel.AppShellUnselectedColor;
            }
            if (appResources.ContainsKey("AppShellTabBarBackgroundColor") && styleModel.AppShellTabBarBackgroundColor.HasValue())
            {
                appResources["AppShellTabBarBackgroundColor"] = styleModel.AppShellTabBarBackgroundColor;
            }
            if (appResources.ContainsKey("AppShellTabBarForegroundColor") && styleModel.AppShellTabBarForegroundColor.HasValue())
            {
                appResources["AppShellTabBarForegroundColor"] = styleModel.AppShellTabBarForegroundColor;
            }
            if (appResources.ContainsKey("AppShellTabBarUnselectedColor") && styleModel.AppShellTabBarUnselectedColor.HasValue())
            {
                appResources["AppShellTabBarUnselectedColor"] = styleModel.AppShellTabBarUnselectedColor;
            }
            if (appResources.ContainsKey("AppShellTabBarDisabledColor") && styleModel.AppShellTabBarDisabledColor.HasValue())
            {
                appResources["AppShellTabBarDisabledColor"] = styleModel.AppShellTabBarDisabledColor;
            }
            if (appResources.ContainsKey("AppShellTabBarTitleColor") && styleModel.AppShellTabBarTitleColor.HasValue())
            {
                appResources["AppShellTabBarTitleColor"] = styleModel.AppShellTabBarTitleColor;
            }
            if (appResources.ContainsKey("AppShellBackgroundColor") && styleModel.AppShellBackgroundColor.HasValue())
            {
                appResources["AppShellBackgroundColor"] = styleModel.AppShellBackgroundColor;
            }
        }
    }
}
