namespace fusion.geartracker.sheet.request;

public class GoogleMergeCellsHeaderRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsBuilder builder, Sheet sheet)
    {
        var gridRanges = builder.GetItemGroupGridRanges(sheet);

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