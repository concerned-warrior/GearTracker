namespace fusion.geartracker.sheet;

public class GoogleSheetsService
{
    private ProgramConfig programConfig;
    private SheetsService service;


    public async Task<Spreadsheet> GetSpreadsheet ()
    {
        var request = new SpreadsheetsResource.GetRequest(service, programConfig.SheetsSpreadsheetId);

        return await request.ExecuteAsync();
    }


    public async Task StyleSheet (Spreadsheet spreadsheet, GoogleSheetsBuilder builder)
    {
        var requests = new List<Request>();
        var request = new SpreadsheetsResource.BatchUpdateRequest(service, new()
        {
            Requests = requests,
        }, programConfig.SheetsSpreadsheetId);
        var sheet = spreadsheet.Sheets.ElementAt(0);

        requests.AddRange(new List<Request>
        {
            new GoogleUpdateSpreadsheetPropertiesRequest(spreadsheet),
            new GoogleConditionalFormatRuleRequest(spreadsheet, sheet),
            new GoogleFreezeCellsRequest(spreadsheet, sheet),
            new GoogleAutoResizeRequest(spreadsheet, sheet),
            new GoogleCenterHeaderTextRequest(spreadsheet, sheet),
            new GoogleFormatSheetRequest(spreadsheet, sheet),
        });
        requests.AddRange(GoogleMergeCellsLeftRequest.CreateRequests(builder, sheet));
        requests.AddRange(GoogleFormatCellsLeftRequest.CreateRequests(builder, sheet));
        requests.AddRange(GoogleMergeCellsHeaderRequest.CreateRequests(builder, sheet));
        requests.AddRange(GoogleFormatCellsHeaderRequest.CreateRequests(builder, sheet));
        requests.AddRange(GoogleFormatCellsIconRequest.CreateRequests(builder, sheet));

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