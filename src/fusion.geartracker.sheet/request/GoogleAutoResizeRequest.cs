namespace fusion.geartracker.sheet.request;

public class GoogleAutoResizeRequest : Request
{
    public GoogleAutoResizeRequest (GoogleSheetsBuilder builder)
    {
        AutoResizeDimensions = new()
        {
            Dimensions = new()
            {
                Dimension = "COLUMNS",
                SheetId = builder.Sheet.Properties.SheetId,
            },
        };
    }
}