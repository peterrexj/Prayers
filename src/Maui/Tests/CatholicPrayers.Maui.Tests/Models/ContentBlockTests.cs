using System.Text.Json;
using CatholicPrayers.Maui.Tests.Helpers;

namespace CatholicPrayers.Maui.Tests.Models;

public class ContentBlockTests
{
    [Fact]
    public void ContentBlock_DefaultType_IsParagraph()
    {
        var block = new ContentBlockModel();
        Assert.Equal("Paragraph", block.Type);
    }

    [Theory]
    [InlineData("H1")]
    [InlineData("H2")]
    [InlineData("H3")]
    [InlineData("Paragraph")]
    [InlineData("BulletItem")]
    [InlineData("NumberedItem")]
    [InlineData("Quote")]
    [InlineData("Image")]
    [InlineData("Divider")]
    [InlineData("Spacer")]
    [InlineData("ExternalLink")]
    [InlineData("Audio")]
    [InlineData("Rubric")]
    public void ContentBlock_DeserializesAllTypes(string type)
    {
        var json = $$"""{"Type":"{{type}}","Text":"test","Order":1}""";
        var block = JsonSerializer.Deserialize<ContentBlockModel>(json);
        Assert.NotNull(block);
        Assert.Equal(type, block.Type);
    }

    [Fact]
    public void ContentBlock_OrderPreserved()
    {
        var blocks = new[]
        {
            new ContentBlockModel { Type = "Paragraph", Text = "C", Order = 3 },
            new ContentBlockModel { Type = "H1",        Text = "A", Order = 1 },
            new ContentBlockModel { Type = "H2",        Text = "B", Order = 2 },
        };
        var sorted = blocks.OrderBy(b => b.Order).ToArray();
        Assert.Equal("A", sorted[0].Text);
        Assert.Equal("B", sorted[1].Text);
        Assert.Equal("C", sorted[2].Text);
    }

    [Fact]
    public void ContentBlock_IsResponse_DefaultFalse()
    {
        var block = new ContentBlockModel();
        Assert.False(block.IsResponse);
    }

    [Fact]
    public void ContentBlock_FontSizeOffset_DefaultZero()
    {
        var block = new ContentBlockModel();
        Assert.Equal(0.0, block.FontSizeOffset);
    }

    [Fact]
    public void ContentBlock_JsonRoundTrip()
    {
        var original = new ContentBlockModel
        {
            Type = "Quote",
            Text = "Test prayer text",
            BulletLabel = "V.",
            IsResponse = true,
            FontStyle = "BoldItalic",
            Alignment = "Center",
            FontSizeOffset = 2.5,
            Order = 5
        };

        var json = JsonSerializer.Serialize(original);
        var restored = JsonSerializer.Deserialize<ContentBlockModel>(json)!;

        Assert.Equal(original.Type, restored.Type);
        Assert.Equal(original.Text, restored.Text);
        Assert.Equal(original.BulletLabel, restored.BulletLabel);
        Assert.Equal(original.IsResponse, restored.IsResponse);
        Assert.Equal(original.FontStyle, restored.FontStyle);
        Assert.Equal(original.Alignment, restored.Alignment);
        Assert.Equal(original.FontSizeOffset, restored.FontSizeOffset);
        Assert.Equal(original.Order, restored.Order);
    }
}
