using System.Text.Json;
using CatholicPrayers.Maui.Tests.Helpers;

namespace CatholicPrayers.Maui.Tests.Services;

/// <summary>
/// Tests for JSON deserialization and search logic.
/// PrayerService itself calls FileSystem.OpenAppPackageFileAsync (MAUI API),
/// so these tests validate the data transformation / search algorithms in isolation.
/// </summary>
public class PrayerMasterDeserializationTests
{
    [Fact]
    public void Deserialize_MasterJson_ReturnsAllPrayers()
    {
        var data = JsonSerializer.Deserialize<PrayerMasterDataModel>(SampleData.PrayerMasterJson);
        Assert.NotNull(data);
        Assert.Equal(4, data.Prayers.Length);
    }

    [Fact]
    public void Deserialize_MasterJson_PreservesIds()
    {
        var data = JsonSerializer.Deserialize<PrayerMasterDataModel>(SampleData.PrayerMasterJson)!;
        var ids = data.Prayers.Select(p => p.Id).ToArray();
        Assert.Contains("our_father", ids);
        Assert.Contains("hail_mary", ids);
        Assert.Contains("glory_be", ids);
        Assert.Contains("st_michael_prayer", ids);
    }

    [Fact]
    public void Deserialize_MasterJson_CorrectCategories()
    {
        var data = JsonSerializer.Deserialize<PrayerMasterDataModel>(SampleData.PrayerMasterJson)!;
        var categories = data.Prayers.Select(p => p.Category).Distinct().ToArray();
        Assert.Contains("Basic Prayers", categories);
        Assert.Contains("Marian Prayers", categories);
        Assert.Contains("Saint Prayers", categories);
    }

    [Fact]
    public void Deserialize_MasterJson_FeaturedFlags()
    {
        var data = JsonSerializer.Deserialize<PrayerMasterDataModel>(SampleData.PrayerMasterJson)!;
        var featured = data.Prayers.Where(p => p.IsFeatured).ToArray();
        Assert.Equal(2, featured.Length);
        Assert.Contains(featured, p => p.Id == "our_father");
        Assert.Contains(featured, p => p.Id == "hail_mary");
    }

    [Fact]
    public void Deserialize_MasterJson_TagsPreserved()
    {
        var data = JsonSerializer.Deserialize<PrayerMasterDataModel>(SampleData.PrayerMasterJson)!;
        var ourFather = data.Prayers.First(p => p.Id == "our_father");
        Assert.Contains("lord", ourFather.Tags);
        Assert.Contains("prayer", ourFather.Tags);
    }

    [Fact]
    public void Deserialize_MasterJson_RelatedPrayerIds()
    {
        var data = JsonSerializer.Deserialize<PrayerMasterDataModel>(SampleData.PrayerMasterJson)!;
        var ourFather = data.Prayers.First(p => p.Id == "our_father");
        Assert.Contains("hail_mary", ourFather.RelatedPrayerIds);
        Assert.Contains("glory_be", ourFather.RelatedPrayerIds);
    }

    [Fact]
    public void Deserialize_PrayerJson_SinglePage()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.OurFatherJson)!;
        Assert.Equal("SinglePage", data.Format);
        Assert.Single(data.Sections);
    }

    [Fact]
    public void Deserialize_PrayerJson_BlocksOrderedCorrectly()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.OurFatherJson)!;
        var blocks = data.Sections[0].Blocks.OrderBy(b => b.Order).ToArray();
        Assert.Equal("H1", blocks[0].Type);
        Assert.Equal("Paragraph", blocks[1].Type);
        Assert.Equal("Paragraph", blocks[2].Type);
    }

    [Fact]
    public void Deserialize_PrayerJson_MultiPage_SectionCount()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.DivineMercyJson)!;
        Assert.Equal("MultiPage", data.Format);
        Assert.Equal(3, data.Sections.Length);
    }

    [Fact]
    public void Deserialize_PrayerJson_BulletItem_IsResponseFlag()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.DivineMercyJson)!;
        var chaplet = data.Sections[1];
        var verse = chaplet.Blocks.First(b => b.BulletLabel == "V.");
        var response = chaplet.Blocks.First(b => b.BulletLabel == "R.");
        Assert.False(verse.IsResponse);
        Assert.True(response.IsResponse);
    }
}

public class PrayerSearchTests
{
    private static PrayerMasterEntryModel[] BuildEntries() =>
    [
        SampleData.MakeEntry("our_father",     "Our Father",          "Basic Prayers",   true,  ["lord", "prayer"]),
        SampleData.MakeEntry("hail_mary",      "Hail Mary",           "Marian Prayers",  true,  ["mary", "ave"]),
        SampleData.MakeEntry("glory_be",       "Glory Be",            "Basic Prayers",   false, ["doxology", "trinity"]),
        SampleData.MakeEntry("st_michael",     "Prayer to St. Michael", "Saint Prayers", false, ["michael", "protection"]),
        SampleData.MakeEntry("divine_mercy",   "Divine Mercy Chaplet", "Chaplets",       false, ["mercy", "chaplet"]),
    ];

    private static IEnumerable<PrayerMasterEntryModel> Search(
        IEnumerable<PrayerMasterEntryModel> entries, string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return entries;
        var q = query.Trim().ToLowerInvariant();
        return entries.Where(e =>
            e.Title.ToLowerInvariant().Contains(q) ||
            (e.Subtitle?.ToLowerInvariant().Contains(q) ?? false) ||
            e.Category.ToLowerInvariant().Contains(q) ||
            e.Tags.Any(t => t.ToLowerInvariant().Contains(q)));
    }

    [Fact]
    public void Search_ByTitle_CaseInsensitive()
    {
        var results = Search(BuildEntries(), "father").ToArray();
        Assert.Single(results);
        Assert.Equal("our_father", results[0].Id);
    }

    [Fact]
    public void Search_ByTitle_MixedCase()
    {
        var results = Search(BuildEntries(), "HAIL").ToArray();
        Assert.Single(results);
        Assert.Equal("hail_mary", results[0].Id);
    }

    [Fact]
    public void Search_ByCategory_ReturnsMultiple()
    {
        var results = Search(BuildEntries(), "basic").ToArray();
        Assert.Equal(2, results.Length);
        Assert.Contains(results, r => r.Id == "our_father");
        Assert.Contains(results, r => r.Id == "glory_be");
    }

    [Fact]
    public void Search_ByTag_ReturnsMatch()
    {
        var results = Search(BuildEntries(), "doxology").ToArray();
        Assert.Single(results);
        Assert.Equal("glory_be", results[0].Id);
    }

    [Fact]
    public void Search_EmptyQuery_ReturnsAll()
    {
        var all = BuildEntries();
        var results = Search(all, "").ToArray();
        Assert.Equal(5, results.Length);
    }

    [Fact]
    public void Search_WhitespaceQuery_ReturnsAll()
    {
        var all = BuildEntries();
        var results = Search(all, "   ").ToArray();
        Assert.Equal(5, results.Length);
    }

    [Fact]
    public void Search_NoMatch_ReturnsEmpty()
    {
        var results = Search(BuildEntries(), "zzznomatch").ToArray();
        Assert.Empty(results);
    }

    [Fact]
    public void Search_PartialWord_Matches()
    {
        var results = Search(BuildEntries(), "chap").ToArray();
        Assert.Single(results);
        Assert.Equal("divine_mercy", results[0].Id);
    }
}

public class CategoryGroupingTests
{
    [Fact]
    public void GroupByCategory_ProducesCorrectGroups()
    {
        var entries = new[]
        {
            SampleData.MakeEntry("a", "A", "Basic Prayers"),
            SampleData.MakeEntry("b", "B", "Basic Prayers"),
            SampleData.MakeEntry("c", "C", "Marian Prayers"),
            SampleData.MakeEntry("d", "D", "Saint Prayers"),
        };

        var groups = entries
            .GroupBy(e => e.Category)
            .Select(g => new { Category = g.Key, Items = g.ToList() })
            .ToList();

        Assert.Equal(3, groups.Count);
        Assert.Equal(2, groups.First(g => g.Category == "Basic Prayers").Items.Count);
        Assert.Single(groups.First(g => g.Category == "Marian Prayers").Items);
    }

    [Fact]
    public void GroupByCategory_SortOrderPreserved()
    {
        var entries = new[]
        {
            SampleData.MakeEntry("c", "C", "Zeta Cat") with { SortOrder = 3 },
            SampleData.MakeEntry("a", "A", "Alpha Cat") with { SortOrder = 1 },
            SampleData.MakeEntry("b", "B", "Alpha Cat") with { SortOrder = 2 },
        };

        var grouped = entries
            .OrderBy(e => e.SortOrder)
            .GroupBy(e => e.Category)
            .ToList();

        var alphaGroup = grouped.First(g => g.Key == "Alpha Cat").ToList();
        Assert.Equal("a", alphaGroup[0].Id);
        Assert.Equal("b", alphaGroup[1].Id);
    }
}
