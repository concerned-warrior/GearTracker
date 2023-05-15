namespace fusion.geartracker.sheet.request;

public class GoogleItemsFormatCellsCheckboxRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsItemsBuilder builder)
    {
        return new()
        {
            new()
            {
                RepeatCell = new()
                {
                    Fields = "dataValidation.Condition",
                    Cell = new()
                    {
                        DataValidation = new()
                        {
                            Condition = new()
                            {
                                Type = "BOOLEAN",
                            },
                        },
                    },
                    Range = new()
                    {
                        StartColumnIndex = 1,
                        EndColumnIndex = 2,
                        StartRowIndex = 1,
                        SheetId = builder.Sheet.Properties.SheetId,
                    }
                },
            },
            new()
            {
                RepeatCell = new()
                {
                    Fields = "dataValidation.Condition",
                    Cell = new()
                    {
                        DataValidation = new()
                        {
                            Condition = new()
                            {
                                Type = "BOOLEAN",
                            },
                        },
                    },
                    Range = new()
                    {
                        StartColumnIndex = 6,
                        EndColumnIndex = 7,
                        StartRowIndex = 1,
                        SheetId = builder.Sheet.Properties.SheetId,
                    }
                },
            },
            new()
            {
                RepeatCell = new()
                {
                    Fields = "dataValidation.Condition",
                    Cell = new()
                    {
                        DataValidation = new()
                        {
                            Condition = new()
                            {
                                Type = "BOOLEAN",
                            },
                        },
                    },
                    Range = new()
                    {
                        StartColumnIndex = 7,
                        EndColumnIndex = 8,
                        StartRowIndex = 1,
                        SheetId = builder.Sheet.Properties.SheetId,
                    }
                },
            },
        };
    }


    private GoogleItemsFormatCellsCheckboxRequest ()
    {

    }
}