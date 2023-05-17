namespace fusion.geartracker.sheet.request;

public class GoogleItemsFormatCellsIconRequest : Request
{
    public GoogleItemsFormatCellsIconRequest (GoogleSheetsItemsBuilder builder)
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
            Range = new()
            {
                StartColumnIndex = builder.IconColumnIndex,
                EndColumnIndex = builder.IconColumnIndex + 1,
                StartRowIndex = 1,
                SheetId = builder.Sheet.Properties.SheetId,
            }
        };
    }
}