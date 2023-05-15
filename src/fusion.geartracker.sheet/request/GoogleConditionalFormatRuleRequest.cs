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
            Value = "=today()-28",
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


    private void addConditionalFormatRule (GoogleSheetsBuilder builder)
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
                        SheetId = builder.Sheet.Properties.SheetId,
                    },
                },
            },
        };
    }


    private void updateConditionalFormatRule (GoogleSheetsBuilder builder)
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
                        SheetId = builder.Sheet.Properties.SheetId,
                    },
                },
            },
        };
    }


    private bool hasConditionalFormatRule (GoogleSheetsBuilder builder)
    {
        var conditionalFormatRule = builder.Sheet.ConditionalFormats?.ElementAtOrDefault(0);

        return conditionalFormatRule?.GradientRule is not null;
    }


    public GoogleConditionalFormatRuleRequest (GoogleSheetsBuilder builder)
    {
        if (hasConditionalFormatRule(builder))
        {
            updateConditionalFormatRule(builder);
        }
        else
        {
            addConditionalFormatRule(builder);
        }
    }
}