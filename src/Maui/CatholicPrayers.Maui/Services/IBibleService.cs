using CatholicPrayers.Maui.Models;

namespace CatholicPrayers.Maui.Services;

public interface IBibleService
{
    Task<List<BibleBook>> GetBooksAsync();
    Task<List<BibleVerse>> GetChapterAsync(string bookId, int chapter);
    Task<List<BibleSearchResult>> SearchAsync(string query, int limit = 50);
    Task<List<BibleCrossRef>> GetCrossRefsAsync(string bookId, int chapter, int verse, int limit = 8);
}
