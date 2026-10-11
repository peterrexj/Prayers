namespace CatholicPrayers.Maui.ViewModels;

public class BaseViewModel : BasePropertyChangeModel
{
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            SetProperty(ref _isBusy, value);
            OnPropertyChanged(nameof(IsFree));
        }
    }
    public bool IsFree => !_isBusy;
}
