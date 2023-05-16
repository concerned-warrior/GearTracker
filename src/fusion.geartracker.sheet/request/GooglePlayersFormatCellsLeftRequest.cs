namespace fusion.geartracker.sheet.request;

public class GooglePlayersFormatCellsLeftRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsPlayersBuilder builder)
    {
        var gridRanges = builder.GetPlayerGridRanges();

        return gridRanges.ConvertAll(gridRange =>
        {
            gridRange.StartColumnIndex = 7;
            gridRange.EndColumnIndex = null;
            gridRange.EndRowIndex = gridRange.EndRowIndex - 1;

            return new Request
            {
                RepeatCell = new()
                {
                    Fields = "userEnteredFormat.borders,userEnteredFormat.verticalAlignment",
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
                            VerticalAlignment = "MIDDLE",
                        },
                    },
                    Range = gridRange,
                },
            };
        });
    }



    private GooglePlayersFormatCellsLeftRequest ()
    {

    }
}