namespace fusion.geartracker.sheet.request;

public class GoogleLootFormatCellsNameRequest : Request
{
    public GoogleLootFormatCellsNameRequest (GoogleSheetsLootBuilder builder)
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
                        Type = "ONE_OF_RANGE",
                        Values = new List<ConditionValue>()
                        {
                            new() { UserEnteredValue = "='Players'!$B$2:$B" },
                        },
                    },
                    ShowCustomUi = true,
                },
            },
            Range = new()
            {
                StartColumnIndex = builder.PlayerNameColumnIndex,
                EndColumnIndex = builder.PlayerNameColumnIndex + 1,
                StartRowIndex = 1,
                SheetId = builder.Sheet.Properties.SheetId,
            }
        };
    }
}