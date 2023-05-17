namespace fusion.geartracker.sheet;

public class GoogleSheetsLootBuilder : GoogleSheetsBuilder
{
    public int DateColumnIndex { get => headers.IndexOf("Date Received"); }
    public int IconColumnIndex { get => headers.IndexOf("Icon"); }
    public int ItemIdColumnIndex { get => headers.IndexOf("Item Id"); }
    public int ItemNameColumnIndex { get => headers.IndexOf("Item Name"); }
    public int PlayerNameColumnIndex { get => headers.IndexOf("Player Name"); }

    private List<string> headers = new() { "Item Id", "Icon", "Item Name", "Player Name", "Date Received" };


    public void AddHeaders ()
    {
        var row = new List<object>(headers);

        data.Add(row);
    }


    public GoogleSheetsLootBuilder (Spreadsheet spreadsheet, Sheet sheet) : base(spreadsheet, sheet)
    {

    }
}