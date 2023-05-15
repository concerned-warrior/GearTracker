namespace fusion.geartracker.sheet.request;

public class GooglePlayersMergeCellsHeaderRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsPlayersBuilder builder)
    {
        var gridRanges = builder.GetItemGroupGridRanges();

        return gridRanges.ConvertAll(gridRange => new Request
        {
            MergeCells = new()
            {
                Range = gridRange,
            },
        });
    }


    private GooglePlayersMergeCellsHeaderRequest ()
    {

    }
}