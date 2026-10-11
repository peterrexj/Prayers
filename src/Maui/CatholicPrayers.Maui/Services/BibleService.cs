using Microsoft.Data.Sqlite;
using CatholicPrayers.Maui.Models;

namespace CatholicPrayers.Maui.Services;

public class BibleService : IBibleService
{
    private string? _dbPath;
    private List<BibleBook>? _bookCache;

    private async Task<string> GetDbPathAsync()
    {
        if (_dbPath != null) return _dbPath;

        var dest = Path.Combine(FileSystem.AppDataDirectory, "bible.db");

        // Always re-copy if bundled DB is larger than cached (schema was updated)
        bool needsCopy = !File.Exists(dest);
        if (!needsCopy)
        {
            using var probe = await FileSystem.OpenAppPackageFileAsync("Data/Bible/bible.db");
            needsCopy = probe.Length > new FileInfo(dest).Length;
        }

        if (needsCopy)
        {
            using var src = await FileSystem.OpenAppPackageFileAsync("Data/Bible/bible.db");
            using var dst = File.Create(dest);
            await src.CopyToAsync(dst);
        }

        _dbPath = dest;
        return dest;
    }

    private async Task<SqliteConnection> OpenAsync()
    {
        var path = await GetDbPathAsync();
        var con = new SqliteConnection($"Data Source={path}");
        con.Open();
        return con;
    }

    public async Task<List<BibleBook>> GetBooksAsync()
    {
        if (_bookCache != null) return _bookCache;

        using var con = await OpenAsync();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT id, title, abbrev, testament, category, chapters, sort_order FROM books ORDER BY sort_order";

        using var reader = cmd.ExecuteReader();
        var list = new List<BibleBook>();
        while (reader.Read())
        {
            list.Add(new BibleBook
            {
                Id           = reader.GetString(0),
                Title        = reader.GetString(1),
                Abbreviation = reader.GetString(2),
                Testament    = reader.GetString(3),
                Category     = reader.GetString(4),
                ChapterCount = reader.GetInt32(5),
                SortOrder    = reader.GetInt32(6)
            });
        }

        _bookCache = list;
        return list;
    }

    public async Task<List<BibleVerse>> GetChapterAsync(string bookId, int chapter)
    {
        using var con = await OpenAsync();
        using var cmd = con.CreateCommand();
        cmd.CommandText = """
            SELECT v.verse, v.text
            FROM verses v
            JOIN books b ON b.title = v.book_id
            WHERE b.id = $b AND v.chapter = $c
            ORDER BY v.verse
            """;
        cmd.Parameters.AddWithValue("$b", bookId);
        cmd.Parameters.AddWithValue("$c", chapter);

        using var reader = cmd.ExecuteReader();
        var list = new List<BibleVerse>();
        while (reader.Read())
            list.Add(new BibleVerse { VerseNumber = reader.GetInt32(0), Text = reader.GetString(1) });
        return list;
    }

    public async Task<List<BibleCrossRef>> GetCrossRefsAsync(string bookId, int chapter, int verse, int limit = 8)
    {
        var books = await GetBooksAsync();
        var bookMap = books.ToDictionary(b => b.Title);

        // Look up the title for this bookId so we can query cross_references by title
        var fromBook = books.FirstOrDefault(b => b.Id == bookId);
        if (fromBook == null) return [];

        using var con = await OpenAsync();
        using var cmd = con.CreateCommand();
        cmd.CommandText = """
            SELECT to_book, to_chapter, to_verse_start, to_verse_end, votes
            FROM cross_references
            WHERE from_book = $b AND from_chapter = $c AND from_verse = $v
            ORDER BY votes DESC
            LIMIT $lim
            """;
        cmd.Parameters.AddWithValue("$b", fromBook.Title);
        cmd.Parameters.AddWithValue("$c", chapter);
        cmd.Parameters.AddWithValue("$v", verse);
        cmd.Parameters.AddWithValue("$lim", limit);

        using var reader = cmd.ExecuteReader();
        var list = new List<BibleCrossRef>();
        while (reader.Read())
        {
            var toTitle = reader.GetString(0);
            bookMap.TryGetValue(toTitle, out var toBook);
            list.Add(new BibleCrossRef
            {
                ToBook         = toBook?.Id ?? toTitle,
                ToBookTitle    = toBook?.Title ?? toTitle,
                ToAbbreviation = toBook?.Abbreviation ?? toTitle,
                ToChapter      = reader.GetInt32(1),
                ToVerseStart   = reader.GetInt32(2),
                ToVerseEnd     = reader.GetInt32(3),
                Votes          = reader.GetInt32(4)
            });
        }
        return list;
    }

    public async Task<List<BibleSearchResult>> SearchAsync(string query, int limit = 50)
    {
        if (string.IsNullOrWhiteSpace(query)) return [];

        var books = await GetBooksAsync();
        var bookMap = books.ToDictionary(b => b.Id);

        using var con = await OpenAsync();

        // Try FTS5 first
        try
        {
            var ftsQuery = query.Trim().Contains(' ')
                ? $"\"{query.Trim()}\""
                : query.Trim() + "*";

            using var cmd = con.CreateCommand();
            cmd.CommandText = """
                SELECT b.id, v.chapter, v.verse, v.text
                FROM verses_fts f
                JOIN verses v ON f.rowid = v.id
                JOIN books b ON b.title = v.book_id
                WHERE verses_fts MATCH $q
                ORDER BY rank
                LIMIT $lim
                """;
            cmd.Parameters.AddWithValue("$q", ftsQuery);
            cmd.Parameters.AddWithValue("$lim", limit);

            using var reader = cmd.ExecuteReader();
            var list = new List<BibleSearchResult>();
            while (reader.Read())
            {
                var bookId = reader.GetString(0);
                bookMap.TryGetValue(bookId, out var book);
                list.Add(new BibleSearchResult
                {
                    BookId       = bookId,
                    BookTitle    = book?.Title ?? bookId,
                    Abbreviation = book?.Abbreviation ?? bookId,
                    Chapter      = reader.GetInt32(1),
                    Verse        = reader.GetInt32(2),
                    Text         = reader.GetString(3)
                });
            }
            return list;
        }
        catch
        {
            // FTS fallback: LIKE search
            using var cmd = con.CreateCommand();
            cmd.CommandText = """
                SELECT b.id, v.chapter, v.verse, v.text
                FROM verses v
                JOIN books b ON b.title = v.book_id
                WHERE v.text LIKE $q
                LIMIT $lim
                """;
            cmd.Parameters.AddWithValue("$q", $"%{query}%");
            cmd.Parameters.AddWithValue("$lim", limit);

            using var reader = cmd.ExecuteReader();
            var list = new List<BibleSearchResult>();
            while (reader.Read())
            {
                var bookId = reader.GetString(0);
                bookMap.TryGetValue(bookId, out var book);
                list.Add(new BibleSearchResult
                {
                    BookId       = bookId,
                    BookTitle    = book?.Title ?? bookId,
                    Abbreviation = book?.Abbreviation ?? bookId,
                    Chapter      = reader.GetInt32(1),
                    Verse        = reader.GetInt32(2),
                    Text         = reader.GetString(3)
                });
            }
            return list;
        }
    }
}
