namespace fusion.geartracker.sheet.request;

public class GoogleUpdateSpreadsheetPropertiesRequest : Request
{
    public GoogleUpdateSpreadsheetPropertiesRequest (Spreadsheet spreadsheet)
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
                    ThemeColors = spreadsheet.Properties.SpreadsheetTheme.ThemeColors,
                },
            },
        };
    }
}