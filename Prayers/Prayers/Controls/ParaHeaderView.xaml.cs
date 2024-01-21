using Pj.Library;
using Prayers.ViewModels.Extras;
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
    public partial class ParaHeaderView : ContentView
    {
        public ParaHeaderView()
        {
            InitializeComponent();

            lblContent.SetBinding(Label.TextColorProperty, new Binding("TextColor", source: this));
            lblContent.SetBinding(Label.FontFamilyProperty, new Binding("FontFamily", source: this));
            lblContent.SetBinding(Label.FontSizeProperty, new Binding("FontSize", source: this));
        }

        public static readonly BindableProperty ParaHeaderContentProperty =
            BindableProperty.Create(
                propertyName: nameof(ParaHeaderContent), returnType: typeof(string),
                declaringType: typeof(string), defaultValue: default(string),
                propertyChanged: OnParaHeaderContentPropertyPropertyChanged);

        private static void OnParaHeaderContentPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ParaHeaderView)bindable;
            if (control != null)
            {
                if (newValue != null)
                {
                    if (newValue is string)
                    {
                        string value = (string)newValue;
                        control.lblContent.Text = value;
                    }
                }
            }
        }

        public string ParaHeaderContent
        {
            get { return (string)GetValue(ParaHeaderContentProperty); }
            set { SetValue(ParaHeaderContentProperty, value); }
        }

        #region Style
        public static readonly BindableProperty DefaultStyleProperty =
            BindableProperty.Create(
                propertyName: nameof(DefaultStyle), returnType: typeof(StyleModelDefault),
                declaringType: typeof(StyleModelDefault), defaultValue: default(StyleModelDefault),
                propertyChanged: OnFontCustomAttributesPropertyPropertyChanged);

        private static void OnFontCustomAttributesPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ParaHeaderView)bindable;
            if (control != null)
            {
                
            }
        }

        public StyleModelDefault DefaultStyle
        {
            get { return (StyleModelDefault)GetValue(DefaultStyleProperty); }
            set { SetValue(DefaultStyleProperty, value); }
        }

        #endregion

        #region Text Color
        public static readonly BindableProperty TextColorProperty =
         BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ParaHeaderView), Color.Default);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        #endregion

        #region Font Family

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ParaHeaderView), default(string));

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        #endregion

        #region Font Size

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ParaHeaderView), default(double));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        #endregion
    }
}