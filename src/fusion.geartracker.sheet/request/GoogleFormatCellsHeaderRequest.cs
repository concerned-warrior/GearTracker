namespace fusion.geartracker.sheet.request;

public class GoogleFormatCellsHeaderRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsBuilder builder)
    {
        var gridRanges = builder.GetItemGroupGridRanges();

        return gridRanges.ConvertAll(gridRange => new Request
        {
            RepeatCell = new()
            {
                Fields = "userEnteredFormat.horizontalAlignment,userEnteredFormat.textFormat",
                Cell = new()
                {
                    UserEnteredFormat = new()
                    {
                        HorizontalAlignment = "CENTER",
                        TextFormat = new()
                        {
                            FontSize = 12,
                            Bold = true,
                            ForegroundColorStyle = new()
                            {
                                RgbColor = new()
                                {
                                    Red = 1,
                                    Green = 1,
                                    Blue = 1,
                                },
                            },
                        },
                    },
                },
                Range = gridRange,
            },
        });
    }


    private GoogleFormatCellsHeaderRequest ()
    {

    }
}