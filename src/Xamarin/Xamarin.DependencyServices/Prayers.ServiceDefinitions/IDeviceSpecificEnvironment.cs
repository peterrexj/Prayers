using System.Drawing;

namespace Prayers.Services;

public interface IDeviceSpecificEnvironment
{
    void SetStatusBarColor(Color color, bool darkStatusBarTint);
}
