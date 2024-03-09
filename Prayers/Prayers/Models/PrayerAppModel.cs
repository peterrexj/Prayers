﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Prayers.Models
{
    public class PrayerAppModel
    {
        public PrayerAppModel() { }
        public List<string> MainHeaders { get; set; }
        public string MainTitle { get; set; }
        public string MainImage { get; set; }

        public List<SinglePageDataModel> SinglePageDataModels { get; set; }
    }
}
