namespace fusion.geartracker.sheet.request;

public class GoogleLootFreezeCellsRequest : Request
{
    public GoogleLootFreezeCellsRequest (GoogleSheetsLootBuilder builder)
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