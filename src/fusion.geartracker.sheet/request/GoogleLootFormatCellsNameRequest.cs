namespace fusion.geartracker.sheet.request;

public class GoogleLootFormatCellsNameRequest : Request
{
    public GoogleLootFormatCellsNameRequest (GoogleSheetsLootBuilder builder, WCLData data)
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
                        Values = data.PlayersByName.Keys.OrderBy(name => name).ToList().ConvertAll(name => new ConditionValue()
                        {
                            UserEnteredValue = name,
                        }),
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