namespace fusion.geartracker.sheet.request;

public class GoogleMergeCellsHeaderRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsBuilder builder)
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


    private GoogleMergeCellsHeaderRequest ()
    {

    }
}