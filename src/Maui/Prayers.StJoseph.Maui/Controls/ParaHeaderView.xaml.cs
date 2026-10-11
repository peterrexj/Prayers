namespace Prayers.StJoseph.Maui.Controls;

public partial class ParaHeaderView : ContentView
{
    public ParaHeaderView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ParaHeaderContentProperty =
        BindableProperty.Create(nameof(ParaHeaderContent), typeof(string), typeof(ParaHeaderView),
            propertyChanged: (b, _, nv) => ((ParaHeaderView)b).lblContent.Text = (string?)nv ?? "");

    public string? ParaHeaderContent
    {
        get => (string?)GetValue(ParaHeaderContentProperty);
        set => SetValue(ParaHeaderContentProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ParaHeaderView), 14.0,
            propertyChanged: (b, _, nv) => ((ParaHeaderView)b).lblContent.FontSize = (double)nv);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ParaHeaderView),
            propertyChanged: (b, _, nv) => { if (nv is string v) ((ParaHeaderView)b).lblContent.FontFamily = v; });

    public string? FontFamily
    {
        get => (string?)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ParaHeaderView), Colors.Gray,
            propertyChanged: (b, _, nv) => ((ParaHeaderView)b).lblContent.TextColor = (Color)nv);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ParaHeaderView), Colors.Gray,
            propertyChanged: (b, _, nv) =>
            {
                var view = (ParaHeaderView)b;
                if (view.Content is Border border) border.Stroke = new SolidColorBrush((Color)nv);
            });

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public static readonly BindableProperty BackgroundGradientProperty =
        BindableProperty.Create(nameof(BackgroundGradient), typeof(string), typeof(ParaHeaderView),
            propertyChanged: (b, _, nv) =>
            {
                // No-op: solid background used in MAUI
            });

    public string? BackgroundGradient
    {
        get => (string?)GetValue(BackgroundGradientProperty);
        set => SetValue(BackgroundGradientProperty, value);
    }

    public static readonly BindableProperty BoxBackgroundColorProperty =
        BindableProperty.Create(nameof(BoxBackgroundColor), typeof(Color), typeof(ParaHeaderView), Colors.Transparent,
            propertyChanged: (b, _, nv) =>
            {
                var view = (ParaHeaderView)b;
                if (view.Content is Border border) border.BackgroundColor = (Color)nv;
            });

    public Color BoxBackgroundColor
    {
        get => (Color)GetValue(BoxBackgroundColorProperty);
        set => SetValue(BoxBackgroundColorProperty, value);
    }
}
