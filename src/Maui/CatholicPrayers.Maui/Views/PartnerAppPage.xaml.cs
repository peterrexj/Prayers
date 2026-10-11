using CatholicPrayers.Maui.ViewModels;

namespace CatholicPrayers.Maui.Views;

[QueryProperty(nameof(EntryId), nameof(EntryId))]
public partial class PartnerAppPage : ContentPage
{
    private readonly PartnerAppViewModel _vm;
    private string? _entryId;

    public string? EntryId
    {
        get => _entryId;
        set { _entryId = value; }
    }

    public PartnerAppPage(PartnerAppViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (!string.IsNullOrEmpty(_entryId))
                await _vm.LoadAsync(_entryId);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[PartnerAppPage.OnAppearing] {ex}");
        }
    }
}
