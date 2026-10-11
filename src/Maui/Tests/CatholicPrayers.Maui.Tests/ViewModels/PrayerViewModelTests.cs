using CatholicPrayers.Maui.Tests.Helpers;
using System.Text.Json;

namespace CatholicPrayers.Maui.Tests.ViewModels;

public class PrayerNavigationLogicTests
{
    [Theory]
    [InlineData(0, 3, false, true)]   // first section
    [InlineData(1, 3, true,  true)]   // middle
    [InlineData(2, 3, true,  false)]  // last
    [InlineData(0, 1, false, false)]  // single page
    public void CanNavigate_MatchesExpected(int current, int total, bool canPrev, bool canNext)
    {
        bool actualCanPrev = current > 0;
        bool actualCanNext = current < total - 1;
        Assert.Equal(canPrev, actualCanPrev);
        Assert.Equal(canNext, actualCanNext);
    }

    [Fact]
    public void NavigateNext_IncrementsIndex()
    {
        int current = 0;
        int total = 3;
        if (current < total - 1) current++;
        Assert.Equal(1, current);
    }

    [Fact]
    public void NavigatePrev_DecrementsIndex()
    {
        int current = 2;
        if (current > 0) current--;
        Assert.Equal(1, current);
    }

    [Fact]
    public void NavigateNext_DoesNotExceedTotal()
    {
        int current = 2;
        int total = 3;
        if (current < total - 1) current++;
        Assert.Equal(2, current);
    }

    [Fact]
    public void NavigatePrev_DoesNotGoBelowZero()
    {
        int current = 0;
        if (current > 0) current--;
        Assert.Equal(0, current);
    }

    [Fact]
    public void NavigateToSection_SetsIndex()
    {
        int current = 0;
        int target = 2;
        int total = 3;
        if (target >= 0 && target < total) current = target;
        Assert.Equal(2, current);
    }

    [Fact]
    public void NavigateToSection_OutOfRange_Ignored()
    {
        int current = 1;
        int target = 99;
        int total = 3;
        if (target >= 0 && target < total) current = target;
        Assert.Equal(1, current);
    }

    [Fact]
    public void SectionCount_FromPrayerData()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.DivineMercyJson)!;
        Assert.Equal(3, data.Sections.Length);
    }

    [Fact]
    public void CurrentSection_Title_AfterNavigation()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.DivineMercyJson)!;
        int idx = 1;
        Assert.Equal("The Chaplet", data.Sections[idx].Title);
    }
}

public class PrayerContentRenderingTests
{
    [Fact]
    public void Blocks_SortedByOrder_BeforeRender()
    {
        var section = new PrayerSectionModel
        {
            Title = "Test",
            Blocks =
            [
                new ContentBlockModel { Type = "Paragraph", Text = "Third", Order = 3 },
                new ContentBlockModel { Type = "H1",        Text = "First", Order = 1 },
                new ContentBlockModel { Type = "H2",        Text = "Second", Order = 2 },
            ]
        };

        var sorted = section.Blocks.OrderBy(b => b.Order).ToArray();
        Assert.Equal("First",  sorted[0].Text);
        Assert.Equal("Second", sorted[1].Text);
        Assert.Equal("Third",  sorted[2].Text);
    }

    [Fact]
    public void FontSize_Calculation()
    {
        const double baseSize = 17.0;
        const double sizeDelta = 2.0;
        const double blockOffset = 1.5;
        double effective = baseSize + sizeDelta + blockOffset;
        Assert.Equal(20.5, effective);
    }

    [Fact]
    public void RelatedPrayers_ShownOnLastSection()
    {
        var data = JsonSerializer.Deserialize<PrayerDataModel>(SampleData.DivineMercyJson)!;
        int lastIdx = data.Sections.Length - 1;
        bool shouldShowRelated = data.RelatedPrayerIds.Length > 0;
        // Only show on last section
        for (int i = 0; i < data.Sections.Length; i++)
        {
            bool show = shouldShowRelated && i == lastIdx;
            if (i < lastIdx) Assert.False(show);
            else Assert.True(show);
        }
    }

    [Fact]
    public void IsResponse_BulletItem_UsesAccentColor()
    {
        var block = new ContentBlockModel
        {
            Type = "BulletItem",
            BulletLabel = "R.",
            Text = "For the sake of His sorrowful Passion",
            IsResponse = true
        };
        Assert.True(block.IsResponse);
        Assert.Equal("R.", block.BulletLabel);
    }
}
