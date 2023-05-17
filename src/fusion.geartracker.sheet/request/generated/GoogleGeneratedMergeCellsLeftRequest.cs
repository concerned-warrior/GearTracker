namespace fusion.geartracker.sheet.request;

public class GoogleGeneratedMergeCellsLeftRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsGeneratedBuilder builder)
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


    private GoogleGeneratedMergeCellsLeftRequest ()
    {

    }
}