using CatholicPrayers.Maui.ViewModels;

namespace CatholicPrayers.Maui.Views;

[QueryProperty(nameof(EntryId), nameof(EntryId))]
public partial class LinkDetailPage : ContentPage
{
    private readonly LinkDetailViewModel _vm;
    private string? _entryId;

    public string? EntryId
    {
        get => _entryId;
        set { _entryId = value; }
    }

    public LinkDetailPage(LinkDetailViewModel vm)
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
            System.Diagnostics.Debug.WriteLine($"[LinkDetailPage.OnAppearing] {ex}");
        }
    }
}
