namespace CatholicPrayers.Maui.Services;

public interface ILocalStorageService
{
    Task<T?> LoadAsync<T>(string key) where T : class;
    Task SaveAsync<T>(string key, T data) where T : class;
    Task DeleteAsync(string key);
}
