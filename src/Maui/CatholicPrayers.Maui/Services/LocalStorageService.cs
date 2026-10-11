using System.Text.Json;

namespace CatholicPrayers.Maui.Services;

public class LocalStorageService : ILocalStorageService
{
    private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };
    private readonly string _basePath;

    public LocalStorageService()
    {
        _basePath = FileSystem.AppDataDirectory;
        Directory.CreateDirectory(_basePath);
    }

    public async Task<T?> LoadAsync<T>(string key) where T : class
    {
        try
        {
            var path = FilePath(key);
            if (!File.Exists(path)) return null;
            var json = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<T>(json, _opts);
        }
        catch { return null; }
    }

    public async Task SaveAsync<T>(string key, T data) where T : class
    {
        var json = JsonSerializer.Serialize(data, _opts);
        await File.WriteAllTextAsync(FilePath(key), json);
    }

    public Task DeleteAsync(string key)
    {
        var path = FilePath(key);
        if (File.Exists(path)) File.Delete(path);
        return Task.CompletedTask;
    }

    private string FilePath(string key) => Path.Combine(_basePath, $"{key}.json");
}
