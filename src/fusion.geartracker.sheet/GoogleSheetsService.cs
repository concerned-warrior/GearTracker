namespace fusion.geartracker.sheet;

public class GoogleSheetsService
{
    private ProgramConfig programConfig;
    private SheetsService service;


    public async Task<Spreadsheet> GetSpreadsheet ()
    {
        var requestSpreadsheet = new SpreadsheetsResource.GetRequest(service, programConfig.SheetsSpreadsheetId)
        {
            IncludeGridData = true,
        };

        return await requestSpreadsheet.ExecuteAsync();
    }


    public async Task<Spreadsheet> ResetSpreadsheet ()
    {
        var requestSpreadsheet = new SpreadsheetsResource.GetRequest(service, programConfig.SheetsSpreadsheetId);
        var spreadsheet = await requestSpreadsheet.ExecuteAsync();
        var requests = new List<Request>();
        var requestDeleteSheets = new SpreadsheetsResource.BatchUpdateRequest(service, new()
        {
            IncludeSpreadsheetInResponse = true,
            Requests = requests,
        }, spreadsheet.SpreadsheetId);

        requests.Add(new()
        {
            AddSheet = new(),
        });
        requests.AddRange(spreadsheet.Sheets.ToList().ConvertAll(sheet => new Request
        {
            DeleteSheet = new()
            {
                SheetId = sheet.Properties.SheetId,
            },
        }));

        var response = await requestDeleteSheets.ExecuteAsync();

        return response.UpdatedSpreadsheet;
    }


    public async Task<Sheet> CreateSheet (Spreadsheet spreadsheet, int index, string title)
    {
        var request = new SpreadsheetsResource.BatchUpdateRequest(service, new()
        {
            IncludeSpreadsheetInResponse = true,
            Requests = new List<Request>
            {
                new()
                {
                    AddSheet = new()
                    {
                        Properties = new()
                        {
                            Index = index,
                            Title = title,
                        },
                    },
                },
            },
        }, spreadsheet.SpreadsheetId);

        var response = await request.ExecuteAsync();
        var result = new Sheet();

        foreach (var sheet in response.UpdatedSpreadsheet.Sheets)
        {
            if (sheet.Properties.Title.Equals(title)) result = sheet;
        }

        return result;
    }


    public async Task DeleteSheet (Spreadsheet spreadsheet, Sheet? sheet)
    {
        if (sheet is null) return;

        var request = new SpreadsheetsResource.BatchUpdateRequest(service, new()
        {
            Requests = new List<Request>
            {
                new()
                {
                    DeleteSheet = new()
                    {
                        SheetId = sheet.Properties.SheetId,
                    },
                },
            },
        }, spreadsheet.SpreadsheetId);

        await request.ExecuteAsync();
    }


    public async Task StyleSheet (GoogleSheetsPlayersBuilder builder)
    {
        var requests = new List<Request>();
        var request = new SpreadsheetsResource.BatchUpdateRequest(service, new()
        {
            Requests = requests,
        }, programConfig.SheetsSpreadsheetId);

        requests.AddRange(new List<Request>
        {
            new GoogleUpdateSpreadsheetPropertiesRequest(builder),
            new GoogleFormatSheetRequest(builder),
            new GooglePlayersFreezeCellsRequest(builder),
            new GooglePlayersFormatCellsHeaderRequest(builder),
            new GoogleAutoResizeRequest(builder),
        });

        await request.ExecuteAsync();
    }


    public async Task StyleSheet (GoogleSheetsItemsBuilder builder)
    {
        var requests = new List<Request>();
        var request = new SpreadsheetsResource.BatchUpdateRequest(service, new()
        {
            Requests = requests,
        }, programConfig.SheetsSpreadsheetId);

        requests.AddRange(new List<Request>
        {
            new GoogleUpdateSpreadsheetPropertiesRequest(builder),
            new GoogleFormatSheetRequest(builder),
            new GoogleItemsFreezeCellsRequest(builder),
            new GoogleItemsFormatCellsHeaderRequest(builder),
            new GoogleItemsFormatCellsIconRequest(builder),
            new GoogleItemsFormatCellsUpgradeRequest(builder),
            new GoogleAutoResizeRequest(builder),
        });
        requests.AddRange(GoogleItemsFormatCellsCheckboxRequest.CreateRequests(builder));

        await request.ExecuteAsync();
    }


    public async Task StyleSheet (GoogleSheetsLootBuilder builder)
    {
        var requests = new List<Request>();
        var request = new SpreadsheetsResource.BatchUpdateRequest(service, new()
        {
            Requests = requests,
        }, programConfig.SheetsSpreadsheetId);

        requests.AddRange(new List<Request>
        {
            new GoogleUpdateSpreadsheetPropertiesRequest(builder),
            new GoogleFormatSheetRequest(builder),
            new GoogleLootFreezeCellsRequest(builder),
            new GoogleLootFormatCellsDateRequest(builder),
            new GoogleLootFormatCellsHeaderRequest(builder),
            new GoogleLootFormatCellsIconRequest(builder),
            new GoogleLootFormatCellsItemIdRequest(builder),
            new GoogleLootFormatCellsItemRequest(builder),
            new GoogleLootFormatCellsNameRequest(builder),
        });
        requests.AddRange(GoogleLootResizeRequest.CreateRequests(builder));

        await request.ExecuteAsync();
    }


    public async Task StyleSheet (GoogleSheetsGeneratedBuilder builder)
    {
        var requests = new List<Request>();
        var request = new SpreadsheetsResource.BatchUpdateRequest(service, new()
        {
            Requests = requests,
        }, programConfig.SheetsSpreadsheetId);

        requests.AddRange(new List<Request>
        {
            new GoogleUpdateSpreadsheetPropertiesRequest(builder),
            new GoogleFormatSheetRequest(builder),
            new GoogleRecentWeekConditionalGradientRequest(builder, 0, new() { new()
            {
                StartColumnIndex = builder.Last10ColumnIndex,
                EndColumnIndex = builder.LastBISColumnIndex + 1,
                StartRowIndex = builder.DataStartRowIndex,
                SheetId = builder.Sheet.Properties.SheetId,
            }}),
            new GoogleHighCountConditionalGradientRequest(builder, 1, new() { new()
            {
                StartColumnIndex = builder.IsBISColumnIndex,
                EndColumnIndex = builder.IsBISColumnIndex + 1,
                StartRowIndex = builder.DataStartRowIndex,
                SheetId = builder.Sheet.Properties.SheetId,
            }}),
            new GoogleRecentDateConditionalGradientRequest(builder, 2, new() { new() { SheetId = builder.Sheet.Properties.SheetId } }),
            new GoogleGeneratedFreezeCellsRequest(builder),
            new GoogleGeneratedCenterHeaderTextRequest(builder),
            new GoogleGeneratedCenterSpecRequest(builder),
            new GoogleAutoResizeRequest(builder),
        });
        requests.AddRange(GoogleGeneratedMergeCellsLeftRequest.CreateRequests(builder));
        requests.AddRange(GoogleGeneratedFormatCellsDataRequest.CreateRequests(builder));
        requests.AddRange(GoogleGeneratedMergeCellsHeaderRequest.CreateRequests(builder));
        requests.AddRange(GoogleGeneratedFormatCellsHeaderRequest.CreateRequests(builder));
        requests.AddRange(GoogleGeneratedFormatCellsIconRequest.CreateRequests(builder));

        await request.ExecuteAsync();
    }


    public async Task UpdateSheet (GoogleSheetsBuilder builder)
    {
        var request = new SpreadsheetsResource.ValuesResource.UpdateRequest(service, new()
        {
            MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.ROWS.ToString(),
            Values = builder.GetData(),
        }, programConfig.SheetsSpreadsheetId, builder.GetA1Range());

        request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

        await request.ExecuteAsync();
    }


    public GoogleSheetsService (ProgramConfig programConfig, UserCredential credential)
    {
        this.programConfig = programConfig;
        this.service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
        });
    }
}