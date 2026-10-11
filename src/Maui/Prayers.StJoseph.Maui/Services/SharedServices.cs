using Prayers.StJoseph.Maui.Models;

namespace Prayers.StJoseph.Maui.Services;

public static class SharedServices
{
    private static PrayerAppModel? _prayerData;
    private static Task? _loadTask;

    public static PrayerAppModel PrayerData => _prayerData ?? new PrayerAppModel();

    // Called once at startup to begin loading; safe to call multiple times.
    public static Task LoadPrayerDataAsync()
    {
        _loadTask ??= LoadInternalAsync();
        return _loadTask;
    }

    // Views call this to ensure data is ready before rendering.
    public static Task EnsureLoadedAsync() => _loadTask ?? LoadPrayerDataAsync();

    private static async Task LoadInternalAsync()
    {
        if (_prayerData != null) return;

        try
        {
            var storage = ServiceLocator.GetRequiredService<ILocalStorageService>();
            var appInfo = ServiceLocator.GetRequiredService<IAppInformationService>();
            var rawData = (await storage.ReadPrayerData(appInfo.PrayerEmbeddedDataFilePath)).ToList();

            _prayerData = new PrayerAppModel
            {
                MainHeaders = [],
                SinglePageDataModels = []
            };

            var grouped = rawData
                .GroupBy(f => f.HeaderSequenceId)
                .Select(g => new { g.Key, Items = g.OrderBy(x => x.SequenceId).ToList() })
                .ToList();

            foreach (var item in grouped
                .Where(g => g.Key == -1)
                .SelectMany(g => g.Items)
                .OrderBy(x => x.SequenceId))
            {
                if (item.ContentType == "MH")
                    _prayerData.MainHeaders.Add(item.Content?.Trim() ?? "");
                else if (item.ContentType == "MHT")
                    _prayerData.MainTitle = item.Content?.Trim() ?? "";
            }

            int currentPageId = 0;
            foreach (var group in grouped.Where(g => g.Key != -1))
            {
                currentPageId++;
                var pageData = new SinglePageDataModel
                {
                    PageId = currentPageId,
                    PreviousPageId = currentPageId == 1 ? null : currentPageId - 1,
                    Content = []
                };

                if (currentPageId > 1 && _prayerData.SinglePageDataModels.Count > 0)
                    _prayerData.SinglePageDataModels.Last().NextPageId = currentPageId;

                foreach (var item in group.Items)
                {
                    var content = new PageContentModel
                    {
                        ContentType = item.ContentType,
                        FontSize = item.FontSize ?? 12,
                        FontAlign = string.IsNullOrWhiteSpace(item.FontAlign) ? "Right" : item.FontAlign.Trim(),
                        FontAttribute = item.FontAttribute?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [],
                        TextWrap = string.IsNullOrWhiteSpace(item.TextWrap) ? "Normal" : item.TextWrap,
                        AdditionalData = item.AdditionalData,
                        IsHeader = item.ContentType == "H"
                    };

                    if (item.SpaceBefore.HasValue && item.SpaceBefore.Value > 0)
                    {
                        var text = item.Content?.Trim() ?? "";
                        content.Content = item.FontAlign?.Trim() == "Right"
                            ? text.PadLeft(text.Length + item.SpaceBefore.Value)
                            : text.PadRight(text.Length + item.SpaceBefore.Value);
                    }
                    else
                    {
                        content.Content = item.Content?.Trim();
                    }

                    pageData.Content.Add(content);
                }

                _prayerData.SinglePageDataModels.Add(pageData);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.Capture(ex, nameof(LoadInternalAsync));
        }
    }
}
