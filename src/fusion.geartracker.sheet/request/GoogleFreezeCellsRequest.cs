namespace fusion.geartracker.sheet.request;

public class GoogleFreezeCellsRequest : Request
{
    public GoogleFreezeCellsRequest (Spreadsheet spreadsheet, Sheet sheet)
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
                SheetId = sheet.Properties.SheetId,
            },
        };
    }
}