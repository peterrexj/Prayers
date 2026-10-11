using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.Platforms.iOS;

public class AppInformationService : IAppInformationService
{
    public string AppVersion => AppInfo.VersionString;
    public string AppBuild => AppInfo.BuildString;
    public string DeviceModel => DeviceInfo.Model;
    public string OsVersion => DeviceInfo.VersionString;
    public string AppCentreKey => ""; // Set your AppCenter key here
    public string LocalStoragePath => FileSystem.AppDataDirectory;
}
