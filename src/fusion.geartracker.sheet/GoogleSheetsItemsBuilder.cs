namespace fusion.geartracker.sheet;

public class GoogleSheetsItemsBuilder : GoogleSheetsBuilder
{
    public int IconColumnIndex { get => headers.IndexOf("Icon"); }
    public int IgnoreColumnIndex { get => headers.IndexOf("Ignore"); }
    public int InstanceSizeColumnIndex { get => headers.IndexOf("10 Man"); }
    public int IsBISColumnIndex { get => headers.IndexOf("BIS"); }
    public int UpgradeColumnIndex { get => headers.IndexOf("Size of Upgrade"); }

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