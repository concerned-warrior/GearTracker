namespace fusion.geartracker.sheet.request;

public class GoogleLootResizeRequest : Request
{
    public static List<Request> CreateRequests (GoogleSheetsLootBuilder builder)
    {
        return new()
        {
            new()
            {
                UpdateDimensionProperties = new()
                {
                    Fields = "pixelSize",
                    Properties = new()
                    {
                        PixelSize = 74,
                    },
                    Range = new()
                    {
                        Dimension = "COLUMNS",
                        StartIndex = 0,
                        EndIndex = 1,
                        SheetId = builder.Sheet.Properties.SheetId,
                    }
                },
            },
            new()
            {
                UpdateDimensionProperties = new()
                {
                    Fields = "pixelSize",
                    Properties = new()
                    {
                        PixelSize = 44,
                    },
                    Range = new()
                    {
                        Dimension = "COLUMNS",
                        StartIndex = 1,
                        EndIndex = 2,
                        SheetId = builder.Sheet.Properties.SheetId,
                    }
                },
            },
            new()
            {
                UpdateDimensionProperties = new()
                {
                    Fields = "pixelSize",
                    Properties = new()
                    {
                        PixelSize = 400,
                    },
                    Range = new()
                    {
                        Dimension = "COLUMNS",
                        StartIndex = 2,
                        EndIndex = 3,
                        SheetId = builder.Sheet.Properties.SheetId,
                    }
                },
            },
            new()
            {
                UpdateDimensionProperties = new()
                {
                    Fields = "pixelSize",
                    Properties = new()
                    {
                        PixelSize = 120,
                    },
                    Range = new()
                    {
                        Dimension = "COLUMNS",
                        StartIndex = 3,
                        EndIndex = 4,
                        SheetId = builder.Sheet.Properties.SheetId,
                    }
                },
            },
            new()
            {
                UpdateDimensionProperties = new()
                {
                    Fields = "pixelSize",
                    Properties = new()
                    {
                        PixelSize = 132,
                    },
                    Range = new()
                    {
                        Dimension = "COLUMNS",
                        StartIndex = 4,
                        EndIndex = 5,
                        SheetId = builder.Sheet.Properties.SheetId,
                    }
                },
            },
        };
    }


    private GoogleLootResizeRequest ()
    {

    }
}