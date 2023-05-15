namespace fusion.geartracker.sheet.request;

public class GooglePlayersMergeCellsLeftRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsPlayersBuilder builder)
    {
        var gridRanges = builder.GetPlayerGridRanges();

        return gridRanges.ConvertAll(gridRange => new Request
        {
            MergeCells = new()
            {
                MergeType = "MERGE_COLUMNS",
                Range = gridRange,
            },
        });
    }


    private GooglePlayersMergeCellsLeftRequest ()
    {

    }
}