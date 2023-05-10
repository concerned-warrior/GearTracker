namespace fusion.geartracker.sheet.request;

public class GoogleFormatCellsIconRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsBuilder builder, Sheet sheet)
    {
        var gridRanges = builder.GetItemGroupIconGridRanges(sheet);

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