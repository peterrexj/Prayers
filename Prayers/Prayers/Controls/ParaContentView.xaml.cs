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
    public partial class ParaContentView : ContentView
    {
        public ParaContentView()
        {
            InitializeComponent();

            lblContent.SetBinding(Label.FontFamilyProperty, new Binding("FontFamily", source: this));
            lblContent.SetBinding(Label.FontSizeProperty, new Binding("FontSize", source: this));

            lblContentNormal.SetBinding(Label.FontFamilyProperty, new Binding("FontFamily", source: this));
            lblContentNormal.SetBinding(Label.FontSizeProperty, new Binding("FontSize", source: this));
        }

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

        #region Style
        public static readonly BindableProperty DefaultStyleProperty =
            BindableProperty.Create(
                propertyName: nameof(DefaultStyle), returnType: typeof(StyleModelDefault),
                declaringType: typeof(StyleModelDefault), defaultValue: default(StyleModelDefault),
                propertyChanged: OnDefaultStylePropertyPropertyChanged);

        private static void OnDefaultStylePropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ParaContentView)bindable;
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

        #region Font Align
        public static readonly BindableProperty FontAlignProperty =
            BindableProperty.Create(
                propertyName: nameof(FontAlign), returnType: typeof(string),
                declaringType: typeof(string), defaultValue: default(string),
                propertyChanged: OnFontAlignPropertyPropertyChanged);

        private static void OnFontAlignPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ParaContentView)bindable;
            if (control != null)
            {
                if (newValue != null)
                {
                    if (newValue is string)
                    {
                        string value = (string)newValue;
                        if (value.IsEmpty() || (value.HasValue() && value == "Right"))
                        {
                            control.lblContentNormal.HorizontalOptions = LayoutOptions.StartAndExpand;
                            control.lblContentNormal.HorizontalTextAlignment = TextAlignment.Start;

                            control.lblContent.HorizontalOptions = LayoutOptions.StartAndExpand;
                            control.lblContent.HorizontalTextAlignment = TextAlignment.Start;
                        }
                        else if (value == "Left")
                        {
                            control.lblContentNormal.HorizontalOptions = LayoutOptions.EndAndExpand;
                            control.lblContentNormal.HorizontalTextAlignment = TextAlignment.End;

                            control.lblContent.HorizontalOptions = LayoutOptions.EndAndExpand;
                            control.lblContent.HorizontalTextAlignment = TextAlignment.End;
                        }
                        else if (value == "Center")
                        {
                            control.lblContentNormal.HorizontalOptions = LayoutOptions.CenterAndExpand;
                            control.lblContentNormal.HorizontalTextAlignment = TextAlignment.Center;

                            control.lblContent.HorizontalOptions = LayoutOptions.CenterAndExpand;
                            control.lblContent.HorizontalTextAlignment = TextAlignment.Center;
                        }
                    }
                }
            }
        }

        public string FontAlign
        {
            get { return (string)GetValue(FontAlignProperty); }
            set { SetValue(FontAlignProperty, value); }
        }

        #endregion

        #region Font Custom Attributes
        public static readonly BindableProperty FontCustomAttributesProperty =
            BindableProperty.Create(
                propertyName: nameof(FontCustomAttributes), returnType: typeof(string[]),
                declaringType: typeof(string[]), defaultValue: default(string[]),
                propertyChanged: OnFontCustomAttributesPropertyPropertyChanged);

        private static void OnFontCustomAttributesPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ParaContentView)bindable;
            if (control != null)
            {
                if (newValue != null)
                {
                    if (newValue is string[])
                    {
                        string[] value = (string[])newValue;
                        if (value != null && value.Length > 0)
                        {
                            if (value.ContainsIgnoreCase("Bold") && value.ContainsIgnoreCase("Italics"))
                            {
                                control.lblContent.FontAttributes = FontAttributes.Bold | FontAttributes.Italic;
                                control.lblContentNormal.FontAttributes = FontAttributes.Bold | FontAttributes.Italic;
                            }
                            else if (value.ContainsIgnoreCase("Bold"))
                            {
                                control.lblContent.FontAttributes = FontAttributes.Bold;
                                control.lblContentNormal.FontAttributes = FontAttributes.Bold;
                            }
                            else if (value.ContainsIgnoreCase("Italics"))
                            {
                                control.lblContent.FontAttributes = FontAttributes.Italic;
                                control.lblContentNormal.FontAttributes = FontAttributes.Italic;
                            }
                        }
                    }
                }
            }
        }

        public string[] FontCustomAttributes
        {
            get { return (string[])GetValue(FontCustomAttributesProperty); }
            set { SetValue(FontCustomAttributesProperty, value); }
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