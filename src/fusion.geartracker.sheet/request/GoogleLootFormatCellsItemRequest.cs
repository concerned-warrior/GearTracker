namespace fusion.geartracker.sheet.request;

public class GoogleLootFormatCellsItemRequest : Request
{
    public GoogleLootFormatCellsItemRequest (GoogleSheetsLootBuilder builder)
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
                            new() { UserEnteredValue = "='Known Items'!$E$2:$E" },
                        },
                    },
                    ShowCustomUi = true,
                },
            },
            Range = new()
            {
                StartColumnIndex = 2,
                EndColumnIndex = 3,
                StartRowIndex = 1,
                SheetId = builder.Sheet.Properties.SheetId,
            }
        };
    }
}