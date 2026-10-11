namespace CatholicPrayers.Maui.Services;

public interface IAppInformationService
{
    string AppVersion { get; }
    string AppBuild { get; }
    string DeviceModel { get; }
    string OsVersion { get; }
    string AppCentreKey { get; }
    string LocalStoragePath { get; }
}
