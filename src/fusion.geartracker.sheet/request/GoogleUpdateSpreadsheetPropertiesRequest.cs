namespace fusion.geartracker.sheet.request;

public class GoogleUpdateSpreadsheetPropertiesRequest : Request
{
    public GoogleUpdateSpreadsheetPropertiesRequest (GoogleSheetsBuilder builder)
    {
        UpdateSpreadsheetProperties = new()
        {
            Fields = "title,spreadsheetTheme",
            Properties = new()
            {
                Title = "Gear Tracker",
                SpreadsheetTheme = new()
                {
                    PrimaryFontFamily = "Roboto Mono",
                    ThemeColors = builder.Spreadsheet.Properties.SpreadsheetTheme.ThemeColors,
                },
            },
        };
    }
}