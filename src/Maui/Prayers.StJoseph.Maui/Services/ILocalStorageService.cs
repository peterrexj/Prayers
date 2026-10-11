using Prayers.StJoseph.Maui.Models;

namespace Prayers.StJoseph.Maui.Services;

public interface ILocalStorageService
{
    Task<IEnumerable<PrayerRawDataModel>> ReadPrayerData(string filePath);
    Task WriteTextAsync(string fileName, string text);
    Task<string?> ReadTextAsync(string fileName);
    Task<T?> GetData<T>() where T : class;
    Task SaveData<T>(T data) where T : class;
    Task ClearData<T>() where T : class;
}
