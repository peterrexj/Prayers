namespace Prayers.Services;

public interface IAppInformation
{
    string PrayerEmbeddedDataFilePath { get; }

    string AppCentreAppKey { get; }
}
