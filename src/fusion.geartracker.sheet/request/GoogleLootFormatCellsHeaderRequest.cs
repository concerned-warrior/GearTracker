namespace fusion.geartracker.sheet.request;

public class GoogleLootFormatCellsHeaderRequest : Request
{
    public GoogleLootFormatCellsHeaderRequest (GoogleSheetsLootBuilder builder)
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
            Range = new()
            {
                StartRowIndex = 0,
                EndRowIndex = 1,
                SheetId = builder.Sheet.Properties.SheetId,
            }
        };
    }
}