using Android.Content;
using Prayers.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Prayers.Controls.LabelExtended), typeof(LabelExtendedRenderer))]
namespace Prayers.Droid.Renderer
{
    public class LabelExtendedRenderer : LabelRenderer
    {
        public LabelExtendedRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.JustificationMode = Android.Text.JustificationMode.InterWord;
            }
        }
    }
}