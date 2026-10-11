namespace Prayers.StJoseph.Maui.ViewModels;

public class BaseViewModel : BasePropertyChangeModel
{
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsFree));
        }
    }

    public bool IsFree => !IsBusy;

    private bool _isActive;
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            OnPropertyChanged(nameof(IsActive));
        }
    }
}
