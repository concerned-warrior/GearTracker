namespace fusion.geartracker.sheet.request;

public class GoogleItemsFreezeCellsRequest : Request
{
    public GoogleItemsFreezeCellsRequest (GoogleSheetsItemsBuilder builder)
    {
        UpdateSheetProperties = new()
        {
            Fields = "gridProperties.frozenColumnCount,gridProperties.frozenRowCount",
            Properties = new()
            {
                GridProperties = new()
                {
                    FrozenRowCount = builder.DataStartRowIndex,
                },
                SheetId = builder.Sheet.Properties.SheetId,
            },
        };
    }
}