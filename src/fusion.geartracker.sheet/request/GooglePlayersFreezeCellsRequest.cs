namespace fusion.geartracker.sheet.request;

public class GooglePlayersFreezeCellsRequest : Request
{
    public GooglePlayersFreezeCellsRequest (GoogleSheetsPlayersBuilder builder)
    {
        UpdateSheetProperties = new()
        {
            Fields = "gridProperties.frozenColumnCount,gridProperties.frozenRowCount",
            Properties = new()
            {
                GridProperties = new()
                {
                    FrozenColumnCount = builder.ItemGroupStartColumnIndex,
                    FrozenRowCount = 2,
                },
                SheetId = builder.Sheet.Properties.SheetId,
            },
        };
    }
}