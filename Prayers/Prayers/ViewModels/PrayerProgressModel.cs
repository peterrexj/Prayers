using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prayers.ViewModels
{
    public class PrayerProgressModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public StepStatus Status { get; set; }
        public int ProgressValue { get; set; }
    }
}
