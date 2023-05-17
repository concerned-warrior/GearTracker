namespace fusion.geartracker.sheet.request;

public class GooglePlayersFormatCellsDataRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsPlayersBuilder builder)
    {
        var gridRanges = builder.GetPlayerGridRanges();

        return gridRanges.ConvertAll(gridRange =>
        {
            gridRange.StartColumnIndex = builder.ItemGroupStartColumnIndex;
            gridRange.EndColumnIndex = null;
            gridRange.EndRowIndex = gridRange.EndRowIndex - 1;

            return new Request
            {
                RepeatCell = new()
                {
                    Fields = "userEnteredFormat.borders",
                    Cell = new()
                    {
                        UserEnteredFormat = new()
                        {
                            Borders = new()
                            {
                                Bottom = new()
                                {
                                    Style = "NONE",
                                },
                            },
                        },
                    },
                    Range = gridRange,
                },
            };
        });
    }



    private GooglePlayersFormatCellsDataRequest ()
    {

    }
}