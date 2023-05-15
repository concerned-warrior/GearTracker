namespace fusion.geartracker.sheet.request;

public class GooglePlayersFreezeCellsRequest : Request
{
    public GooglePlayersFreezeCellsRequest (GoogleSheetsBuilder builder)
    {
        UpdateSheetProperties = new()
        {
            Fields = "gridProperties.frozenColumnCount,gridProperties.frozenRowCount",
            Properties = new()
            {
                GridProperties = new()
                {
                    FrozenColumnCount = 7,
                    FrozenRowCount = 2,
                },
                SheetId = builder.Sheet.Properties.SheetId,
            },
        };
    }
}