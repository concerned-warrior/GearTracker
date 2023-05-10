namespace fusion.geartracker.sheet;

internal class Program
{
    private GoogleSheetsService sheetsService;
    private FusionData data;


    private static async Task Main(string[] args)
    {
        var programConfig = ProgramConfig.Load($"{Directory.GetCurrentDirectory()}/../../appsettings/appsettings.json");
        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets()
        {
            ClientId = programConfig.SheetsClientId,
            ClientSecret = programConfig.SheetsClientSecret,
        }, new[] { SheetsService.Scope.Spreadsheets }, "console", CancellationToken.None);
        var sheetsService = new GoogleSheetsService(programConfig, credential);
        var data = FusionData.Load(programConfig.AppDataPath);
        var program = new Program(sheetsService, data);

        var spreadsheet = await sheetsService.ResetSpreadsheet();
        var sheetByRaidSpec = await sheetsService.CreateSheet(spreadsheet, "By Raid Spec");
        var sheetByRaidLastUpgrade = await sheetsService.CreateSheet(spreadsheet, "By Raid Last Upgrade");
        var sheetBySpec = await sheetsService.CreateSheet(spreadsheet, "By Spec");
        var sheetByName = await sheetsService.CreateSheet(spreadsheet, "By Name");

        await sheetsService.DeleteSheet(spreadsheet, spreadsheet.Sheets.ElementAtOrDefault(0));

        await program.UpdateSheet(new GoogleSheetsBuilder(spreadsheet, sheetByRaidSpec), data.PlayersByName.ByRaidSpec());
        await program.UpdateSheet(new GoogleSheetsBuilder(spreadsheet, sheetByRaidLastUpgrade), data.PlayersByName.ByRaidLastUpgrade());
        await program.UpdateSheet(new GoogleSheetsBuilder(spreadsheet, sheetBySpec), data.PlayersByName.BySpec());
        await program.UpdateSheet(new GoogleSheetsBuilder(spreadsheet, sheetByName), data.PlayersByName.ByName());

        Console.WriteLine($"We're tracking gear for {data.PlayersByName.Count} players.");
    }


    public async Task UpdateSheet (GoogleSheetsBuilder builder, List<FusionPlayer> players)
    {
        builder.AddHeaders();
        players.ForEach(builder.AddPlayer);

        await sheetsService.UpdateSheet(builder);
        await sheetsService.StyleSheet(builder);
    }


    public Program (GoogleSheetsService sheetsService, FusionData data)
    {
        this.sheetsService = sheetsService;
        this.data = data;
    }
}