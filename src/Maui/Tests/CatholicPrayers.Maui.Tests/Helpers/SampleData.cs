namespace CatholicPrayers.Maui.Tests.Helpers;

public static class SampleData
{
    public static string PrayerMasterJson => """
    {
      "Prayers": [
        {
          "Id": "our_father",
          "Title": "Our Father",
          "Subtitle": "The Lord's Prayer",
          "Icon": "🙏",
          "DataFile": "Data/our_father.json",
          "Category": "Basic Prayers",
          "SortOrder": 1,
          "IsFeatured": true,
          "AccentColor": "#9D7DE8",
          "Tags": ["lord", "prayer", "jesus", "basic"],
          "RelatedPrayerIds": ["hail_mary", "glory_be"]
        },
        {
          "Id": "hail_mary",
          "Title": "Hail Mary",
          "Subtitle": "Ave Maria",
          "Icon": "🌹",
          "DataFile": "Data/hail_mary.json",
          "Category": "Marian Prayers",
          "SortOrder": 2,
          "IsFeatured": true,
          "AccentColor": "#7BC8F6",
          "Tags": ["mary", "ave", "rosary"],
          "RelatedPrayerIds": ["our_father", "rosary"]
        },
        {
          "Id": "glory_be",
          "Title": "Glory Be",
          "Subtitle": "Doxology",
          "Icon": "✨",
          "DataFile": "Data/glory_be.json",
          "Category": "Basic Prayers",
          "SortOrder": 3,
          "IsFeatured": false,
          "AccentColor": "#F6C547",
          "Tags": ["trinity", "doxology", "basic"],
          "RelatedPrayerIds": ["our_father"]
        },
        {
          "Id": "st_michael_prayer",
          "Title": "Prayer to St. Michael",
          "Subtitle": "Defender Against Evil",
          "Icon": "⚔️",
          "DataFile": "Data/st_michael_prayer.json",
          "Category": "Saint Prayers",
          "SortOrder": 10,
          "IsFeatured": false,
          "AccentColor": "#5B9BD5",
          "Tags": ["michael", "archangel", "protection"],
          "RelatedPrayerIds": []
        }
      ]
    }
    """;

    public static string OurFatherJson => """
    {
      "Format": "SinglePage",
      "Sections": [
        {
          "Title": "Our Father",
          "Blocks": [
            { "Type": "H1", "Text": "Our Father", "Order": 1 },
            {
              "Type": "Paragraph",
              "Text": "Our Father, who art in heaven, hallowed be Thy name.",
              "Order": 2
            },
            {
              "Type": "Paragraph",
              "Text": "Thy kingdom come, Thy will be done on earth as it is in heaven.",
              "Order": 3
            }
          ]
        }
      ],
      "RelatedPrayerIds": ["hail_mary", "glory_be"]
    }
    """;

    public static string DivineMercyJson => """
    {
      "Format": "MultiPage",
      "Sections": [
        {
          "Title": "Opening Prayers",
          "Blocks": [
            { "Type": "H1", "Text": "Divine Mercy Chaplet", "Order": 1 },
            { "Type": "Paragraph", "Text": "Begin with the Apostles' Creed...", "Order": 2 }
          ]
        },
        {
          "Title": "The Chaplet",
          "Blocks": [
            { "Type": "H2", "Text": "On each Our Father bead:", "Order": 1 },
            {
              "Type": "BulletItem",
              "BulletLabel": "V.",
              "Text": "Eternal Father, I offer you the Body and Blood...",
              "IsResponse": false,
              "Order": 2
            },
            {
              "Type": "BulletItem",
              "BulletLabel": "R.",
              "Text": "For the sake of His sorrowful Passion...",
              "IsResponse": true,
              "Order": 3
            }
          ]
        },
        {
          "Title": "Closing",
          "Blocks": [
            { "Type": "H2", "Text": "Closing Prayer", "Order": 1 },
            { "Type": "Paragraph", "Text": "Holy God, Holy Mighty One...", "Order": 2 }
          ]
        }
      ],
      "RelatedPrayerIds": ["rosary"]
    }
    """;

    public static PrayerMasterEntryModel MakeEntry(
        string id, string title, string category,
        bool featured = false, string[]? tags = null) =>
        new()
        {
            Id = id,
            Title = title,
            Category = category,
            IsFeatured = featured,
            Tags = tags ?? [],
            DataFile = $"Data/{id}.json",
            Icon = "✝",
            AccentColor = "#9D7DE8"
        };
}
