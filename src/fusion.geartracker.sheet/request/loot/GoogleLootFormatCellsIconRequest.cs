namespace fusion.geartracker.sheet.request;

public class GoogleLootFormatCellsIconRequest : Request
{
    public GoogleLootFormatCellsIconRequest (GoogleSheetsLootBuilder builder)
    {
        var columnItemName = builder.GetColumnOffset(builder.ItemNameColumnIndex);

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
                    FormulaValue = $"=xlookup({columnItemName}2, 'Known Items'!$E$2:$E, 'Known Items'!$D$2:$D, \"\", 0, 1)",
                },
            },
            Range = new()
            {
                StartColumnIndex = builder.IconColumnIndex,
                EndColumnIndex = builder.IconColumnIndex + 1,
                StartRowIndex = 1,
                SheetId = builder.Sheet.Properties.SheetId,
            }
        };
    }
}