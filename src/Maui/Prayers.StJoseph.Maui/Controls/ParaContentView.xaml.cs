namespace Prayers.StJoseph.Maui.Controls;

public partial class ParaContentView : ContentView
{
    public ParaContentView()
    {
        InitializeComponent();
    }

    // ParaContent
    public static readonly BindableProperty ParaContentProperty =
        BindableProperty.Create(nameof(ParaContent), typeof(string), typeof(ParaContentView),
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaContentView)b;
                var v = (string?)nv ?? "";
                c.lblContentNormal.Text = v;
                c.lblContentJustify.Text = v;
            });

    public string? ParaContent
    {
        get => (string?)GetValue(ParaContentProperty);
        set => SetValue(ParaContentProperty, value);
    }

    // FontSize
    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ParaContentView), 14.0,
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaContentView)b;
                var v = (double)nv;
                c.lblContentNormal.FontSize = v;
                c.lblContentJustify.FontSize = v;
            });

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    // FontFamily
    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ParaContentView),
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaContentView)b;
                var v = (string?)nv;
                if (v != null) { c.lblContentNormal.FontFamily = v; c.lblContentJustify.FontFamily = v; }
            });

    public string? FontFamily
    {
        get => (string?)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    // TextWrap
    public static readonly BindableProperty TextWrapProperty =
        BindableProperty.Create(nameof(TextWrap), typeof(string), typeof(ParaContentView),
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaContentView)b;
                var justify = (string?)nv == "Justify";
                c.lblContentNormal.IsVisible = !justify;
                c.lblContentJustify.IsVisible = justify;
            });

    public string? TextWrap
    {
        get => (string?)GetValue(TextWrapProperty);
        set => SetValue(TextWrapProperty, value);
    }

    // FontAlign
    public static readonly BindableProperty FontAlignProperty =
        BindableProperty.Create(nameof(FontAlign), typeof(string), typeof(ParaContentView),
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaContentView)b;
                var (lo, ta) = AlignFromString((string?)nv);
                c.lblContentNormal.HorizontalOptions = lo;
                c.lblContentNormal.HorizontalTextAlignment = ta;
                c.lblContentJustify.HorizontalOptions = lo;
                c.lblContentJustify.HorizontalTextAlignment = ta;
            });

    public string? FontAlign
    {
        get => (string?)GetValue(FontAlignProperty);
        set => SetValue(FontAlignProperty, value);
    }

    // FontCustomAttributes
    public static readonly BindableProperty FontCustomAttributesProperty =
        BindableProperty.Create(nameof(FontCustomAttributes), typeof(string[]), typeof(ParaContentView),
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaContentView)b;
                var fa = AttributesFromArray((string[]?)nv);
                c.lblContentNormal.FontAttributes = fa;
                c.lblContentJustify.FontAttributes = fa;
            });

    public string[]? FontCustomAttributes
    {
        get => (string[]?)GetValue(FontCustomAttributesProperty);
        set => SetValue(FontCustomAttributesProperty, value);
    }

    // TextColor (driven by theme)
    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ParaContentView), Colors.Gray,
            propertyChanged: (b, _, nv) =>
            {
                var c = (ParaContentView)b;
                var col = (Color)nv;
                c.lblContentNormal.TextColor = col;
                c.lblContentJustify.TextColor = col;
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
