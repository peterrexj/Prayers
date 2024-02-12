﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Prayers.iOS.Services;
using UIKit;

namespace Prayers.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(ConfigHelper.SyncFusionLicense);

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}
