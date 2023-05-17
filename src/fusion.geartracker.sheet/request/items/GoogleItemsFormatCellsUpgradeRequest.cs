namespace fusion.geartracker.sheet.request;

public class GoogleItemsFormatCellsUpgradeRequest : Request
{
    public GoogleItemsFormatCellsUpgradeRequest (GoogleSheetsItemsBuilder builder)
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
                        Type = "ONE_OF_LIST",
                        Values = new List<ConditionValue>()
                        {
                            new() { UserEnteredValue = nameof(UpgradeType.Minor) },
                            new() { UserEnteredValue = nameof(UpgradeType.Moderate) },
                            new() { UserEnteredValue = nameof(UpgradeType.Major) },
                        },
                    },
                    ShowCustomUi = true,
                },
            },
            Range = new()
            {
                StartColumnIndex = builder.UpgradeColumnIndex,
                EndColumnIndex = builder.UpgradeColumnIndex + 1,
                StartRowIndex = 1,
                SheetId = builder.Sheet.Properties.SheetId,
            }
        };
    }
}