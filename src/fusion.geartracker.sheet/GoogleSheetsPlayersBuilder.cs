namespace fusion.geartracker.sheet;

public class GoogleSheetsPlayersBuilder : GoogleSheetsBuilder
{
    public int ClassColumnIndex { get => headers.IndexOf("Class"); }
    public int NameColumnIndex { get => headers.IndexOf("Name"); }
    public int RaidColumnIndex { get => headers.IndexOf("Raid"); }
    public int SpecColumnIndex { get => headers.IndexOf("Spec"); }

    public int DataStartRowIndex { get => 1; }

    private List<string> headers = new() { "Raid", "Name", "Class", "Spec" };


    public void AddHeaders ()
    {
        var row = new List<object>(headers);

        data.Add(row);
    }


    public void AddPlayer (WCLPlayer player)
    {
        var row = new List<object> { player.Raid, player.Name, player.Class, player.Spec };

        data.Add(row);
    }


    public GoogleSheetsPlayersBuilder (Spreadsheet spreadsheet, Sheet sheet) : base(spreadsheet, sheet)
    {

    }
}