namespace fusion.geartracker.sheet.request;

public class GoogleGeneratedMergeCellsHeaderRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsGeneratedBuilder builder)
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


    private GoogleGeneratedMergeCellsHeaderRequest ()
    {

    }
}