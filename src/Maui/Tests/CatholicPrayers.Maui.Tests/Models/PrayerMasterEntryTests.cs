using System.Text.Json;
using CatholicPrayers.Maui.Tests.Helpers;

namespace CatholicPrayers.Maui.Tests.Models;

public class PrayerMasterEntryTests
{
    [Fact]
    public void Entry_DefaultAccentColor_IsValid()
    {
        var entry = SampleData.MakeEntry("test", "Test", "Basic");
        Assert.NotEmpty(entry.AccentColor);
        Assert.StartsWith("#", entry.AccentColor);
    }

    [Fact]
    public void Entry_DefaultIcon_IsNotEmpty()
    {
        var entry = SampleData.MakeEntry("test", "Test", "Basic");
        Assert.NotEmpty(entry.Icon);
    }

    [Fact]
    public void Entry_DefaultTags_IsEmptyArray()
    {
        var entry = SampleData.MakeEntry("test", "Test", "Basic");
        Assert.NotNull(entry.Tags);
        Assert.Empty(entry.Tags);
    }

    [Fact]
    public void Entry_DefaultRelatedIds_IsEmptyArray()
    {
        var entry = SampleData.MakeEntry("test", "Test", "Basic");
        Assert.NotNull(entry.RelatedPrayerIds);
        Assert.Empty(entry.RelatedPrayerIds);
    }

    [Fact]
    public void Entry_Deserialized_AllFieldsPopulated()
    {
        var data = JsonSerializer.Deserialize<PrayerMasterDataModel>(SampleData.PrayerMasterJson)!;
        var ourFather = data.Prayers.First(p => p.Id == "our_father");

        Assert.Equal("our_father", ourFather.Id);
        Assert.Equal("Our Father", ourFather.Title);
        Assert.Equal("The Lord's Prayer", ourFather.Subtitle);
        Assert.Equal("🙏", ourFather.Icon);
        Assert.Equal("Data/our_father.json", ourFather.DataFile);
        Assert.Equal("Basic Prayers", ourFather.Category);
        Assert.Equal(1, ourFather.SortOrder);
        Assert.True(ourFather.IsFeatured);
        Assert.Equal("#9D7DE8", ourFather.AccentColor);
    }
}

public class PrayerDataTests
{
    [Fact]
    public void PrayerData_DefaultFormat_IsSinglePage()
    {
        var data = new PrayerDataModel();
        Assert.Equal("SinglePage", data.Format);
    }

    [Fact]
    public void PrayerData_DefaultSections_IsEmptyArray()
    {
        var data = new PrayerDataModel();
        Assert.NotNull(data.Sections);
        Assert.Empty(data.Sections);
    }

    [Fact]
    public void PrayerData_SectionTitle_IsPresent()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.OurFatherJson)!;
        Assert.Equal("Our Father", data.Sections[0].Title);
    }

    [Fact]
    public void PrayerData_Blocks_AreNotEmpty()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.OurFatherJson)!;
        Assert.NotEmpty(data.Sections[0].Blocks);
    }

    [Fact]
    public void PrayerData_RelatedIds_FromJson()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.OurFatherJson)!;
        Assert.Contains("hail_mary", data.RelatedPrayerIds);
        Assert.Contains("glory_be", data.RelatedPrayerIds);
    }

    [Fact]
    public void PrayerData_MultiPage_SectionsAllHaveBlocks()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.DivineMercyJson)!;
        Assert.All(data.Sections, s => Assert.NotEmpty(s.Blocks));
    }
}
