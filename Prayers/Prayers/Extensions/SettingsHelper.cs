using Prayers.Services;
using Prayers.ViewModels.Extras;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prayers.Extensions
{
    public static class SettingsHelper
    {
        private static SettingsViewModel _model;
        public static SettingsViewModel Model
        {
            get
            {
                if (_model == null)
                {
                    Refresh();
                }
                return _model;
            }
        }

        public static void Refresh()
        {
            Task.Run(async () => _model = await SharedServices.LocalStorage.GetData<SettingsViewModel>()).Wait();

            if (_model == null)
            {
                _model = DefaultValue;
            }
            _model.DefaultStyle = ThemeHelper.GetDefaultStyleTheme(_model.SelectedAppTheme);
        }

        private static SettingsViewModel DefaultValue
        {
            get
            {
                return new SettingsViewModel
                {
                    SelectedTheme = OSAppTheme.Light.ToString(),
                };
            }
        }
    }
}
