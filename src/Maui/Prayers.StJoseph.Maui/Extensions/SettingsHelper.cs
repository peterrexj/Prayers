using Prayers.StJoseph.Maui.Models;
using Prayers.StJoseph.Maui.Services;
using Prayers.StJoseph.Maui.ViewModels;

namespace Prayers.StJoseph.Maui.Extensions;

public static class SettingsHelper
{
    private static SettingsViewModel? _model;

    public static SettingsViewModel Model
    {
        get
        {
            if (_model == null) Refresh();
            return _model ??= DefaultValue;
        }
    }

    public static void Refresh()
    {
        Task.Run(async () =>
        {
            var storage = ServiceLocator.GetService<ILocalStorageService>();
            if (storage != null)
                _model = await storage.GetData<SettingsViewModel>();
        }).Wait();
        _model ??= DefaultValue;
    }

    private static SettingsViewModel DefaultValue => new()
    {
        SelectedTheme = nameof(AppThemes.Dark),
        CurrentFontSize = 0
    };
}
