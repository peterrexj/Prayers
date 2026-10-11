namespace Prayers.StJoseph.Maui.Services;

public static class ExceptionHandler
{
    public static void Capture(Exception ex, params string[] context)
    {
        try
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[Exception] {ex.Message}");
            foreach (var c in context)
                System.Diagnostics.Debug.WriteLine($"  Context: {c}");
#else
            // AppCenter / Sentry crash reporting can be wired here
#endif
        }
        catch { /* swallow reporting errors */ }
    }
}
