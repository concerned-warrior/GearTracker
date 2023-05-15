namespace fusion.geartracker.sheet.request;

public class GoogleLootFormatCellsIconRequest : Request
{
    public GoogleLootFormatCellsIconRequest (GoogleSheetsLootBuilder builder)
    {
        RepeatCell = new()
        {
            Fields = "userEnteredFormat.horizontalAlignment,userEnteredValue.formulaValue",
            Cell = new()
            {
                UserEnteredFormat = new()
                {
                    HorizontalAlignment = "CENTER",
                },
                UserEnteredValue = new()
                {
                    FormulaValue = "=ifna(lookup(C2, 'Known Items'!$E$2:$E, 'Known Items'!$D$2:$D),\"\")",
                },
            },
            Range = new()
            {
                StartColumnIndex = 1,
                EndColumnIndex = 2,
                StartRowIndex = 1,
                SheetId = builder.Sheet.Properties.SheetId,
            }
        };
    }
}