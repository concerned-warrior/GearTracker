namespace fusion.geartracker.sheet.request;

public class GoogleFormatCellsIconRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsBuilder builder)
    {
        var gridRanges = builder.GetItemGroupIconGridRanges();

        return gridRanges.ConvertAll(gridRange => new Request
        {
            RepeatCell = new()
            {
                Fields = "userEnteredFormat.horizontalAlignment",
                Cell = new()
                {
                    UserEnteredFormat = new()
                    {
                        HorizontalAlignment = "CENTER",
                    },
                },
                Range = gridRange,
            },
        });
    }


    private GoogleFormatCellsIconRequest ()
    {

    }
}