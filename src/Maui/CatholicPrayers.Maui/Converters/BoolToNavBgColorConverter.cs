using System.Globalization;

namespace CatholicPrayers.Maui.Converters;

public class BoolToNavBgColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool enabled = value is bool b && b;
        var key = enabled ? "PrayAppNavButtonBgColor" : "PrayAppNavButtonDisabledBgColor";
        if (Application.Current?.Resources?.TryGetValue(key, out var c) == true)
            return c;
        return enabled ? Colors.Purple : Colors.Gray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
