using System.Text.Json.Serialization;
using System.Windows.Input;
using Prayers.StJoseph.Maui.Services;

namespace Prayers.StJoseph.Maui.ViewModels;

public class MainViewModel : BaseViewModel
{
    public ICommand BtnStartCommand { get; }

    public MainViewModel()
    {
        BtnStartCommand = new Command(async () => await ProcessRequestToPrayerPage());
    }

    private string? _prayerMainHeader01;
    public string? PrayerMainHeader01
    {
        get => _prayerMainHeader01;
        set { _prayerMainHeader01 = value; OnPropertyChanged(nameof(PrayerMainHeader01)); }
    }

    private string? _prayerMainHeader02;
    public string? PrayerMainHeader02
    {
        get => _prayerMainHeader02;
        set { _prayerMainHeader02 = value; OnPropertyChanged(nameof(PrayerMainHeader02)); }
    }

    private string? _prayerMainTitle;
    public string? PrayerMainTitle
    {
        get => _prayerMainTitle;
        set { _prayerMainTitle = value; OnPropertyChanged(nameof(PrayerMainTitle)); }
    }

    public string PathToMainImage => "mainpic.jpg";

    [JsonIgnore]
    private StyleModelDefault? _defaultStyle;
    [JsonIgnore]
    public StyleModelDefault? DefaultStyle
    {
        get => _defaultStyle;
        set { _defaultStyle = value; OnPropertyChanged(nameof(DefaultStyle)); }
    }

    private async Task ProcessRequestToPrayerPage()
    {
        await Shell.Current.GoToAsync("Page1?PageId=1");
    }
}
