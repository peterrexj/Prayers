using Prayers.StJoseph.Maui.Services;
using Prayers.StJoseph.Maui.ViewModels;

namespace Prayers.StJoseph.Maui.Views;

public partial class MainView : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainView()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await SharedServices.EnsureLoadedAsync();

            var data = SharedServices.PrayerData;
            _viewModel.PrayerMainTitle = data.MainTitle;
            if (data.MainHeaders.Count > 0)
                _viewModel.PrayerMainHeader01 = data.MainHeaders[0];
            if (data.MainHeaders.Count > 1)
                _viewModel.PrayerMainHeader02 = data.MainHeaders[1];

            var theme = ServiceLocator.GetService<IThemeService>();
            if (theme != null)
                _viewModel.DefaultStyle = theme.GetDefaultStyleTheme();
        }
        catch (Exception ex) { ExceptionHandler.Capture(ex, nameof(OnAppearing)); }
    }
}
