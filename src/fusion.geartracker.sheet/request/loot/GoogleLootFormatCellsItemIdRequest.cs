namespace fusion.geartracker.sheet.request;

public class GoogleLootFormatCellsItemIdRequest : Request
{
    public GoogleLootFormatCellsItemIdRequest (GoogleSheetsLootBuilder builder)
    {
        var columnItemName = builder.GetColumnOffset(builder.ItemNameColumnIndex);

        RepeatCell = new()
        {
            Fields = "userEnteredValue.formulaValue",
            Cell = new()
            {
                UserEnteredValue = new()
                {
                    FormulaValue = $"=xlookup({columnItemName}2, 'Known Items'!$E$2:$E, 'Known Items'!$A$2:$A, \"\", 0, 1)",
                },
            },
            Range = new()
            {
                StartColumnIndex = builder.ItemIdColumnIndex,
                EndColumnIndex = builder.ItemIdColumnIndex + 1,
                StartRowIndex = 1,
                SheetId = builder.Sheet.Properties.SheetId,
            }
        };
    }
}