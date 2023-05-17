namespace fusion.geartracker.sheet.test;

internal class Program
{
    private ProgramConfig config;
    private GoogleSheetsService sheetsService;
    private WCLData data;

    private const string PlayersTitle = "Players";
    private const string KnownItemsTitle = "Known Items";
    private const string LootDumpTitle = "Loot Dump";

    public List<(string Name, Func<List<WCLPlayer>> GetSortedPlayers)> GeneratedSheets { get; set; }


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
        var program = new Program(programConfig, sheetsService, data);

        var spreadsheet = await sheetsService.ResetSpreadsheet();
        var defaultSheet = spreadsheet.Sheets.ElementAt(0);

        await program.CreateGeneratedSheets(spreadsheet);

        var playersSheet = await sheetsService.CreateSheet(spreadsheet, program.GeneratedSheets.Count, PlayersTitle);
        var knownItemsSheet = await sheetsService.CreateSheet(spreadsheet, program.GeneratedSheets.Count + 1, KnownItemsTitle);
        var lootDumpSheet = await sheetsService.CreateSheet(spreadsheet, program.GeneratedSheets.Count + 2, LootDumpTitle);

        var playersBuilder = new GoogleSheetsPlayersBuilder(spreadsheet, playersSheet);
        var lootBuilder = new GoogleSheetsLootBuilder(spreadsheet, lootDumpSheet);
        var itemsBuilder = new GoogleSheetsItemsBuilder(spreadsheet, knownItemsSheet);

        await program.UpdateSheet(playersBuilder);
        await program.UpdateSheet(lootBuilder);
        await program.UpdateSheet(itemsBuilder);

        await sheetsService.DeleteSheet(spreadsheet, defaultSheet);

        Console.WriteLine($"We're tracking gear for {data.PlayersByName.Count} players.");
    }


    public async Task CreateGeneratedSheets (Spreadsheet spreadsheet)
    {
        var index = 0;

        foreach ((var name, var getSortedPlayers) in GeneratedSheets)
        {
            var sheet = await sheetsService.CreateSheet(spreadsheet, index++, name);

            await UpdateSheet(new GoogleSheetsGeneratedBuilder(spreadsheet, sheet, config.SheetsWeeksOldToIgnore), getSortedPlayers());
        }
    }


    public async Task UpdateSheet (GoogleSheetsPlayersBuilder builder)
    {
        var players = data.PlayersToTrack
            .OrderBy(player => player.Raid)
            .ThenBy(player => player.Class)
            .ThenBy(player => player.Spec)
            .ThenBy(player => player.Name);
        builder.AddHeaders();

        foreach (var player in players)
        {
            builder.AddPlayer(player);
        }

        await sheetsService.UpdateSheet(builder);
        await sheetsService.StyleSheet(builder);
    }


    public async Task UpdateSheet (GoogleSheetsItemsBuilder builder)
    {
        var knownItems = data.KnownItems
            .OrderBy(gear => gear.SlotId)
            .ThenBy(gear => gear.ItemLevel, Comparer<int>.Create((a, b) => b.CompareTo(a)))
            .ThenBy(gear => gear.Name);
        builder.AddHeaders();

        foreach (var gear in knownItems)
        {
            builder.AddItem(gear);
        }

        await sheetsService.UpdateSheet(builder);
        await sheetsService.StyleSheet(builder);
    }


    public async Task UpdateSheet (GoogleSheetsLootBuilder builder)
    {
        builder.AddHeaders();

        await sheetsService.UpdateSheet(builder);
        await sheetsService.StyleSheet(builder);
    }


    public async Task UpdateSheet (GoogleSheetsGeneratedBuilder builder, List<WCLPlayer> players)
    {
        builder.AddHeaders();
        players.ForEach(builder.AddPlayer);

        await sheetsService.UpdateSheet(builder);
        await sheetsService.StyleSheet(builder);
    }


    public Program (ProgramConfig programConfig, GoogleSheetsService sheetsService, WCLData data)
    {
        this.config = programConfig;
        this.sheetsService = sheetsService;
        this.data = data;

        GeneratedSheets = new()
        {
            // { ("By Raid BIS Count", data.PlayersByName.ByRaidBISCount) },
            // { ("By Raid Spec", data.PlayersByName.ByRaidSpec) },
            // { ("By Raid Last Upgrade", data.PlayersByName.ByRaidLastUpgrade) },
            // { ("By Spec", data.PlayersByName.BySpec) },
            // { ("By BIS Count", data.PlayersByName.ByBISCount) },
            { ("By Name", data.PlayersByName.ByName) },
        };
    }
}