using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Prayers.Droid.Services
{
    internal static class ConfigHelper
    {
        public static ConcurrentDictionary<string, string> Configs { get; set; }
        public static string SyncFusionLicense => Configs?.FirstOrDefault(f => f.Key == "SyncFusionLicense").Value ?? "";
        public static void LoadConfig()
        {

            string content;
            using (var streamReader = new StreamReader(Application.Context.Assets.Open("CommonConfigs.json")))
            {
                content = streamReader.ReadToEnd();
            }
            Configs = Pj.Library.JsonHelper.ConvertComplexJsonDataToDictionary(content);
        }
    }
}