namespace CatholicPrayers.Maui.Services;

public interface IFavouriteService
{
    Task<HashSet<string>> LoadAllAsync();
    Task SetAsync(string prayerId, bool isFavourite);
    Task<bool> IsFavouriteAsync(string prayerId);
}
