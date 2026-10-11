using CatholicPrayers.Maui.Models;

namespace CatholicPrayers.Maui.Services;

public interface IPrayerService
{
    Task<PrayerMasterData> GetMasterDataAsync();
    Task<PrayerData?> GetPrayerDataAsync(string dataFile);
    Task<IEnumerable<PrayerMasterEntry>> SearchAsync(string query);
    Task<IEnumerable<PrayerMasterEntry>> GetChildrenAsync(string sectionId);
    IEnumerable<string> GetCategories(PrayerMasterData master);
    void ClearCache();
    void ClearPrayerContentCache();
}
