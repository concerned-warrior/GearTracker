namespace fusion.geartracker.sheet.request;

public class GoogleConditionalFormatRuleRequest : Request
{
    private GradientRule gradientRule = new()
    {
        Minpoint = new()
        {
            ColorStyle = new()
            {
                RgbColor = new()
                {
                    Red = 0,
                    Green = 0,
                    Blue = 0,
                },
            },
            Type = "NUMBER",
            Value = "=today()-49",
        },
        Maxpoint = new()
        {
            ColorStyle = new()
            {
                ThemeColor = "ACCENT4",
            },
            Type = "NUMBER",
            Value = "=today()",
        },
    };


    private void addConditionalFormatRule (Sheet sheet)
    {
        AddConditionalFormatRule = new()
        {
            Index = 0,
            Rule = new()
            {
                GradientRule = gradientRule,
                Ranges = new List<GridRange>
                {
                    new()
                    {
                        SheetId = sheet.Properties.SheetId,
                    },
                },
            },
        };
    }


    private void updateConditionalFormatRule (Sheet sheet)
    {
        UpdateConditionalFormatRule = new()
        {
            Index = 0,
            Rule = new()
            {
                GradientRule = gradientRule,
                Ranges = new List<GridRange>
                {
                    new()
                    {
                        SheetId = sheet.Properties.SheetId,
                    },
                },
            },
        };
    }


    private bool hasConditionalFormatRule (Spreadsheet spreadsheet, Sheet sheet)
    {
        var conditionalFormatRule = sheet.ConditionalFormats?.ElementAtOrDefault(0);

        return conditionalFormatRule?.GradientRule is not null;
    }


    public GoogleConditionalFormatRuleRequest (Spreadsheet spreadsheet, Sheet sheet)
    {
        if (hasConditionalFormatRule(spreadsheet, sheet))
        {
            updateConditionalFormatRule(sheet);
        }
        else
        {
            addConditionalFormatRule(sheet);
        }
    }
}