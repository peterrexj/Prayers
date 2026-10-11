namespace CatholicPrayers.Maui.Models;

public class BibleBook
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public string Testament { get; set; } = string.Empty; // "OT" | "NT"
    public int ChapterCount { get; set; }
    public string Category { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsFavourite { get; set; }
}

public class BibleIndex
{
    public string Canon { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
    public List<BibleBook> Books { get; set; } = [];
}

public class BibleChapter
{
    public int ChapterNumber { get; set; }
    public List<BibleVerse> Verses { get; set; } = [];
}

public class BibleVerse
{
    public int VerseNumber { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class BibleBookData
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public string Testament { get; set; } = string.Empty;
    public List<BibleChapter> Chapters { get; set; } = [];
}

public class BibleCrossRef
{
    public string ToBook { get; set; } = string.Empty;
    public string ToBookTitle { get; set; } = string.Empty;
    public string ToAbbreviation { get; set; } = string.Empty;
    public int ToChapter { get; set; }
    public int ToVerseStart { get; set; }
    public int ToVerseEnd { get; set; }
    public int Votes { get; set; }

    public string Reference => ToVerseEnd > ToVerseStart
        ? $"{ToBookTitle} {ToChapter}:{ToVerseStart}–{ToVerseEnd}"
        : $"{ToBookTitle} {ToChapter}:{ToVerseStart}";

    public string ShortReference => ToVerseEnd > ToVerseStart
        ? $"{ToAbbreviation} {ToChapter}:{ToVerseStart}–{ToVerseEnd}"
        : $"{ToAbbreviation} {ToChapter}:{ToVerseStart}";
}

public class BibleSearchResult
{
    public string BookId { get; set; } = string.Empty;
    public string BookTitle { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public int Chapter { get; set; }
    public int Verse { get; set; }
    public string Text { get; set; } = string.Empty;

    public string Reference => $"{BookTitle} {Chapter}:{Verse}";
    public string ShortReference => $"{Abbreviation} {Chapter}:{Verse}";
}
