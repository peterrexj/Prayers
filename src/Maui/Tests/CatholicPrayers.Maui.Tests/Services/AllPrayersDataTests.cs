using System.Text.Json;
using CatholicPrayers.Maui.Tests.Helpers;

namespace CatholicPrayers.Maui.Tests.Services;

/// <summary>
/// Validates that all 23 expected Catholic prayers are present in the master data
/// with correct metadata.
/// </summary>
public class AllPrayersPresenceTests
{
    private static readonly string[] ExpectedIds =
    [
        "our_father", "hail_mary", "glory_be", "apostles_creed", "nicene_creed",
        "hail_holy_queen", "memorare", "act_of_contrition", "act_of_faith",
        "act_of_hope", "act_of_love", "divine_mercy_chaplet", "rosary",
        "angelus", "morning_offering", "guardian_angel_prayer",
        "st_michael_prayer", "come_holy_spirit", "prayer_to_st_joseph",
        "prayer_for_the_dead", "jesus_prayer", "grace_before_meals", "grace_after_meals"
    ];

    private static PrayerMasterDataModel LoadFullMaster()
    {
        // FindProjectRoot() returns the directory containing CatholicPrayers.Maui.sln
        var path = Path.Combine(
            FindProjectRoot(),
            "CatholicPrayers.Maui",
            "Resources", "Raw", "Data", "prayers_master.json");

        if (!File.Exists(path))
            throw new FileNotFoundException($"Master file not found at {path}");

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<PrayerMasterDataModel>(json)
               ?? throw new InvalidOperationException("Failed to deserialize prayers_master.json");
    }

    private static string FindProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            if (dir.GetFiles("*.sln").Length > 0 || dir.GetDirectories(".git").Length > 0)
                return dir.FullName;
            dir = dir.Parent;
        }
        return AppContext.BaseDirectory;
    }

    [Fact]
    public void PrayersMasterJson_Contains23Prayers()
    {
        var data = LoadFullMaster();
        Assert.Equal(23, data.Prayers.Length);
    }

    [Theory]
    [MemberData(nameof(GetExpectedIds))]
    public void Prayer_ExistsInMaster(string prayerId)
    {
        var data = LoadFullMaster();
        Assert.Contains(data.Prayers, p => p.Id == prayerId);
    }

    [Fact]
    public void AllPrayers_HaveNonEmptyTitle()
    {
        var data = LoadFullMaster();
        Assert.All(data.Prayers, p => Assert.False(string.IsNullOrWhiteSpace(p.Title)));
    }

    [Fact]
    public void AllPrayers_HaveNonEmptyDataFile()
    {
        var data = LoadFullMaster();
        Assert.All(data.Prayers, p => Assert.False(string.IsNullOrWhiteSpace(p.DataFile)));
    }

    [Fact]
    public void AllPrayers_DataFileEndsWithJson()
    {
        var data = LoadFullMaster();
        Assert.All(data.Prayers, p => Assert.EndsWith(".json", p.DataFile));
    }

    [Fact]
    public void AllPrayers_HaveValidAccentColor()
    {
        var data = LoadFullMaster();
        Assert.All(data.Prayers, p =>
        {
            Assert.NotEmpty(p.AccentColor);
            Assert.Matches(@"^#[0-9A-Fa-f]{6}$", p.AccentColor);
        });
    }

    [Fact]
    public void AllPrayers_HaveNonEmptyIcon()
    {
        var data = LoadFullMaster();
        Assert.All(data.Prayers, p => Assert.NotEmpty(p.Icon));
    }

    [Fact]
    public void AllPrayers_HaveNonEmptyCategory()
    {
        var data = LoadFullMaster();
        Assert.All(data.Prayers, p => Assert.NotEmpty(p.Category));
    }

    [Fact]
    public void AllPrayers_SortOrderIsPositive()
    {
        var data = LoadFullMaster();
        Assert.All(data.Prayers, p => Assert.True(p.SortOrder > 0));
    }

    [Fact]
    public void AllPrayers_SortOrderIsUnique()
    {
        var data = LoadFullMaster();
        var orders = data.Prayers.Select(p => p.SortOrder).ToArray();
        Assert.Equal(orders.Length, orders.Distinct().Count());
    }

    [Fact]
    public void AtLeastFourPrayers_AreFeatured()
    {
        var data = LoadFullMaster();
        var featuredCount = data.Prayers.Count(p => p.IsFeatured);
        Assert.True(featuredCount >= 4, $"Expected at least 4 featured prayers, got {featuredCount}");
    }

    [Fact]
    public void AllPrayers_DataFilesExistOnDisk()
    {
        var data = LoadFullMaster();
        var projectRoot = FindProjectRoot();
        var dataDir = Path.Combine(projectRoot, "CatholicPrayers.Maui", "Resources", "Raw");

        Assert.All(data.Prayers, p =>
        {
            var fullPath = Path.Combine(dataDir, p.DataFile.Replace('/', Path.DirectorySeparatorChar));
            Assert.True(File.Exists(fullPath), $"Data file missing: {fullPath}");
        });
    }

    [Fact]
    public void AllPrayerDataFiles_DeserializeWithoutError()
    {
        var data = LoadFullMaster();
        var projectRoot = FindProjectRoot();
        var dataDir = Path.Combine(projectRoot, "CatholicPrayers.Maui", "Resources", "Raw");

        foreach (var prayer in data.Prayers)
        {
            var fullPath = Path.Combine(dataDir, prayer.DataFile.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(fullPath)) continue;

            var json = File.ReadAllText(fullPath);
            var prayerData = JsonSerializer.Deserialize<PrayerDataModel>(json);
            Assert.NotNull(prayerData);
            Assert.NotEmpty(prayerData.Sections);
        }
    }

    [Fact]
    public void AllPrayerDataFiles_HaveAtLeastOneBlock()
    {
        var data = LoadFullMaster();
        var projectRoot = FindProjectRoot();
        var dataDir = Path.Combine(projectRoot, "CatholicPrayers.Maui", "Resources", "Raw");

        foreach (var prayer in data.Prayers)
        {
            var fullPath = Path.Combine(dataDir, prayer.DataFile.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(fullPath)) continue;

            var json = File.ReadAllText(fullPath);
            var prayerData = JsonSerializer.Deserialize<PrayerDataModel>(json)!;
            Assert.True(prayerData.Sections.Sum(s => s.Blocks.Length) > 0,
                $"Prayer {prayer.Id} has no content blocks");
        }
    }

    public static IEnumerable<object[]> GetExpectedIds() =>
        ExpectedIds.Select(id => new object[] { id });
}
