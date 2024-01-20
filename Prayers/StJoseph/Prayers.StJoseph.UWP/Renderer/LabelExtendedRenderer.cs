using Prayers.UWP.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly:ExportRenderer(typeof(Prayers.Controls.LabelExtended), typeof(LabelExtendedRenderer))]
namespace Prayers.UWP.Renderer
{
    public class LabelExtendedRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.TextAlignment = Windows.UI.Xaml.TextAlignment.Justify;
            }
        }
    }
}
