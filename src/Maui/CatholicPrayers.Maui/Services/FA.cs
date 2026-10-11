namespace CatholicPrayers.Maui.Services;

// Font Awesome 5 Solid glyph codepoints used across the app.
// Use with FontFamily="FontAwesome5Solid".
public static class FA
{
    // ── UI chrome ────────────────────────────────────────────────────────────
    public const string Cross        = ""; // fa-cross
    public const string Home         = ""; // fa-home
    public const string Cog          = ""; // fa-cog  (settings gear)
    public const string Search       = ""; // fa-search
    public const string Times        = ""; // fa-times  (clear / close)
    public const string ChevronLeft  = ""; // fa-chevron-left
    public const string ChevronRight = ""; // fa-chevron-right
    public const string EllipsisV    = ""; // fa-ellipsis-v  (⋮ share/more)
    public const string FontLower    = ""; // fa-font  (A- proxy)
    public const string FontLarger   = ""; // same glyph, size set in code

    // ── Prayer tile icons ────────────────────────────────────────────────────
    public const string PrayingHands = ""; // fa-praying-hands
    public const string Dove         = ""; // fa-dove
    public const string Church       = ""; // fa-church
    public const string Star         = ""; // fa-star
    public const string Heart        = ""; // fa-heart
    public const string Fire         = ""; // fa-fire
    public const string BookOpen     = ""; // fa-book-open
    public const string Rosary       = ""; // fa-rosary
    public const string AngelWings   = ""; // fa-angel  (guardian angel)
    public const string Shield       = ""; // fa-shield-alt  (St Michael)
    public const string Sun          = ""; // fa-sun  (morning offering)
    public const string Moon         = ""; // fa-moon
    public const string Candle       = ""; // fa-candle-holder (penitential)
    public const string Crown        = ""; // fa-crown
    public const string Infinity     = ""; // fa-infinity
    public const string Utensils     = ""; // fa-utensils  (grace before meals)
    public const string LeafHeart    = ""; // fa-leaf  (after meals / thanksgiving)
    public const string HandsHelping = ""; // fa-hands-helping

    // ── Divine fly animation symbols ─────────────────────────────────────────
    public static readonly string[] FlySymbols =
    [
        Cross, Dove, Star, PrayingHands, Church, Heart, Crown, Rosary
    ];
}
