namespace Prayers.StJoseph.Maui.Controls;

public partial class ParaBulletView : ContentView
{
    public ParaBulletView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ParaContentProperty =
        BindableProperty.Create(nameof(ParaContent), typeof(string), typeof(ParaBulletView),
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaBulletView)b; var v = (string?)nv ?? "";
                c.lblContentNormal.Text = v; c.lblContentJustify.Text = v;
            });

    public string? ParaContent
    {
        get => (string?)GetValue(ParaContentProperty);
        set => SetValue(ParaContentProperty, value);
    }

    public static readonly BindableProperty ParaNumberProperty =
        BindableProperty.Create(nameof(ParaNumber), typeof(string), typeof(ParaBulletView),
            propertyChanged: (b, _, nv) => ((ParaBulletView)b).lblNumber.Text = (string?)nv ?? "");

    public string? ParaNumber
    {
        get => (string?)GetValue(ParaNumberProperty);
        set => SetValue(ParaNumberProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ParaBulletView), 14.0,
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaBulletView)b; var v = (double)nv;
                c.lblNumber.FontSize = v; c.lblContentNormal.FontSize = v; c.lblContentJustify.FontSize = v;
            });

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ParaBulletView),
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaBulletView)b; var v = (string?)nv;
                if (v != null) { c.lblNumber.FontFamily = v; c.lblContentNormal.FontFamily = v; c.lblContentJustify.FontFamily = v; }
            });

    public string? FontFamily
    {
        get => (string?)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty TextWrapProperty =
        BindableProperty.Create(nameof(TextWrap), typeof(string), typeof(ParaBulletView),
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaBulletView)b; bool j = (string?)nv == "Justify";
                c.lblContentNormal.IsVisible = !j; c.lblContentJustify.IsVisible = j;
            });

    public string? TextWrap
    {
        get => (string?)GetValue(TextWrapProperty);
        set => SetValue(TextWrapProperty, value);
    }

    public static readonly BindableProperty FontAlignProperty =
        BindableProperty.Create(nameof(FontAlign), typeof(string), typeof(ParaBulletView),
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaBulletView)b;
                var (lo, ta) = AlignFromString((string?)nv);
                c.lblNumber.HorizontalOptions = lo; c.lblNumber.HorizontalTextAlignment = ta;
                c.lblContentNormal.HorizontalOptions = lo; c.lblContentNormal.HorizontalTextAlignment = ta;
                c.lblContentJustify.HorizontalOptions = lo; c.lblContentJustify.HorizontalTextAlignment = ta;
            });

    public string? FontAlign
    {
        get => (string?)GetValue(FontAlignProperty);
        set => SetValue(FontAlignProperty, value);
    }

    public static readonly BindableProperty FontCustomAttributesProperty =
        BindableProperty.Create(nameof(FontCustomAttributes), typeof(string[]), typeof(ParaBulletView),
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaBulletView)b;
                var fa = AttributesFromArray((string[]?)nv);
                c.lblNumber.FontAttributes = fa; c.lblContentNormal.FontAttributes = fa; c.lblContentJustify.FontAttributes = fa;
            });

    public string[]? FontCustomAttributes
    {
        get => (string[]?)GetValue(FontCustomAttributesProperty);
        set => SetValue(FontCustomAttributesProperty, value);
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ParaBulletView), Colors.Gray,
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaBulletView)b; var col = (Color)nv;
                c.lblNumber.TextColor = col; c.lblContentNormal.TextColor = col; c.lblContentJustify.TextColor = col;
            });

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    private static (LayoutOptions lo, TextAlignment ta) AlignFromString(string? align) =>
        align switch
        {
            "Left" => (LayoutOptions.End, TextAlignment.End),
            "Center" => (LayoutOptions.Center, TextAlignment.Center),
            _ => (LayoutOptions.Start, TextAlignment.Start),
        };

    private static FontAttributes AttributesFromArray(string[]? attrs)
    {
        if (attrs == null || attrs.Length == 0) return FontAttributes.None;
        bool bold = attrs.Any(a => string.Equals(a, "Bold", StringComparison.OrdinalIgnoreCase));
        bool italic = attrs.Any(a => string.Equals(a, "Italics", StringComparison.OrdinalIgnoreCase)
                                   || string.Equals(a, "Italic", StringComparison.OrdinalIgnoreCase));
        return (bold, italic) switch
        {
            (true, true) => FontAttributes.Bold | FontAttributes.Italic,
            (true, false) => FontAttributes.Bold,
            (false, true) => FontAttributes.Italic,
            _ => FontAttributes.None,
        };
    }
}
