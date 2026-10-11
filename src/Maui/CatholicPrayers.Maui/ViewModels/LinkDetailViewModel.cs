using CatholicPrayers.Maui.Models;
using CatholicPrayers.Maui.Services;

namespace CatholicPrayers.Maui.ViewModels;

public class LinkDetailViewModel : BaseViewModel
{
    private readonly IPrayerService _prayerService;

    private string _title = string.Empty;
    private string _subtitle = string.Empty;
    private string _description = string.Empty;
    private string _url = string.Empty;
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

    public string Description
    {
        get => _description;
        private set => SetProperty(ref _description, value);
    }

    public string Url
    {
        get => _url;
        private set => SetProperty(ref _url, value);
    }

    public string AccentColor
    {
        get => _accentColor;
        private set => SetProperty(ref _accentColor, value);
    }

    public Command GoBackCommand { get; }
    public Command OpenLinkCommand { get; }

    public LinkDetailViewModel(IPrayerService prayerService)
    {
        _prayerService = prayerService;
        GoBackCommand  = new Command(async () => await Shell.Current.GoToAsync(".."));
        OpenLinkCommand = new Command(async () =>
        {
            if (!string.IsNullOrEmpty(_url))
                await Launcher.OpenAsync(new Uri(_url));
        });
    }

    public async Task LoadAsync(string entryId)
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var master = await _prayerService.GetMasterDataAsync();
            var entry = master.Prayers.FirstOrDefault(p => p.Id == entryId);
            if (entry == null) return;

            PageTitle    = entry.Title;
            PageSubtitle = entry.Subtitle;
            Description  = entry.ExternalAppDescription ?? string.Empty;
            Url          = entry.ExternalAppUrl ?? string.Empty;
            AccentColor  = entry.AccentColor;
        }
        finally { IsBusy = false; }
    }
}
