using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.ViewModels;

public class PartnerAppViewModel : BaseViewModel
{
    private readonly IPrayerService _prayerService;

    private string _title = string.Empty;
    private string _subtitle = string.Empty;
    private string _iconGlyph = string.Empty;
    private string _appIconImage = string.Empty;
    private string _appImage = string.Empty;
    private string _shortDescription = string.Empty;
    private string _fullDescription = string.Empty;
    private string _storeUrl = string.Empty;
    private string _accentColor = "#4A90D9";

    public string PageTitle
    {
        get => _title;
        private set => SetProperty(ref _title, value);
    }

    public string PageSubtitle
    {
        get => _subtitle;
        private set => SetProperty(ref _subtitle, value);
    }

    public string IconGlyph
    {
        get => _iconGlyph;
        private set => SetProperty(ref _iconGlyph, value);
    }

    public string AppIconImage
    {
        get => _appIconImage;
        private set => SetProperty(ref _appIconImage, value);
    }

    public string AppImage
    {
        get => _appImage;
        private set => SetProperty(ref _appImage, value);
    }

    public string ShortDescription
    {
        get => _shortDescription;
        private set => SetProperty(ref _shortDescription, value);
    }

    public string FullDescription
    {
        get => _fullDescription;
        private set => SetProperty(ref _fullDescription, value);
    }

    public string StoreUrl
    {
        get => _storeUrl;
        private set => SetProperty(ref _storeUrl, value);
    }

    public string AccentColor
    {
        get => _accentColor;
        private set => SetProperty(ref _accentColor, value);
    }

    public Command GoBackCommand { get; }
    public Command OpenStoreCommand { get; }

    public PartnerAppViewModel(IPrayerService prayerService)
    {
        _prayerService = prayerService;
        GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        OpenStoreCommand = new Command(async () =>
        {
            if (!string.IsNullOrEmpty(_storeUrl))
                await Launcher.OpenAsync(new Uri(_storeUrl));
        });
    }

    public async Task LoadAsync(string entryId)
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var master = await _prayerService.GetMasterDataAsync();

            // Search both Sections and Prayers arrays
            var entry = (master.Sections?.FirstOrDefault(s => s.Id == entryId))
                     ?? master.Prayers.FirstOrDefault(p => p.Id == entryId);

            if (entry == null) return;

            PageTitle        = entry.Title;
            PageSubtitle     = entry.Subtitle;
            IconGlyph        = entry.PartnerAppIconGlyph ?? entry.Icon;
            AppIconImage     = entry.PartnerAppIconImage ?? string.Empty;
            AppImage         = entry.PartnerAppImage ?? string.Empty;
            ShortDescription = entry.PartnerAppShortDescription ?? string.Empty;
            FullDescription  = entry.PartnerAppFullDescription ?? string.Empty;
            AccentColor      = entry.AccentColor;

            StoreUrl = DeviceInfo.Platform == DevicePlatform.iOS
                ? entry.PartnerAppUrlIos ?? string.Empty
                : entry.PartnerAppUrlAndroid ?? string.Empty;
        }
        finally { IsBusy = false; }
    }
}
