namespace fusion.geartracker.sheet.request;

public class GoogleCenterHeaderTextRequest : Request
{
    public GoogleCenterHeaderTextRequest (GoogleSheetsBuilder builder)
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
                StartRowIndex = 1,
                EndRowIndex = 2,
                SheetId = builder.Sheet.Properties.SheetId,
            },
        };
    }
}