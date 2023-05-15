namespace fusion.geartracker.sheet.request;

public class GoogleLootFormatCellsDateRequest : Request
{
    public GoogleLootFormatCellsDateRequest (GoogleSheetsLootBuilder builder)
    {
        RepeatCell = new()
        {
            Fields = "dataValidation.Condition",
            Cell = new()
            {
                DataValidation = new()
                {
                    Condition = new()
                    {
                        Type = "DATE_IS_VALID",
                    },
                    ShowCustomUi = true,
                },
            },
            Range = new()
            {
                StartColumnIndex = 4,
                EndColumnIndex = 5,
                StartRowIndex = 1,
                SheetId = builder.Sheet.Properties.SheetId,
            }
        };
    }
}