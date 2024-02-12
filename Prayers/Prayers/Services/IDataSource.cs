using System;
using System.Collections.Generic;
using System.Text;

namespace Prayers.Services
{
    public interface IDataSource
    {
        string PrayerEmbeddedDataFilePath { get; }
    }
}
