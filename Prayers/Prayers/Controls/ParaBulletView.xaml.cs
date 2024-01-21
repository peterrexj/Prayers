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
    public partial class ParaBulletView : ContentView
    {
        public ParaBulletView()
        {
            InitializeComponent();

            lblContentNormal.SetBinding(Label.TextColorProperty, new Binding("TextColor", source: this));
            lblContentNormal.SetBinding(Label.FontFamilyProperty, new Binding("FontFamily", source: this));
            lblContentNormal.SetBinding(Label.FontSizeProperty, new Binding("FontSize", source: this));

            lblContentJustified.SetBinding(Label.TextColorProperty, new Binding("TextColor", source: this));
            lblContentJustified.SetBinding(Label.FontFamilyProperty, new Binding("FontFamily", source: this));
            lblContentJustified.SetBinding(Label.FontSizeProperty, new Binding("FontSize", source: this));
        }

        #region Para Content
        public static readonly BindableProperty ParaContentProperty =
            BindableProperty.Create(
                propertyName: nameof(ParaContent), returnType: typeof(string),
                declaringType: typeof(string), defaultValue: default(string),
                propertyChanged: OnParaContentPropertyPropertyChanged);

        private static void OnParaContentPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ParaBulletView)bindable;
            if (control != null)
            {
                if (newValue != null)
                {
                    if (newValue is string)
                    {
                        string value = (string)newValue;
                        control.lblContentNormal.Text = value;
                        control.lblContentJustified.Text = value;
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

        #region Para Number

        public static readonly BindableProperty ParaNumberProperty =
           BindableProperty.Create(
               propertyName: nameof(ParaNumber), returnType: typeof(string),
               declaringType: typeof(string), defaultValue: default(string),
               propertyChanged: OnParaNumberPropertyPropertyChanged);

        private static void OnParaNumberPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ParaBulletView)bindable;
            if (control != null)
            {
                if (newValue != null)
                {
                    if (newValue is string value)
                    {
                        control.lblNumber.Text = value;
                    }
                }
            }
        }

        public string ParaNumber
        {
            get { return (string)GetValue(ParaNumberProperty); }
            set { SetValue(ParaNumberProperty, value); }
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
            var control = (ParaBulletView)bindable;
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
                            control.lblContentJustified.IsVisible = false;
                        }
                        else if (value == "Justify")
                        {
                            control.lblContentNormal.IsVisible = false;
                            control.lblContentJustified.IsVisible = true;
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
            var control = (ParaBulletView)bindable;
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

                            control.lblContentJustified.HorizontalOptions = LayoutOptions.StartAndExpand;
                            control.lblContentJustified.HorizontalTextAlignment = TextAlignment.Start;

                            control.lblNumber.HorizontalOptions = LayoutOptions.StartAndExpand;
                            control.lblNumber.HorizontalTextAlignment= TextAlignment.Start;
                        }
                        else if (value == "Left")
                        {
                            control.lblContentNormal.HorizontalOptions = LayoutOptions.EndAndExpand;
                            control.lblContentNormal.HorizontalTextAlignment = TextAlignment.End;

                            control.lblContentJustified.HorizontalOptions = LayoutOptions.EndAndExpand;
                            control.lblContentJustified.HorizontalTextAlignment = TextAlignment.End;

                            control.lblNumber.HorizontalOptions = LayoutOptions.EndAndExpand;
                            control.lblNumber.HorizontalTextAlignment = TextAlignment.End;
                        }
                        else if (value == "Center")
                        {
                            control.lblContentNormal.HorizontalOptions = LayoutOptions.CenterAndExpand;
                            control.lblContentNormal.HorizontalTextAlignment = TextAlignment.Center;

                            control.lblContentJustified.HorizontalOptions = LayoutOptions.CenterAndExpand;
                            control.lblContentJustified.HorizontalTextAlignment = TextAlignment.Center;

                            control.lblNumber.HorizontalOptions = LayoutOptions.CenterAndExpand;
                            control.lblNumber.HorizontalTextAlignment = TextAlignment.Center;
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
            var control = (ParaBulletView)bindable;
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
                                control.lblContentJustified.FontAttributes = FontAttributes.Bold | FontAttributes.Italic;
                                control.lblContentNormal.FontAttributes = FontAttributes.Bold | FontAttributes.Italic;
                                control.lblNumber.FontAttributes = FontAttributes.Bold | FontAttributes.Italic;
                            }
                            else if (value.ContainsIgnoreCase("Bold"))
                            {
                                control.lblContentJustified.FontAttributes = FontAttributes.Bold;
                                control.lblContentNormal.FontAttributes = FontAttributes.Bold;
                                control.lblNumber.FontAttributes = FontAttributes.Bold;
                            }
                            else if (value.ContainsIgnoreCase("Italics"))
                            {
                                control.lblContentJustified.FontAttributes = FontAttributes.Italic;
                                control.lblContentNormal.FontAttributes = FontAttributes.Italic;
                                control.lblNumber.FontAttributes = FontAttributes.Italic;
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

        #region Text Color
        public static readonly BindableProperty TextColorProperty =
         BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ParaBulletView), Color.Default);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        #endregion

        #region Font Family

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ParaBulletView), default(string));

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        #endregion

        #region Font Size

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ParaBulletView), default(double));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        #endregion
    }
}