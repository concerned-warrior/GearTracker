namespace fusion.geartracker.sheet;

public class GoogleSheetsGeneratedBuilder : GoogleSheetsBuilder
{
    public int ItemGroupStartColumnIndex { get => headersLeft.Count; }
    public int RaidColumnIndex { get => headersLeft.IndexOf("Raid"); }
    public int SpecColumnIndex { get => headersLeft.IndexOf("Spec"); }
    public int Last10ColumnIndex { get => headersLeft.IndexOf("Last\n10"); }
    public int Last25ColumnIndex { get => headersLeft.IndexOf("Last\n25"); }
    public int LastMinColumnIndex { get => headersLeft.IndexOf("Last\nMin"); }
    public int LastModColumnIndex { get => headersLeft.IndexOf("Last\nMod"); }
    public int LastMajColumnIndex { get => headersLeft.IndexOf("Last\nMaj"); }
    public int LastBISColumnIndex { get => headersLeft.IndexOf("Last\nBIS"); }
    public int IsBISColumnIndex { get => headersLeft.IndexOf("Num\nBIS"); }
    public int AverageItemLevelColumnIndex { get => headersLeft.IndexOf("aiLvl"); }
    public int NameColumnIndex { get => headersLeft.IndexOf("Name"); }

    public int DataStartRowIndex { get => 2; }

    private int weeksToIgnore;

    private List<string> headersItemGroup = new() { "Icon", "iLvl", "Name", "Date" };
    private List<string> headersLeft = new() { "Raid", "Spec", "Last\n10", "Last\n25", "Last\nMin", "Last\nMod", "Last\nMaj", "Last\nBIS", "Num\nBIS", "aiLvl", "Name" };
    private List<string> headersSlots = new() { "Head", "Neck", "Shoulder", "Back", "Chest", "Wrist", "Hands", "Waist", "Legs", "Feet", "Finger", "Trinket", "Main Hand", "Off Hand", "Ranged" };


    public void AddHeaders ()
    {
        var row1 = new List<object>(Enumerable.Repeat(string.Empty, headersLeft.Count));
        var row2 = new List<object>(headersLeft);

        foreach (var slot in headersSlots)
        {
            row1.Add(slot);
            row1.AddRange(Enumerable.Repeat(string.Empty, headersItemGroup.Count - 1));

            row2.AddRange(headersItemGroup);
        }

        data.Add(row1);
        data.Add(row2);
    }


    public void AddPlayer (WCLPlayer player)
    {
        var gear = player.GetOrderedGear();
        var averageItemLevel = player.GetAverageItemLevel(gear);
        var last10 = (int)((DateTimeOffset.Now - player.GetLast10()).TotalDays / 7);
        var last25 = (int)((DateTimeOffset.Now - player.GetLast25()).TotalDays / 7);
        var lastMin = (int)((DateTimeOffset.Now - player.GetLastMin()).TotalDays / 7);
        var lastMod = (int)((DateTimeOffset.Now - player.GetLastMod()).TotalDays / 7);
        var lastMaj = (int)((DateTimeOffset.Now - player.GetLastMaj()).TotalDays / 7);
        var lastBIS = (int)((DateTimeOffset.Now - player.GetLastBIS()).TotalDays / 7);
        var bisCount = player.GetBISCount();
        var rows = new List<List<object>>();
        var rowCount = 1;

        for (var i = 0; i < rowCount; i++)
        {
            var row = new List<object> { player.Raid, $"{player.Spec}\n{player.Class}", last10, last25, lastMin, lastMod, lastMaj, lastBIS, bisCount, averageItemLevel, player.Name };

            foreach (var slot in headersSlots)
            {
                var slotGear = gear.Where(item => item.Slot.Equals(slot)).Aggregate(new HashSet<WCLGear>(), (gearSet, item) =>
                {
                    if (gearSet.TryGetValue(item, out var existingItem))
                    {
                        var olderItem = item.FirstSeenAt < existingItem.FirstSeenAt ? item : existingItem;

                        gearSet.Remove(item);
                        gearSet.Add(olderItem);
                    }
                    else
                    {
                        gearSet.Add(item);
                    }

                    return gearSet;
                }).Where(item => item.LastSeenAt > DateTimeOffset.Now.AddDays(weeksToIgnore * -7));

                var currentItem = slotGear.ElementAtOrDefault(i);

                rowCount = rowCount > slotGear.Count() ? rowCount : slotGear.Count();

                if (currentItem is null || currentItem.ItemLevel == 0)
                {
                    row.AddRange(Enumerable.Repeat(string.Empty, headersItemGroup.Count));
                }
                else
                {
                    row.AddRange(new List<object> { $"=image(\"https://assets.rpglogs.com/img/warcraft/abilities/{currentItem.Icon}\")", currentItem.ItemLevel, currentItem.Name, currentItem.FirstSeenAt.ToLocalTime().ToString("d") });
                }
            }

            rows.Add(row);
        }

        data.AddRange(rows);
    }


    public List<GridRange> GetPlayerGridRanges ()
    {
        var gridRanges = new List<GridRange>();
        var currentName = string.Empty;
        var startingRowCurrentName = DataStartRowIndex;
        var startingColumn = 0;
        var endingColumn = headersLeft.Count;
        var i = 0;

        for (i = startingRowCurrentName; i < data.Count; i++)
        {
            var row = data[i];
            var name = row.ElementAtOrDefault(NameColumnIndex) as string;

            if (string.IsNullOrWhiteSpace(name)) break;
            if (name.Equals(currentName)) continue;

            if (i - startingRowCurrentName > 1)
            {
                gridRanges.Add(new()
                {
                    StartColumnIndex = startingColumn,
                    EndColumnIndex = endingColumn,
                    StartRowIndex = startingRowCurrentName,
                    EndRowIndex = i,
                    SheetId = Sheet.Properties.SheetId,
                });
            }

            currentName = name;
            startingRowCurrentName = i;
        }

        if (i - startingRowCurrentName > 1)
        {
            gridRanges.Add(new()
            {
                StartColumnIndex = startingColumn,
                EndColumnIndex = endingColumn,
                StartRowIndex = startingRowCurrentName,
                EndRowIndex = i,
                SheetId = Sheet.Properties.SheetId,
            });
        }

        return gridRanges;
    }


    public List<GridRange> GetItemGroupGridRanges ()
    {
        var gridRanges = new List<GridRange>();
        var startingColumn = headersLeft.Count;
        var endingColumn = startingColumn + headersItemGroup.Count;

        foreach (var item in headersSlots)
        {
            gridRanges.Add(new()
            {
                StartColumnIndex = startingColumn,
                EndColumnIndex = endingColumn,
                StartRowIndex = 0,
                EndRowIndex = 1,
                SheetId = Sheet.Properties.SheetId,
            });

            startingColumn += headersItemGroup.Count;
            endingColumn += headersItemGroup.Count;
        }

        return gridRanges;
    }


    public List<GridRange> GetItemGroupIconGridRanges ()
    {
        var gridRanges = new List<GridRange>();
        var startingColumn = headersLeft.Count;

        foreach (var item in headersSlots)
        {
            gridRanges.Add(new()
            {
                StartColumnIndex = startingColumn,
                EndColumnIndex = startingColumn + 1,
                StartRowIndex = DataStartRowIndex,
                SheetId = Sheet.Properties.SheetId,
            });

            startingColumn += headersItemGroup.Count;
        }

        return gridRanges;
    }


    public GoogleSheetsGeneratedBuilder (Spreadsheet spreadsheet, Sheet sheet, int weeksToIgnore) : base(spreadsheet, sheet)
    {
        this.weeksToIgnore = weeksToIgnore;
    }
}