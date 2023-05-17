namespace fusion.geartracker.sheet.request;

public class GooglePlayersCenterSpecRequest : Request
{
    public GooglePlayersCenterSpecRequest (GoogleSheetsPlayersBuilder builder)
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
                StartColumnIndex = builder.SpecColumnIndex,
                EndColumnIndex = builder.SpecColumnIndex + 1,
                SheetId = builder.Sheet.Properties.SheetId,
            },
        };
    }
}