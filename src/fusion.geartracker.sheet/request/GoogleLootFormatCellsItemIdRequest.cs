namespace fusion.geartracker.sheet.request;

public class GoogleLootFormatCellsItemIdRequest : Request
{
    public GoogleLootFormatCellsItemIdRequest (GoogleSheetsLootBuilder builder)
    {
        RepeatCell = new()
        {
            Fields = "userEnteredValue.formulaValue",
            Cell = new()
            {
                UserEnteredValue = new()
                {
                    FormulaValue = "=ifna(lookup(C2, 'Known Items'!$E$2:$E, 'Known Items'!$A$2:$A),\"\")",
                },
            },
            Range = new()
            {
                StartColumnIndex = 0,
                EndColumnIndex = 1,
                StartRowIndex = 1,
                SheetId = builder.Sheet.Properties.SheetId,
            }
        };
    }
}