namespace fusion.geartracker.sheet;

public class GoogleSheetsService
{
    private ProgramConfig programConfig;
    private SheetsService service;


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


    public async Task StyleSheet (GoogleSheetsLootBuilder builder, WCLData data)
    {
        var requests = new List<Request>();
        var request = new SpreadsheetsResource.BatchUpdateRequest(service, new()
        {
            Requests = requests,
        }, programConfig.SheetsSpreadsheetId);

        requests.AddRange(new List<Request>
        {
            new GoogleUpdateSpreadsheetPropertiesRequest(builder),
            new GoogleLootFormatCellsDateRequest(builder),
            new GoogleLootFormatCellsHeaderRequest(builder),
            new GoogleLootFormatCellsIconRequest(builder),
            new GoogleLootFormatCellsItemIdRequest(builder),
            new GoogleLootFormatCellsItemRequest(builder),
            new GoogleLootFormatCellsNameRequest(builder, data),
            new GoogleFormatSheetRequest(builder),
        });
        requests.AddRange(GoogleLootResizeRequest.CreateRequests(builder));

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
            new GoogleItemsFormatCellsHeaderRequest(builder),
            new GoogleItemsFormatCellsIconRequest(builder),
            new GoogleItemsFormatCellsUpgradeRequest(builder),
            new GoogleAutoResizeRequest(builder),
            new GoogleFormatSheetRequest(builder),
        });
        requests.AddRange(GoogleItemsFormatCellsCheckboxRequest.CreateRequests(builder));

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
            new GoogleConditionalFormatRuleRequest(builder),
            new GooglePlayersFreezeCellsRequest(builder),
            new GoogleAutoResizeRequest(builder),
            new GooglePlayersCenterHeaderTextRequest(builder),
            new GoogleFormatSheetRequest(builder),
        });
        requests.AddRange(GooglePlayersMergeCellsLeftRequest.CreateRequests(builder));
        requests.AddRange(GooglePlayersFormatCellsLeftRequest.CreateRequests(builder));
        requests.AddRange(GooglePlayersMergeCellsHeaderRequest.CreateRequests(builder));
        requests.AddRange(GooglePlayersFormatCellsHeaderRequest.CreateRequests(builder));
        requests.AddRange(GooglePlayersFormatCellsIconRequest.CreateRequests(builder));

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