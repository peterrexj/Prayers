using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prayers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParaContentView : ContentView
    {
        public ParaContentView()
        {
            InitializeComponent();

            lblContent.SetBinding(Label.TextColorProperty, new Binding("TextColor", source: this));
            lblContent.SetBinding(Label.FontFamilyProperty, new Binding("FontFamily", source: this));
            lblContent.SetBinding(Label.FontSizeProperty, new Binding("FontSize", source: this));
        }

        #region Text Wrap
        public static readonly BindableProperty TextWrapProperty =
            BindableProperty.Create(
                propertyName: nameof(TextWrap), returnType: typeof(string),
                declaringType: typeof(string), defaultValue: default(string),
                propertyChanged: OnTextWrapPropertyPropertyChanged);

        private static void OnTextWrapPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ParaContentView)bindable;
            if (control != null)
            {
                if (newValue != null)
                {
                    if (newValue is string)
                    {
                        string value = (string)newValue;
                        if (value.IsEmpty() || (value.HasValue() && value == "Normal"))
                        {
                            control.lblContentNormal.IsVisible = true;
                            control.lblContent.IsVisible = false;
                        }
                        else if (value == "Justify")
                        {
                            control.lblContentNormal.IsVisible = false;
                            control.lblContent.IsVisible = true;
                        }
                    }
                }
            }
        }

        public string TextWrap
        {
            get { return (string)GetValue(TextWrapProperty); }
            set { SetValue(TextWrapProperty, value); }
        }

        #endregion

        #region Para Content
        public static readonly BindableProperty ParaContentProperty =
            BindableProperty.Create(
                propertyName: nameof(ParaContent), returnType: typeof(string),
                declaringType: typeof(string), defaultValue: default(string),
                propertyChanged: OnParaContentPropertyPropertyChanged);

        private static void OnParaContentPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ParaContentView)bindable;
            if (control != null)
            {
                if (newValue != null)
                {
                    if (newValue is string)
                    {
                        string value = (string)newValue;
                        control.lblContent.Text = value;
                        control.lblContentNormal.Text = value;
                    }
                }
            }
        }

        public string ParaContent
        {
            get { return (string)GetValue(ParaContentProperty); }
            set { SetValue(ParaContentProperty, value); }
        }

        #endregion

        #region Text Color
        public static readonly BindableProperty TextColorProperty =
         BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ParaContentView), Color.Default);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        #endregion

        #region Font Family

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ParaContentView), default(string));

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        #endregion

        #region Font Size

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ParaContentView), default(double));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        #endregion
    }
}