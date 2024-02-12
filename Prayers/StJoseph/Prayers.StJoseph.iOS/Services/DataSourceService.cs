using Prayers.iOS.Services;
using Prayers.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataSourceService))]
namespace Prayers.iOS.Services
{
    public class DataSourceService : IDataSource
    {
        public string PrayerEmbeddedDataFilePath => "St Joseph Prayer.csv";
    }
}