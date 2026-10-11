using System.Text.Json;
using CatholicPrayers.Maui.Models;

namespace CatholicPrayers.Maui.Services;

public class PrayerService : IPrayerService
{
    private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };
    private PrayerMasterData? _cachedMaster;
    private Dictionary<string, PrayerMasterEntry>? _prayerLookup;
    private readonly Dictionary<string, PrayerData> _prayerCache = new();

    public async Task<PrayerMasterData> GetMasterDataAsync()
    {
        if (_cachedMaster != null) return _cachedMaster;

        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("Data/content_index.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            _cachedMaster = JsonSerializer.Deserialize<PrayerMasterData>(json, _opts)
                            ?? new PrayerMasterData();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[PrayerService] Failed to load content_index: {ex.Message}");
            _cachedMaster = new PrayerMasterData();
        }

        // Build fast lookup by id
        _prayerLookup = _cachedMaster.Prayers
            .ToDictionary(p => p.Id, p => p, StringComparer.OrdinalIgnoreCase);

        return _cachedMaster;
    }

    public async Task<PrayerData?> GetPrayerDataAsync(string dataFile)
    {
        var lang = SharedServices.AppSettings?.ContentLanguage ?? "en";
        var resolvedFile = lang == "en" ? dataFile : LocalizedPath(dataFile, lang);
        var cacheKey = resolvedFile;

        if (_prayerCache.TryGetValue(cacheKey, out var cached)) return cached;

        // Try locale file; fall back to English
        var filesToTry = resolvedFile == dataFile
            ? [dataFile]
            : new[] { resolvedFile, dataFile };

        foreach (var file in filesToTry)
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(file);
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                var data = JsonSerializer.Deserialize<PrayerData>(json, _opts);
                if (data != null)
                {
                    _prayerCache[cacheKey] = data;
                    return data;
                }
            }
            catch
            {
                // locale file missing — try next (English fallback)
            }
        }

        System.Diagnostics.Debug.WriteLine($"[PrayerService] Failed to load {dataFile}");
        return null;
    }

    // "Data/Prayers/our_father.json" + "la" → "Data/Prayers/our_father_la.json"
    private static string LocalizedPath(string dataFile, string lang)
    {
        var dir  = Path.GetDirectoryName(dataFile)?.Replace('\\', '/') ?? string.Empty;
        var name = Path.GetFileNameWithoutExtension(dataFile);
        var ext  = Path.GetExtension(dataFile);
        return string.IsNullOrEmpty(dir)
            ? $"{name}_{lang}{ext}"
            : $"{dir}/{name}_{lang}{ext}";
    }

    public async Task<IEnumerable<PrayerMasterEntry>> GetChildrenAsync(string sectionId)
    {
        var master = await GetMasterDataAsync();

        // Look in both Sections and Prayers for the parent
        var parent = master.Sections.FirstOrDefault(s => s.Id == sectionId)
                  ?? master.Prayers.FirstOrDefault(p => p.Id == sectionId);

        if (parent == null || parent.ChildIds.Length == 0)
            return [];

        var lookup = _prayerLookup ?? [];
        return parent.ChildIds
            .Select(id => lookup.TryGetValue(id, out var entry) ? entry : null)
            .Where(e => e != null)
            .Select(e => e!)
            .OrderBy(e => e.SortOrder);
    }

    public async Task<IEnumerable<PrayerMasterEntry>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return [];
        var master = await GetMasterDataAsync();
        var lower = query.ToLowerInvariant();
        return master.Prayers.Where(p =>
            p.Title.Contains(lower, StringComparison.OrdinalIgnoreCase) ||
            p.Subtitle.Contains(lower, StringComparison.OrdinalIgnoreCase) ||
            p.Category.Contains(lower, StringComparison.OrdinalIgnoreCase) ||
            p.Tags.Any(t => t.Contains(lower, StringComparison.OrdinalIgnoreCase)));
    }

    public IEnumerable<string> GetCategories(PrayerMasterData master)
        => master.Prayers.Select(p => p.Category).Distinct().OrderBy(c => c);

    public void ClearCache()
    {
        _cachedMaster = null;
        _prayerLookup = null;
        _prayerCache.Clear();
    }

    public void ClearPrayerContentCache() => _prayerCache.Clear();
}
