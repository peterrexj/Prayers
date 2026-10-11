using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Prayers.StJoseph.Maui.Models;
using Prayers.StJoseph.Maui.Services;

namespace Prayers.StJoseph.Maui.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private bool _isUpdating;

    public SettingsViewModel()
    {
        Themes = new ObservableCollection<string>(Enum.GetNames<AppThemes>());
    }

    private string? _selectedTheme;
    public string? SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            if (_isUpdating) return;
            _isUpdating = true;
            _selectedTheme = value;
            OnPropertyChanged(nameof(SelectedTheme));
            SaveData();
            _isUpdating = false;
        }
    }

    public AppThemes SelectedAppTheme =>
        Enum.TryParse<AppThemes>(_selectedTheme, out var t) ? t : AppThemes.Dark;

    [JsonIgnore]
    public ObservableCollection<string> Themes { get; }

    private double _currentFontSize;
    public double CurrentFontSize
    {
        get => _currentFontSize;
        set
        {
            if (_isUpdating) return;
            _isUpdating = true;
            _currentFontSize = value;
            OnPropertyChanged(nameof(CurrentFontSize));
            SaveData();
            _isUpdating = false;
        }
    }

    // Restore methods set backing fields without triggering SaveData (used on app startup)
    public void RestoreTheme(string theme)
    {
        _selectedTheme = theme;
        OnPropertyChanged(nameof(SelectedTheme));
        OnPropertyChanged(nameof(SelectedAppTheme));
    }

    public void RestoreFontSize(double size)
    {
        _currentFontSize = size;
        OnPropertyChanged(nameof(CurrentFontSize));
    }

    private void SaveData()
    {
        Task.Run(async () =>
        {
            var storage = ServiceLocator.GetService<ILocalStorageService>();
            if (storage != null)
                await storage.SaveData(this);
        });
    }
}
