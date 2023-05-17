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
                        StartIndex = builder.ItemIdColumnIndex,
                        EndIndex = builder.ItemIdColumnIndex + 1,
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
                        StartIndex = builder.IconColumnIndex,
                        EndIndex = builder.IconColumnIndex + 1,
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
                        StartIndex = builder.ItemNameColumnIndex,
                        EndIndex = builder.ItemNameColumnIndex + 1,
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
                        StartIndex = builder.PlayerNameColumnIndex,
                        EndIndex = builder.PlayerNameColumnIndex + 1,
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
                        StartIndex = builder.DateColumnIndex,
                        EndIndex = builder.DateColumnIndex + 1,
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