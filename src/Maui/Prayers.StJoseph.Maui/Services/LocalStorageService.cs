using System.Text.Json;
using CsvHelper;
using Prayers.StJoseph.Maui.Models;

namespace Prayers.StJoseph.Maui.Services;

public class LocalStorageService : ILocalStorageService
{
    private static string DataFolder =>
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    private static string FilePathFor<T>() where T : class
    {
        return typeof(T).Name switch
        {
            "SettingsViewModel" => Path.Combine(DataFolder, "settingsdata.json"),
            _ => Path.Combine(DataFolder, "defaultdata.json"),
        };
    }

    public async Task<IEnumerable<PrayerRawDataModel>> ReadPrayerData(string filePath)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(filePath);
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
        return csv.GetRecords<PrayerRawDataModel>().ToList();
    }

    public async Task WriteTextAsync(string fileName, string text)
    {
        var path = Path.Combine(DataFolder, fileName);
        await File.WriteAllTextAsync(path, text);
    }

    public async Task<string?> ReadTextAsync(string fileName)
    {
        var path = Path.Combine(DataFolder, fileName);
        if (!File.Exists(path)) return null;
        return await File.ReadAllTextAsync(path);
    }

    public async Task<T?> GetData<T>() where T : class
    {
        var path = FilePathFor<T>();
        if (!File.Exists(path)) return default;
        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task SaveData<T>(T data) where T : class
    {
        var path = FilePathFor<T>();
        var json = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(path, json);
    }

    public Task ClearData<T>() where T : class
    {
        var path = FilePathFor<T>();
        if (File.Exists(path)) File.Delete(path);
        return Task.CompletedTask;
    }
}
