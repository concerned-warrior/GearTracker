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
        var players = await program.FindPlayers(reports);
        var gear = await program.UpdateGear(players);

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
            if (!data.ReportsByCode.ContainsKey(report.Code))
            {
                data.ReportsByCode.Add(report.Code, report);
            }
        }

        return reports;
    }


    public async Task<List<FusionPlayer>> FindPlayers (HashSet<FusionReport> reports)
    {
        return await dataService.GetPlayers(reports, data.PlayersToTrack);
    }


    public async Task<Dictionary<FusionPlayer, HashSet<FusionGear>>> UpdateGear (List<FusionPlayer> players)
    {
        var gearSetByPlayer = await dataService.GetGearSetByPlayer(players, data.ItemsToTrack);

        foreach ((var player, var gearSet) in gearSetByPlayer)
        {
            foreach (var gear in gearSet)
            {
                Console.WriteLine($"{gear.FirstSeenAt} - {player} acquired {gear}");
            }
        }

        return gearSetByPlayer;
    }


    public Program (DataService dataService, FusionData data)
    {
        this.dataService = dataService;
        this.data = data;
    }
}