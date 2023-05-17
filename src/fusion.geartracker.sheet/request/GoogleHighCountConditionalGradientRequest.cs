namespace fusion.geartracker.sheet.request;

public class GoogleHighCountConditionalGradientRequest : GoogleConditionalGradientRequest
{
    public GoogleHighCountConditionalGradientRequest (GoogleSheetsBuilder builder, int index, List<GridRange> ranges) : base(builder, index, ranges)
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
                Value = "0",
            },
            Maxpoint = new()
            {
                ColorStyle = new()
                {
                    ThemeColor = "ACCENT2",
                },
                Type = "NUMBER",
                Value = "17",
            },
        });
    }
}