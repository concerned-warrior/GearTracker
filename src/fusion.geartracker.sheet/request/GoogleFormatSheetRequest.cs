namespace fusion.geartracker.sheet.request;

public class GoogleFormatSheetRequest : Request
{
    public GoogleFormatSheetRequest (Spreadsheet spreadsheet, Sheet sheet)
    {
        RepeatCell = new()
        {
            Fields = "userEnteredFormat.borders,userEnteredFormat.backgroundColorStyle,userEnteredFormat.textFormat.foregroundColorStyle",
            Cell = new()
            {
                UserEnteredFormat = new()
                {
                    Borders = new()
                    {
                        Bottom = new()
                        {
                            Style = "SOLID",
                            ColorStyle = new()
                            {
                                RgbColor = new()
                                {
                                    Red = 0.2f,
                                    Green = 0.2f,
                                    Blue = 0.2f,
                                },
                            },
                        },
                    },
                    BackgroundColorStyle = new()
                    {
                        RgbColor = new()
                        {
                            Red = 0,
                            Green = 0,
                            Blue = 0,
                        },
                    },
                    TextFormat = new()
                    {
                        ForegroundColorStyle = new()
                        {
                            RgbColor = new()
                            {
                                Red = 1,
                                Green = 1,
                                Blue = 1,
                            },
                        },
                    },
                },
            },
            Range = new()
            {
                SheetId = sheet.Properties.SheetId,
            },
        };
    }
}