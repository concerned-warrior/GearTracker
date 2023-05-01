internal class Program
{
    private DataService dataService;
    private FusionData data;


    private static async Task Main(string[] args)
    {
        var programConfig = CreateProgramConfig("./appsettings/appsettings.json");
        var dataService = CreateDataService(programConfig);
        var data = dataService.Load();
        var program = new Program(dataService, data);

        var reports = await program.UpdateReports();
        var players = await program.FindPlayers(reports, programConfig.PlayersToTrack);

        await program.UpdateGear(players, programConfig.ItemsToTrack);

        dataService.Save(data);
    }


    private static DataService CreateDataService (ProgramConfig programConfig)
    {
        var httpClient = new HttpClient();
        var graphQLClient = new WCLGraphQLClient(httpClient);
        var dataService = new DataService(graphQLClient, programConfig);

        httpClient.BaseAddress = new Uri(programConfig.BaseAddress);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", programConfig.BearerToken);

        return dataService;
    }


    private static ProgramConfig CreateProgramConfig (string appSettingsPath)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: false)
            .Build();

        var programConfig = new ProgramConfig();

        config.Bind(programConfig);

        return programConfig;
    }


    public async Task<HashSet<FusionReport>> UpdateReports ()
    {
        var reports = await dataService.GetReports();

        foreach (var report in reports)
        {
            if (data.ReportsByCode.ContainsKey(report.Code))
            {
                data.ReportsByCode[report.Code] = report;
            }
            else
            {
                data.ReportsByCode.Add(report.Code, report);
            }
        }

        return reports;
    }


    public async Task<List<FusionPlayer>> FindPlayers (HashSet<FusionReport> reports, HashSet<string> playersToTrack)
    {
        var players = await dataService.GetPlayers(reports, playersToTrack);

        return players.FindAll(player =>
        {
            if (data.ReportCodesByPlayer.TryGetValue(player.Name, out var codes))
            {
                var result = !codes.Contains(player.Report.Code);

                codes.Add(player.Report.Code);

                return result;
            }
            else
            {
                data.ReportCodesByPlayer.Add(player.Name, new() { player.Report.Code });

                return true;
            }
        });
    }


    public async Task UpdateGear (List<FusionPlayer> players, HashSet<int> itemsToTrack)
    {
        var gearSetByPlayer = await dataService.GetGearSetByPlayer(players, itemsToTrack);

        foreach ((var player, var gearSet) in gearSetByPlayer)
        {
            Dictionary<int, FusionGear> playerGearById;

            // Get current player gear, if any
            if (data.PlayersByName.TryGetValue(player.Name, out var playerData))
            {
                playerGearById = playerData.GearById;
            }
            else
            {
                data.PlayersByName.Add(player.Name, player);

                playerGearById = player.GearById;
            }

            // Update player gear
            foreach (var gear in gearSet)
            {
                Console.WriteLine($"{gear.FirstSeenAt} - {player} acquired {gear}");

                if (playerGearById.ContainsKey(gear.Id))
                {
                    playerGearById[gear.Id] = gear;
                }
                else
                {
                    playerGearById.Add(gear.Id, gear);
                }
            }
        }
    }


    public Program (DataService dataService, FusionData data)
    {
        this.dataService = dataService;
        this.data = data;
    }
}