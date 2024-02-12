using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Image), typeof(Prayers.UWP.AspectImageRenderer))]

namespace Prayers.UWP
{
    public class AspectImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Image> e)
        {   
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                Control.Stretch = Windows.UI.Xaml.Media.Stretch.UniformToFill;
            }
        }
    }
}
