namespace fusion.geartracker.sheet.request;

public class GoogleAutoResizeRequest : Request
{
    public GoogleAutoResizeRequest (Spreadsheet spreadsheet, Sheet sheet)
    {
        AutoResizeDimensions = new()
        {
            Dimensions = new()
            {
                Dimension = "COLUMNS",
                SheetId = sheet.Properties.SheetId,
            },
        };
    }
}