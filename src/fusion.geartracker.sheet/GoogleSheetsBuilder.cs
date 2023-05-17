namespace fusion.geartracker.sheet;

public abstract class GoogleSheetsBuilder
{
    public Spreadsheet Spreadsheet { get; }
    public Sheet Sheet { get; }

    protected List<List<object>> data = new();


    public string GetA1Range ()
    {
        var max = 1;

        foreach (var list in data)
        {
            max = max > list.Count ? max : list.Count;
        }

        return $"'{Sheet.Properties.Title}'!A1:{GetColumnOffset(max)}{data.Count}";
    }


    public string GetColumnOffset (int offset)
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