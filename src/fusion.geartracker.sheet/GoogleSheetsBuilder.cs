namespace fusion.geartracker.sheet;

public class GoogleSheetsBuilder
{
    public Spreadsheet Spreadsheet { get; }
    public Sheet Sheet { get; }

    private List<List<object>> data = new();
    private List<string> headersItemGroup = new() { "Icon", "iLvl", "Name", "Date" };
    private List<string> headersLeft = new() { "Name", "Raid", "aiLvl", "Class", "Spec", "Last 10", "Last 25" };
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
        var last10 = player.GetLast10().ToLocalTime().ToString("d");
        var last25 = player.GetLast25().ToLocalTime().ToString("d");
        var gear = player.GearById.Values.ToList();
        var rows = new List<List<object>>();
        var rowCount = 1;

        gear.Sort((a, b) => b.ItemLevel.CompareTo(a.ItemLevel));

        for (var i = 0; i < rowCount; i++)
        {
            var row = new List<object> { player.Name, player.Raid, player.GetAverageItemLevel(), player.Class, player.Spec, last10, last25 };

            foreach (var slot in headersSlots)
            {
                var slotGear = gear.Aggregate(new List<WCLGear>(), (list, item) =>
                {
                    var existingItem = list.Find(existingItem => existingItem.Id.Equals(item.Id));

                    if (item.Slot.Equals(slot) && existingItem is null)
                    {
                        list.Add(item);
                    }

                    return list;
                });
                var currentItem = slotGear.ElementAtOrDefault(i);

                rowCount = rowCount > slotGear.Count ? rowCount : slotGear.Count;

                if (currentItem is null)
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


    public string GetA1Range ()
    {
        var max = 1;

        foreach (var list in data)
        {
            max = max > list.Count ? max : list.Count;
        }

        return $"'{Sheet.Properties.Title}'!A1:{getColumnOffset(max)}{data.Count}";
    }


    public List<GridRange> GetPlayerGridRanges ()
    {
        var gridRanges = new List<GridRange>();
        var currentName = string.Empty;
        var startingRowCurrentName = 2;
        var startingColumn = 0;
        var endingColumn = headersLeft.Count;
        var i = 0;

        for (i = startingRowCurrentName; i < data.Count; i++)
        {
            var row = data[i];
            var name = row.ElementAtOrDefault(0) as string;

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
                StartRowIndex = 2,
                SheetId = Sheet.Properties.SheetId,
            });

            startingColumn += headersItemGroup.Count;
        }

        return gridRanges;
    }


    private string getColumnOffset (int offset)
    {
        var column = string.Empty;
        var letter = (int)'A';

        offset -= 25;

        while (offset > 0)
        {
            column += 'Z';

            offset -= 26;
        }

        column += (char)(letter + offset + 25);

        return column;
    }


    public IList<IList<object>> GetData ()
    {
        return data.ConvertAll(list => (IList<object>)list);
    }


    public GoogleSheetsBuilder (Spreadsheet spreadsheet, Sheet sheet)
    {
        Spreadsheet = spreadsheet;
        Sheet = sheet;
    }
}