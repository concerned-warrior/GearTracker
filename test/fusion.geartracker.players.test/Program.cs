namespace fusion.geartracker.players.test;

internal class Program
{
    private ProgramConfig config;
    private GoogleSheetsService sheetsService;
    private WCLData data;

    private const string PlayersTitle = "Players";


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

        var spreadsheet = await sheetsService.GetSpreadsheet($"'{PlayersTitle}'");
        var sheet = spreadsheet.Sheets.Single(sheet => sheet.Properties.Title.Equals(PlayersTitle));
        var players = program.GetPlayers(sheet);

        Console.WriteLine($"Got {players.Count} players");
        Console.WriteLine($"Currently tracking {data.PlayersToTrack.Count} players");

        program.UpdateTrackedPlayers(players);

        Console.WriteLine($"Now tracking {data.PlayersToTrack.Count} players");

        data.Save(programConfig.AppDataPath);
    }


    public HashSet<WCLPlayer> GetPlayers (Sheet sheet)
    {
        var players = new HashSet<WCLPlayer>();
        var gridData = sheet.Data.First();
        var header = gridData.RowData.First();
        var raidIndex = 0;
        var nameIndex = 0;
        var classIndex = 0;
        var specIndex = 0;

        for (var i = 0; i < header.Values.Count; i++)
        {
            var cell = header.Values[i];

            raidIndex = cell?.FormattedValue?.Trim().Equals("Raid") ?? false ? i : raidIndex;
            nameIndex = cell?.FormattedValue?.Trim().Equals("Name") ?? false ? i : nameIndex;
            classIndex = cell?.FormattedValue?.Trim().Equals("Class") ?? false ? i : classIndex;
            specIndex = cell?.FormattedValue?.Trim().Equals("Spec") ?? false ? i : specIndex;
        }

        for (var i = 1; i < gridData.RowData.Count; i++)
        {
            var row = gridData.RowData[i];

            var raid = row?.Values[raidIndex]?.EffectiveValue?.StringValue?.Trim() ?? string.Empty;
            var name = row?.Values[nameIndex]?.EffectiveValue?.StringValue?.Trim() ?? string.Empty;
            var @class = row?.Values[classIndex]?.EffectiveValue?.StringValue?.Trim() ?? string.Empty;
            var spec = row?.Values[specIndex]?.EffectiveValue?.StringValue?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(raid) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(@class) || string.IsNullOrWhiteSpace(spec))
            {
                continue;
            }

            players.Add(new WCLPlayer
            {
                Raid = raid,
                Name = name,
                Class = @class,
                Spec = spec,
            });
        }

        return players;
    }


    public void UpdateTrackedPlayers (HashSet<WCLPlayer> players)
    {
        data.PlayersToTrack = players;
    }


    public Program (ProgramConfig programConfig, GoogleSheetsService sheetsService, WCLData data)
    {
        this.config = programConfig;
        this.sheetsService = sheetsService;
        this.data = data;
    }
}