namespace fusion.geartracker.sheet.test;

internal class Program
{
    private GoogleSheetsService sheetsService;
    private WCLData data;


    private static async Task Main(string[] args)
    {
        var programConfig = ProgramConfig.Load();
        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets()
        {
            ClientId = programConfig.SheetsClientId,
            ClientSecret = programConfig.SheetsClientSecret,
        }, new[] { SheetsService.Scope.Spreadsheets }, "console", CancellationToken.None);
        var sheetsService = new GoogleSheetsService(programConfig, credential);
        var data = WCLData.Load(programConfig.AppDataPath);
        var program = new Program(sheetsService, data);

        var spreadsheet = await sheetsService.ResetSpreadsheet();
        var sheetByRaidSpec = await sheetsService.CreateSheet(spreadsheet, "By Raid Spec");
        var sheetByRaidLastUpgrade = await sheetsService.CreateSheet(spreadsheet, "By Raid Last Upgrade");
        var sheetBySpec = await sheetsService.CreateSheet(spreadsheet, "By Spec");
        var sheetByName = await sheetsService.CreateSheet(spreadsheet, "By Name");

        await sheetsService.DeleteSheet(spreadsheet, spreadsheet.Sheets.ElementAtOrDefault(0));

        var playersByRaidSpec = data.PlayersByName.ByRaidSpec();
        var playersByRaidLastUpgrade = data.PlayersByName.ByRaidLastUpgrade();
        var playersBySpec = data.PlayersByName.BySpec();
        var playersByName = data.PlayersByName.ByName();

        Console.WriteLine($"ByRaidSpec: {playersByRaidSpec.Count}");
        Console.WriteLine($"ByRaidLastUpgrade: {playersByRaidLastUpgrade.Count}");
        Console.WriteLine($"BySpec: {playersBySpec.Count}");
        Console.WriteLine($"ByName: {playersByName.Count}");

        await program.UpdateSheet(new GoogleSheetsBuilder(spreadsheet, sheetByRaidSpec), playersByRaidSpec);
        await program.UpdateSheet(new GoogleSheetsBuilder(spreadsheet, sheetByRaidLastUpgrade), playersByRaidLastUpgrade);
        await program.UpdateSheet(new GoogleSheetsBuilder(spreadsheet, sheetBySpec), playersBySpec);
        await program.UpdateSheet(new GoogleSheetsBuilder(spreadsheet, sheetByName), playersByName);

        Console.WriteLine($"We're tracking gear for {data.PlayersByName.Count} players.");
    }


    public async Task UpdateSheet (GoogleSheetsBuilder builder, List<WCLPlayer> players)
    {
        builder.AddHeaders();
        players.ForEach(builder.AddPlayer);

        await sheetsService.UpdateSheet(builder);
        await sheetsService.StyleSheet(builder);
    }


    public Program (GoogleSheetsService sheetsService, WCLData data)
    {
        this.sheetsService = sheetsService;
        this.data = data;
    }
}