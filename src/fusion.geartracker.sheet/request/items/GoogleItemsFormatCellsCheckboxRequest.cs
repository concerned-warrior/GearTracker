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
                        StartColumnIndex = builder.IgnoreColumnIndex,
                        EndColumnIndex = builder.IgnoreColumnIndex + 1,
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
                        StartColumnIndex = builder.InstanceSizeColumnIndex,
                        EndColumnIndex = builder.InstanceSizeColumnIndex + 1,
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
                        StartColumnIndex = builder.IsBISColumnIndex,
                        EndColumnIndex = builder.IsBISColumnIndex + 1,
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