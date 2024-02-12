using Prayers.Droid.Services;
using Prayers.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataSourceService))]
namespace Prayers.Droid.Services
{
    public class DataSourceService : IDataSource
    {
        public string PrayerEmbeddedDataFilePath => "St Joseph Prayer.csv";
    }
}