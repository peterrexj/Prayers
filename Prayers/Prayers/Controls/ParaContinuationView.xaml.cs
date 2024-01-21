﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prayers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParaContinuationView : ContentView
    {
        public ParaContinuationView()
        {
            InitializeComponent();

            lblContent.SetBinding(Label.TextColorProperty, new Binding("TextColor", source: this));
            lblContent.SetBinding(Label.FontFamilyProperty, new Binding("FontFamily", source: this));
            lblContent.SetBinding(Label.FontSizeProperty, new Binding("FontSize", source: this));
        }

        public static readonly BindableProperty ParaContentProperty =
            BindableProperty.Create(
                propertyName: nameof(ParaContent), returnType: typeof(string),
                declaringType: typeof(string), defaultValue: default(string),
                propertyChanged: OnParaContentPropertyPropertyChanged);

        private static void OnParaContentPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ParaContinuationView)bindable;
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

        public string ParaContent
        {
            get { return (string)GetValue(ParaContentProperty); }
            set { SetValue(ParaContentProperty, value); }
        }

        #region Text Color
        public static readonly BindableProperty TextColorProperty =
         BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ParaContinuationView), Color.Default);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        #endregion

        #region Font Family

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ParaContinuationView), default(string));

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        #endregion

        #region Font Size

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ParaContinuationView), default(double));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        #endregion
    }
}