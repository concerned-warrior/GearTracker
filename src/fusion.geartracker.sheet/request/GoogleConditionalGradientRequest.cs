namespace fusion.geartracker.sheet.request;

public abstract class GoogleConditionalGradientRequest : Request
{
    private GoogleSheetsBuilder builder;
    private int index;
    private List<GridRange> ranges;


    private void addConditionalFormatRule (GradientRule gradientRule)
    {
        AddConditionalFormatRule = new()
        {
            Index = index,
            Rule = new()
            {
                GradientRule = gradientRule,
                Ranges = ranges.ConvertAll(range =>
                {
                   range.SheetId = builder.Sheet.Properties.SheetId;

                   return range;
                }),
            },
        };
    }


    private void updateConditionalFormatRule (GradientRule gradientRule)
    {
        UpdateConditionalFormatRule = new()
        {
            Index = index,
            Rule = new()
            {
                GradientRule = gradientRule,
                Ranges = ranges.ConvertAll(range =>
                {
                   range.SheetId = builder.Sheet.Properties.SheetId;

                   return range;
                }),
            },
        };
    }


    private bool hasConditionalFormatRule ()
    {
        var conditionalFormatRule = builder.Sheet.ConditionalFormats?.ElementAtOrDefault(index);

        return conditionalFormatRule?.GradientRule is not null;
    }


    protected void init (GradientRule gradientRule)
    {
        if (hasConditionalFormatRule())
        {
            updateConditionalFormatRule(gradientRule);
        }
        else
        {
            addConditionalFormatRule(gradientRule);
        }
    }


    public GoogleConditionalGradientRequest (GoogleSheetsBuilder builder, int index, List<GridRange> ranges)
    {
        this.builder = builder;
        this.index = index;
        this.ranges = ranges;
    }
}