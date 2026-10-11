namespace CatholicPrayers.Maui.Services;

public class FavouriteService : IFavouriteService
{
    private const string StorageKey = "Favourites";
    private readonly ILocalStorageService _storage;
    private HashSet<string>? _cache;

    public FavouriteService(ILocalStorageService storage)
    {
        _storage = storage;
    }

    public async Task<HashSet<string>> LoadAllAsync()
    {
        if (_cache != null) return _cache;
        var saved = await _storage.LoadAsync<List<string>>(StorageKey);
        _cache = saved != null ? [.. saved] : [];
        return _cache;
    }

    public async Task SetAsync(string prayerId, bool isFavourite)
    {
        var favs = await LoadAllAsync();
        if (isFavourite) favs.Add(prayerId);
        else favs.Remove(prayerId);
        await _storage.SaveAsync(StorageKey, favs.ToList());
    }

    public async Task<bool> IsFavouriteAsync(string prayerId)
    {
        var favs = await LoadAllAsync();
        return favs.Contains(prayerId);
    }
}
