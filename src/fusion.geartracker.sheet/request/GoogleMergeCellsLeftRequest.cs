namespace fusion.geartracker.sheet.request;

public class GoogleMergeCellsLeftRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsBuilder builder, Sheet sheet)
    {
        var gridRanges = builder.GetPlayerGridRanges(sheet);

        return gridRanges.ConvertAll(gridRange => new Request
        {
            MergeCells = new()
            {
                MergeType = "MERGE_COLUMNS",
                Range = gridRange,
            },
        });
    }


    private GoogleMergeCellsLeftRequest ()
    {

    }
}