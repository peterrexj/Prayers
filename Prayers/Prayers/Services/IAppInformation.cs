using System;
using System.Collections.Generic;
using System.Text;

namespace Prayers.Services
{
    public interface IAppInformation
    {
        string PrayerEmbeddedDataFilePath { get; }

        string AppCentreAppKey { get; }
    }
}
