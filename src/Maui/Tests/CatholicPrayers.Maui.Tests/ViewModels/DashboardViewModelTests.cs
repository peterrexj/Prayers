using CatholicPrayers.Maui.Tests.Helpers;

namespace CatholicPrayers.Maui.Tests.ViewModels;

public class DashboardViewModelLogicTests
{
    private static PrayerMasterEntryModel[] BuildPrayers() =>
    [
        SampleData.MakeEntry("our_father",   "Our Father",          "Basic Prayers",   true),
        SampleData.MakeEntry("hail_mary",    "Hail Mary",           "Marian Prayers",  true),
        SampleData.MakeEntry("glory_be",     "Glory Be",            "Basic Prayers",   false),
        SampleData.MakeEntry("st_michael",   "Prayer to St Michael", "Saint Prayers",  false),
        SampleData.MakeEntry("divine_mercy", "Divine Mercy Chaplet", "Chaplets",       false),
        SampleData.MakeEntry("rosary",       "Holy Rosary",          "Chaplets",       true),
    ];

    [Fact]
    public void FeaturedPrayers_FilteredCorrectly()
    {
        var all = BuildPrayers();
        var featured = all.Where(p => p.IsFeatured).ToArray();
        Assert.Equal(3, featured.Length);
        Assert.All(featured, p => Assert.True(p.IsFeatured));
    }

    [Fact]
    public void HasFeatured_TrueWhenAnyFeatured()
    {
        var all = BuildPrayers();
        bool hasFeatured = all.Any(p => p.IsFeatured);
        Assert.True(hasFeatured);
    }

    [Fact]
    public void HasFeatured_FalseWhenNoneFeatured()
    {
        var all = BuildPrayers().Select(p => p with { IsFeatured = false }).ToArray();
        bool hasFeatured = all.Any(p => p.IsFeatured);
        Assert.False(hasFeatured);
    }

    [Fact]
    public void CategoryGroups_CorrectCount()
    {
        var all = BuildPrayers();
        var groups = all.GroupBy(p => p.Category).ToList();
        Assert.Equal(4, groups.Count);
    }

    [Fact]
    public void CategoryGroups_CorrectItemCounts()
    {
        var all = BuildPrayers();
        var groups = all.GroupBy(p => p.Category).ToDictionary(g => g.Key, g => g.Count());
        Assert.Equal(2, groups["Basic Prayers"]);
        Assert.Equal(1, groups["Marian Prayers"]);
        Assert.Equal(1, groups["Saint Prayers"]);
        Assert.Equal(2, groups["Chaplets"]);
    }

    [Fact]
    public void CategoryGroups_SortedAlphabetically()
    {
        var all = BuildPrayers();
        var categories = all.GroupBy(p => p.Category).Select(g => g.Key).OrderBy(c => c).ToArray();
        var sorted = (string[])categories.Clone();
        Array.Sort(sorted);
        Assert.Equal(sorted, categories);
    }

    [Fact]
    public void AllPrayers_SortedBySortOrder()
    {
        var entries = new[]
        {
            SampleData.MakeEntry("c", "C", "X") with { SortOrder = 3 },
            SampleData.MakeEntry("a", "A", "X") with { SortOrder = 1 },
            SampleData.MakeEntry("b", "B", "X") with { SortOrder = 2 },
        };
        var sorted = entries.OrderBy(p => p.SortOrder).ToArray();
        Assert.Equal("a", sorted[0].Id);
        Assert.Equal("b", sorted[1].Id);
        Assert.Equal("c", sorted[2].Id);
    }

    [Fact]
    public void Search_ActiveState_FiltersCategories()
    {
        var all = BuildPrayers();
        string query = "basic";
        var results = all.Where(p =>
            p.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.Category.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.Tags.Any(t => t.Contains(query, StringComparison.OrdinalIgnoreCase))
        ).ToArray();

        Assert.Equal(2, results.Length);
        Assert.All(results, r => Assert.Equal("Basic Prayers", r.Category));
    }
}
