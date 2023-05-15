namespace fusion.geartracker.sheet;

public class GoogleSheetsItemsBuilder : GoogleSheetsBuilder
{
    private List<string> headers = new() { "Id", "Ignore", "Slot", "Icon", "Name", "iLvl", "10 Man", "BIS", "Size of Upgrade" };



    public void AddHeaders ()
    {
        var row = new List<object>(headers);

        data.Add(row);
    }


    public void AddItem (WCLGear gear)
    {
        var row = new List<object> { gear.Id, gear.Ignore, gear.Slot, $"=image(\"https://assets.rpglogs.com/img/warcraft/abilities/{gear.Icon}\")", gear.Name, gear.ItemLevel, gear.InstanceSize == 10, gear.IsBIS, gear.SizeOfUpgrade.ToString() };

        data.Add(row);
    }


    public GoogleSheetsItemsBuilder (Spreadsheet spreadsheet, Sheet sheet) : base(spreadsheet, sheet)
    {

    }
}