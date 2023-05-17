namespace fusion.geartracker.sheet.request;

public class GoogleRecentDateConditionalGradientRequest : GoogleConditionalGradientRequest
{
    public GoogleRecentDateConditionalGradientRequest (GoogleSheetsBuilder builder, int index, List<GridRange> ranges) : base(builder, index, ranges)
    {
        init(new()
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
                Value = "=today()-56",
            },
            Maxpoint = new()
            {
                ColorStyle = new()
                {
                    ThemeColor = "ACCENT3",
                },
                Type = "NUMBER",
                Value = "=today()",
            },
        });
    }
}