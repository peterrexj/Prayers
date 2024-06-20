using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Microsoft.AppCenter.Crashes;
using Prayers.iOS.Services;
using UIKit;

namespace Prayers.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            var configService = new ConfigHelper();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(configService.SyncFusionLicense);

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            try
            {
                UIApplication.Main(args, null, typeof(AppDelegate));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
