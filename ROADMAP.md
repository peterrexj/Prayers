# Catholic Prayers App — Full Roadmap

## App Identity
- **Package:** `com.pj.catholicprayersapp`
- **Platform:** .NET 9 MAUI — `net9.0-ios18.0`, `net9.0-android36.0`, `net9.0-maccatalyst`
- **Project root:** `src/Maui/CatholicPrayers.Maui/`
- **Architecture:** Manual MVVM (no CommunityToolkit generators), Shell navigation, DI via `MauiProgram.cs`
- **Font:** Font Awesome 5 Solid (`FontAwesome5Solid.otf`) registered as alias `"FontAwesome5Solid"`
- **Icons:** All icons use FA5 Solid glyphs with `DynamicResource PrayAppAccentColor`
- **Storage:** `ILocalStorageService` → JSON files in `FileSystem.AppDataDirectory`
- **Theme:** XAML files as `EmbeddedResource`, loaded via `Assembly.GetManifestResourceStream`

---

## Completed Work (as of 2026-08-03)

### Foundation
- [x] App boots on iOS/Android (SIGABRT crashes fixed)
- [x] Theme system (light/dark/sepia) loading from embedded XAML
- [x] `ILocalStorageService` — JSON persistence
- [x] `IPrayerService` — loads `prayers_master.json` + individual prayer JSON files
- [x] Shell flyout navigation with FA5 icons via `FontImageSource`
- [x] `DashboardPage` — featured strip + category grid of `PrayerTileView` tiles
- [x] `PrayerPage` — multi-section prayer with progress dots, prev/next nav
- [x] `SettingsPage` — theme picker, font size

### Prayer Page features
- [x] FA5 icons throughout (all glyph constants in `Services/FA.cs`)
- [x] Bullet/numbered items aligned via `FormattedString` + `PadRight(5)`
- [x] Section counter shows `CurrentSectionNumber / SectionCount` (1-based)
- [x] "Amen" completion card on last section (spring animation, contextual blessing)
- [x] Divine fly animation (`DivineFlyView`) — timer fix (dispatched to MainThread)
- [x] Background image per prayer (`PrayerData.HeaderImageFile`) with opacity 0.07
- [x] Default background SVG (`prayer_bg_default.svg`) — subtle cross watermark
- [x] Inline image blocks (`ContentBlock.Type = "Image"`) rendered in `ContentBlockView`

### Related prayers flow
- [x] `FromPrayerId` query param passed on navigation
- [x] Related section splits into "Continue With" (forward) + "Came From" (source, dimmed)

### Favourites
- [x] `IFavouriteService` / `FavouriteService` — persisted via `ILocalStorageService`
- [x] `IsFavourite` on `PrayerMasterEntry` (runtime-only, `[JsonIgnore]`)
- [x] Heart badge on `PrayerTileView` — grey outline → red filled
- [x] Tap heart to toggle; separate from tap-tile-to-navigate
- [x] Heart filter button in dashboard header — turns red when active
- [x] Filter rebuilds Featured + Category sections together

---

## Architecture: Content Node System (Phase 1 — next)

### The problem with the current flat structure
Everything in `prayers_master.json` is a single-level list of prayers. Novenas, Stations,
Saints, and Bible all require sub-lists — a Collection layer between the dashboard and PrayerPage.

### New node model
`PrayerMasterEntry` gains these fields:

```csharp
public string NodeType { get; set; } = "Prayer";
// "Prayer"       → navigate directly to PrayerPage
// "Collection"   → navigate to CollectionPage (sub-list of children)
// "Novena"       → Collection + date metadata
// "ExternalLink" → navigate to LinkDetailPage (description + Open App button)
// "Bible"        → navigate to BiblePage (dedicated)

public string? ParentId { get; set; }              // breadcrumb context
public string[] ChildIds { get; set; } = [];       // IDs of children (Collection/Novena)
public string? FeastDate { get; set; }             // "May 13"  (Novena)
public string? StartDate { get; set; }             // "May 4"   (Novena)
public string? ExternalAppUrl { get; set; }        // (ExternalLink)
public string? ExternalAppDescription { get; set; } // (ExternalLink)
```

### New master index
`prayers_master.json` → `content_index.json`

Top-level structure:
```json
{
  "Sections": [
    { "Id": "prayers",  "NodeType": "Collection", "Title": "Prayers",              "ChildIds": [...] },
    { "Id": "novenas",  "NodeType": "Collection", "Title": "Novenas",              "ChildIds": [...] },
    { "Id": "stations", "NodeType": "Prayer",     "Title": "Stations of the Cross","DataFile": "Stations/stations_of_the_cross.json" },
    { "Id": "saints",   "NodeType": "Collection", "Title": "Saints",               "ChildIds": [...] },
    { "Id": "bible",    "NodeType": "Bible",      "Title": "The Bible" }
  ],
  "Prayers": [ ...all prayer/novena/saint/link entries (flat lookup table)... ]
}
```

### New data folder layout
```
Resources/Raw/Data/
  content_index.json              ← replaces prayers_master.json
  Prayers/                        ← existing JSON files move here
    our_father.json
    hail_mary.json
    ... (all existing 22 files)
  Novenas/
    novena_index.json             ← list entry for each novena (date metadata)
    divine_mercy_novena.json
    st_joseph_novena.json
    our_lady_of_fatima_novena.json
    ... (target: 50 novenas at launch)
  Stations/
    stations_of_the_cross.json    ← 14 stations as PrayerSections
  Saints/
    saints_index.json             ← ExternalLink entries only (no prayer text)
  Bible/
    bible_index.json              ← 73-book Catholic canon index
    OT/
      genesis.json
      exodus.json
      ... (45 OT books)
    NT/
      matthew.json
      mark.json
      ... (27 NT books + 1 deuterocanonical NT)
  i18n/
    Prayers/
      our_father.la.json          ← Latin
      hail_mary.la.json
      glory_be.la.json
      apostles_creed.la.json
      nicene_creed.la.json
      our_father.es.json          ← Spanish
      hail_mary.es.json
      ... (full Spanish pack)
```

### New screens
| Screen | File | Status |
|---|---|---|
| `CollectionPage` | `Views/CollectionPage.xaml` | Planned |
| `CollectionViewModel` | `ViewModels/CollectionViewModel.cs` | Planned |
| `LinkDetailPage` | `Views/LinkDetailPage.xaml` | Planned |
| `BiblePage` | `Views/BiblePage.xaml` | Planned |
| `BookPage` | `Views/BookPage.xaml` | Planned |

### Dashboard changes
Current category grid → **section cards** (large tappable rows):

```
┌─────────────────────────────────────┐
│  🙏  Prayers                    →   │
│  Traditional & daily prayers        │
├─────────────────────────────────────┤
│  📿  Novenas                    →   │
│  200+ novenas · 3 active now        │
├─────────────────────────────────────┤
│  ✝   Stations of the Cross      →   │
│  The 14 Stations                    │
├─────────────────────────────────────┤
│  👼  Saints                     →   │
│  Lives & devotions                  │
├─────────────────────────────────────┤
│  📖  The Bible                  →   │
│  73 books · Catholic canon          │
└─────────────────────────────────────┘
```

Favourites filter applies across all sections.

---

## Phase Breakdown

### Phase 1 — Content Node Foundation
**Goal:** Everything else depends on this.

- [ ] Add `NodeType`, `ParentId`, `ChildIds`, `FeastDate`, `StartDate`, `ExternalAppUrl`,
      `ExternalAppDescription` to `PrayerMasterEntry`
- [ ] Create `content_index.json` (wraps existing prayers, adds section entries)
- [ ] Update `IPrayerService` / `PrayerService` to read `content_index.json`
- [ ] Create `CollectionPage` + `CollectionViewModel`
  - Header: section title + subtitle
  - List of child entries (reuse tile style or list-row style based on count)
  - Tap → routes to `PrayerPage`, `CollectionPage` (nested), `LinkDetailPage`, or `BiblePage`
- [ ] Update `DashboardPage` to show section cards instead of flat category grid
- [ ] Register `CollectionPage` in Shell routes + DI
- [ ] Existing prayer navigation unchanged (tile → `PrayerPage?PrayerId=...`)

### Phase 2 — Stations of the Cross
**Goal:** Quick win — no new code, just data.

- [ ] Create `Stations/stations_of_the_cross.json`
  - Format: `MultiPage`, 16 sections (Opening + 14 Stations + Closing)
  - Each station: H2 title, image block (if image available), reflection paragraph, station prayer
- [ ] Add station entry to `content_index.json` with `NodeType: "Prayer"`
- [ ] Add `stations_of_the_cross` to `prayers_master` section children
- [ ] Verify it opens correctly in existing `PrayerPage`

### Phase 3 — Novenas
**Goal:** Full novena section with date intelligence.

- [ ] Create `Novenas/novena_index.json` — 50 priority novenas with:
  - `FeastDate` (e.g. `"May 13"`)
  - `StartDate` (e.g. `"May 4"` = feast - 9 days)
  - `DataFile` path
- [ ] Create novena JSON files (50 novenas, 9 sections each = 1 per day)
  - Priority order: Divine Mercy, Our Lady of Fatima, St. Joseph, St. Michael,
    Sacred Heart, Holy Spirit, Immaculate Conception, Christmas, Easter, St. Anthony,
    St. Therese, St. Francis, St. Patrick, St. Jude, Assumption, Corpus Christi,
    Pentecost, Advent, St. Anne, St. Padre Pio … (continue to 50)
- [ ] `CollectionPage` novena mode:
  - "Active Now" section at top (today between StartDate and StartDate+8)
  - Cards show feast date badge
  - Filter chips: All | Active Now | By Month
- [ ] `IsActiveNovena` computed from today's date in `CollectionViewModel`

### Phase 4 — Saints & External Links
**Goal:** Showcase partner/related apps with a polished handoff screen.

- [ ] Create `Saints/saints_index.json`
  - Each entry: `NodeType: "ExternalLink"`, description (2–3 sentences), app store URL
  - Suggested apps: Hallow, iBreviary, Laudate, iMissal, Catholic Saints app
- [ ] Create `LinkDetailPage` + `LinkDetailViewModel`
  - Layout: large icon, title, description paragraphs, "Open App" button
  - Button calls `Launcher.OpenAsync(Uri)` with try/catch
  - No navigation back complications — simple push/pop
- [ ] Register `LinkDetailPage` in Shell routes
- [ ] Add saints section to `content_index.json`

### Phase 5 — The Bible (index + pilot books)
**Goal:** Architecture and 5 pilot books; full content in Phase 6.

- [ ] Create `Bible/bible_index.json` — all 73 books:
  - Fields: `Id`, `Title`, `Testament` (OT/NT), `Abbreviation`, `Chapters` (count), `DataFile`
  - Catholic canon: 46 OT (includes 7 deuterocanonicals) + 27 NT = 73
- [ ] Create `BiblePage` + `BibleViewModel`
  - Two tabs: Old Testament / New Testament
  - Book list with chapter count badge
  - Tap → `BookPage` (chapter list) or directly to `PrayerPage` if single-chapter
- [ ] Create `BookPage` + `BookViewModel`
  - Simple list of chapters
  - Tap → `PrayerPage?PrayerId=...` (each chapter = one PrayerSection)
- [ ] Create pilot book JSON files (5 books):
  - `Bible/NT/matthew.json` — 28 chapters
  - `Bible/NT/mark.json` — 16 chapters
  - `Bible/NT/luke.json` — 24 chapters
  - `Bible/NT/john.json` — 21 chapters
  - `Bible/OT/psalms.json` — 150 chapters (Psalm per section)
- [ ] Register `BiblePage`, `BookPage` in Shell + DI

### Phase 6 — Full Bible Content
**Goal:** All 73 books populated.

- [ ] Create remaining 68 book JSON files
  - OT: Genesis through Malachi (46 books)
  - NT: Acts through Revelation (23 remaining books)
- [ ] Each book: one `PrayerSection` per chapter, verse text as `Paragraph` blocks
- [ ] Consider chunking very large books (Psalms 150, Isaiah 66) into sub-sections
- [ ] Bible search — extend `IPrayerService.SearchAsync` to search Bible content

### Phase 7 — Multi-language
**Goal:** Latin core + Spanish full pack.

#### Latin (core prayers)
- [ ] `i18n/Prayers/our_father.la.json`
- [ ] `i18n/Prayers/hail_mary.la.json`
- [ ] `i18n/Prayers/glory_be.la.json`
- [ ] `i18n/Prayers/apostles_creed.la.json`
- [ ] `i18n/Prayers/nicene_creed.la.json`

#### Spanish (full pack — all 22 current prayers)
- [ ] `i18n/Prayers/our_father.es.json` through all existing prayer files

#### Infrastructure
- [ ] Add `ContentLanguage` (`"en"` | `"la"` | `"es"`) to `AppSettings`
- [ ] Update `PrayerService.GetPrayerDataAsync(dataFile)` to:
  1. Build locale path: `{base}.{lang}.json`
  2. Try locale file first via `FileSystem.OpenAppPackageFileAsync`
  3. Fall back to base English file if locale file missing
- [ ] Add "Prayer Language" picker to `SettingsPage`
  - Options: English | Latin (core only) | Español
  - On change: clear `PrayerService` cache, rebuild dashboard
- [ ] Show language badge on prayer tile when non-English active

---

## Key Technical Notes (carry forward to every session)

### SIGABRT prevention
- All `async void` in constructors must be fire-and-forget with top-level try/catch
- `System.Timers.Timer.Elapsed` fires on TP Worker — ALWAYS wrap body in `MainThread.BeginInvokeOnMainThread`
- `TaskScheduler.UnobservedTaskException` global handler registered in `MauiProgram.cs`

### Theme system
- `Theme_Styles_Common.xaml` is `EmbeddedResource` (NOT `MauiXaml`) — critical, do not change
- Theme colors via `DynamicResource` — never hardcode colors in views

### Shell navigation
- New pages need both `Shell.RegisterRoute` (in `AppShell.xaml.cs` or `MauiProgram`) AND DI registration
- Use `GoToAsync("//DashboardPage")` for root reset, `GoToAsync("PageName?Param=value")` for push

### FA5 Solid glyphs
- All constants in `Services/FA.cs`
- `FontFamily="FontAwesome5Solid"` required on every Label/Button using glyphs
- FA5 Regular glyphs (open variants) are NOT bundled — use only Solid

### Data files
- Prayer JSON files are `MauiAsset` with `LogicalName="%(RecursiveDir)%(Filename)%(Extension)"`
- Load via `FileSystem.OpenAppPackageFileAsync(path)` in `PrayerService`
- `PrayerMasterEntry.IsFavourite` is `[JsonIgnore]` — runtime only, never serialised

### Favourites
- `FavouriteService` caches the `HashSet<string>` in memory after first load
- Tile heart tap flips `IsFavourite` on the tile first, THEN fires `FavouriteTapped`
- `ToggleFavouriteAsync` uses `entry.IsFavourite` as-is (do NOT negate — tile already did)

### Images
- Background: `PrayerData.HeaderImageFile` → `ImageSource.FromFile(file)`, fallback `prayer_bg_default.svg`
- Inline: `ContentBlock.Type = "Image"` with `ImageFile` + optional `ImageCaption`
- All images in `Resources/Images/` covered by `<MauiImage Include="Resources/Images/*" />`

---

## Novena Source
Site: https://catholicnovenaapp.com/list-of-all-novenas/
- 200+ novenas organised by liturgical calendar month
- Each entry has: name, start date, feast day
- Content to be adapted into our JSON format (not copied verbatim)
- Display order in our app: alphabetical primary, "active now" secondary filter

## Priority Novenas (Phase 3, first 20)
1. Divine Mercy Novena — Good Friday → Divine Mercy Sunday
2. Our Lady of Fatima — May 4 → May 13
3. St. Joseph — Mar 10 → Mar 19
4. St. Michael — Sep 20 → Sep 29
5. Sacred Heart of Jesus — variable (Friday after Corpus Christi - 8)
6. Holy Spirit — 10 days before Pentecost
7. Immaculate Conception — Nov 29 → Dec 8
8. Christmas Novena — Dec 16 → Dec 24
9. St. Anthony of Padua — Jun 4 → Jun 13
10. St. Therese of Lisieux — Sep 23 → Oct 1
11. St. Francis of Assisi — Sep 27 → Oct 4
12. St. Patrick — Mar 8 → Mar 17
13. St. Jude — Oct 19 → Oct 28
14. Assumption of Mary — Aug 6 → Aug 15
15. Corpus Christi — variable
16. Pentecost Novena — 10 days before Pentecost
17. St. Anne — Jul 17 → Jul 26
18. St. Padre Pio — Sep 14 → Sep 23
19. Our Lady of Lourdes — Jan 2 → Feb 11
20. St. Ignatius of Loyola — Jul 22 → Jul 31
