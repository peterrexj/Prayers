namespace CatholicPrayers.Maui.Models;

/// <summary>
/// Resolves a novena's 9-day window from its StartDate string.
/// Handles fixed calendar dates ("March 10"), moveable feasts ("Good Friday",
/// "Ascension Day", "9 days before"), and returns null when the date cannot
/// be determined for the current year.
/// </summary>
public static class NovenaDateHelper
{
    private static readonly Dictionary<string, int> MonthNames = new(StringComparer.OrdinalIgnoreCase)
    {
        ["January"] = 1, ["February"] = 2, ["March"] = 3, ["April"] = 4,
        ["May"] = 5, ["June"] = 6, ["July"] = 7, ["August"] = 8,
        ["September"] = 9, ["October"] = 10, ["November"] = 11, ["December"] = 12
    };

    // Returns (start, end) inclusive for today's calendar year, or null.
    public static (DateTime Start, DateTime End)? GetWindow(PrayerMasterEntry entry)
    {
        var year = DateTime.Today.Year;
        var start = ResolveStart(entry.StartDate, year);
        if (start == null) return null;
        return (start.Value, start.Value.AddDays(8));
    }

    // Returns true if today falls within the novena window.
    public static bool IsActiveToday(PrayerMasterEntry entry)
    {
        var w = GetWindow(entry);
        if (w == null) return false;
        var today = DateTime.Today;
        return today >= w.Value.Start && today <= w.Value.End;
    }

    private static DateTime? ResolveStart(string? raw, int year)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        // "Good Friday" — 2 days before Easter
        if (raw.Equals("Good Friday", StringComparison.OrdinalIgnoreCase))
            return Easter(year).AddDays(-2);

        // "Ascension Day" — 39 days after Easter
        if (raw.Equals("Ascension Day", StringComparison.OrdinalIgnoreCase))
            return Easter(year).AddDays(39);

        // "9 days before" — used for Sacred Heart (feast = Friday after Corpus Christi = 68 days after Easter)
        // and Corpus Christi (feast = 60 days after Easter)
        // We use the entry title to disambiguate
        if (raw.Equals("9 days before", StringComparison.OrdinalIgnoreCase))
            return null; // moveable — skip active detection; handled as "calendar unknown"

        // "Month Day" e.g. "May 4", "March 10"
        var parts = raw.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 2
            && MonthNames.TryGetValue(parts[0], out var month)
            && int.TryParse(parts[1], out var day))
        {
            try { return new DateTime(year, month, day); }
            catch { return null; }
        }

        return null;
    }

    // Computus (Anonymous Gregorian algorithm)
    public static DateTime Easter(int year)
    {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31;
        int day   = ((h + l - 7 * m + 114) % 31) + 1;
        return new DateTime(year, month, day);
    }
}
