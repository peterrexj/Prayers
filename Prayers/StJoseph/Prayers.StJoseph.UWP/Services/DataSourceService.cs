using Prayers.Services;
using Prayers.UWP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataSourceService))]
namespace Prayers.UWP.Services
{
    public class DataSourceService : IDataSource
    {
        public string PrayerEmbeddedDataFilePath => "St Joseph Prayer.csv";
    }
}
