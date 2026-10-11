using CatholicPrayers.Maui.ViewModels;

namespace CatholicPrayers.Maui.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _vm;

    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try { await _vm.RestoreAsync(); }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[SettingsPage.OnAppearing] {ex}"); }
    }

    private async void OnBackClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}
