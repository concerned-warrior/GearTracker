namespace fusion.geartracker.sheet;

public class GoogleSheetsLootBuilder : GoogleSheetsBuilder
{
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