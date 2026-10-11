using NUnit.Framework;
using Prayers.StJoseph.Maui.Models;
using Prayers.StJoseph.Maui.Services;
using Moq;

namespace Prayers.StJoseph.Maui.Tests;

[TestFixture]
public class SharedServicesParserTests
{
    [Test]
    public void PrayerAppModel_DefaultsAreEmpty()
    {
        var model = new PrayerAppModel();
        Assert.That(model.MainHeaders, Is.Not.Null);
        Assert.That(model.SinglePageDataModels, Is.Not.Null);
    }

    [Test]
    public void SinglePageDataModel_ContentDefaultsToEmpty()
    {
        var page = new SinglePageDataModel();
        Assert.That(page.Content, Is.Not.Null);
        Assert.That(page.Content, Is.Empty);
    }

    [Test]
    public void PageContentModel_DefaultFontSize_IsZero()
    {
        var m = new PageContentModel();
        Assert.That(m.FontSize, Is.EqualTo(0));
    }

    [Test]
    public void PrayerRawDataModel_Properties_RoundTrip()
    {
        var raw = new PrayerRawDataModel
        {
            SequenceId = 1,
            ContentType = "P",
            Content = "Hello",
            FontSize = 14.0
        };
        Assert.That(raw.SequenceId, Is.EqualTo(1));
        Assert.That(raw.ContentType, Is.EqualTo("P"));
        Assert.That(raw.Content, Is.EqualTo("Hello"));
        Assert.That(raw.FontSize, Is.EqualTo(14.0));
    }

    [Test]
    public void AppThemes_AllValuesExist()
    {
        var names = Enum.GetNames<AppThemes>();
        Assert.That(names, Does.Contain("Dark"));
        Assert.That(names, Does.Contain("Light"));
        Assert.That(names, Does.Contain("FireBreather"));
    }
}
