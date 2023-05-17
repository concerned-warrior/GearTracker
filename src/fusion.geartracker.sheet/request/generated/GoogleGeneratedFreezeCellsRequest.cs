namespace fusion.geartracker.sheet.request;

public class GoogleGeneratedFreezeCellsRequest : Request
{
    public GoogleGeneratedFreezeCellsRequest (GoogleSheetsGeneratedBuilder builder)
    {
        UpdateSheetProperties = new()
        {
            Fields = "gridProperties.frozenColumnCount,gridProperties.frozenRowCount",
            Properties = new()
            {
                GridProperties = new()
                {
                    FrozenColumnCount = builder.ItemGroupStartColumnIndex,
                    FrozenRowCount = builder.DataStartRowIndex,
                },
                SheetId = builder.Sheet.Properties.SheetId,
            },
        };
    }
}