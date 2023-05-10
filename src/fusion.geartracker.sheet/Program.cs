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

        var spreadsheet = await sheetsService.GetSpreadsheet();
        var builder = await program.UpdateSheet();
        await sheetsService.StyleSheet(spreadsheet, builder);

        Console.WriteLine($"We're tracking gear for {data.PlayersByName.Count} players.");
    }


    public async Task<GoogleSheetsBuilder> UpdateSheet ()
    {
        var builder = new GoogleSheetsBuilder();
        var players = data.PlayersByName.Values.ToList();

        builder.AddHeaders();

        players.Sort((a, b) =>
        {
            var sortStr = (FusionPlayer p) => $"{p.Raid}{p.Class}{p.Spec}";

            return sortStr(a).CompareTo(sortStr(b));
        });

        foreach (var player in players)
        {
            builder.AddPlayer(player);
        }

        await sheetsService.UpdateSheet(builder);

        return builder;
    }


    public Program (GoogleSheetsService sheetsService, FusionData data)
    {
        this.sheetsService = sheetsService;
        this.data = data;
    }
}