namespace fusion.geartracker;

internal class Program
{
    private DataService dataService;
    private FusionData data;


    private static async Task Main(string[] args)
    {
        var programConfig = ProgramConfig.Load($"{Directory.GetCurrentDirectory()}/../../appsettings/appsettings.json");
        var dataService = await CreateDataService(programConfig);
        var data = FusionData.Load(programConfig.AppDataPath);
        var program = new Program(dataService, data);

        var reports = programConfig.UseReportCache ? data.ReportsByCode.Values.ToHashSet() : await program.UpdateReports();
        var players = await program.FindPlayers(reports, programConfig);

        if (programConfig.UpdateGear)
        {
            await program.UpdateGear(players, programConfig);
        }

        data.Save(programConfig.AppDataPath);
    }


    private static async Task SetBearerToken (HttpClient httpClient, ProgramConfig programConfig)
    {
        var clientAddress = new Uri(programConfig.BaseAddress);
        var clientId = HttpUtility.UrlEncode(programConfig.ClientId);
        var clientSecret = HttpUtility.UrlEncode(programConfig.ClientSecret);
        var encodedPair = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{clientAddress.Scheme}://{clientAddress.Host}/oauth/token"),
            Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"),
            Headers = { { "Authorization", $"Basic {encodedPair}" } },
        };
        var response = await httpClient.SendAsync(request);
        var contentStream = await response.Content.ReadAsStreamAsync();
        var bearerObject = JsonDocument.Parse(contentStream);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerObject.RootElement.GetProperty("access_token").GetString());
    }


    private static async Task<DataService> CreateDataService (ProgramConfig programConfig)
    {
        var httpClient = new HttpClient();
        var graphQLClient = new WCLGraphQLClient(httpClient);
        var dataService = new DataService(graphQLClient, programConfig);

        httpClient.BaseAddress = new Uri(programConfig.BaseAddress);

        await SetBearerToken(httpClient, programConfig);

        return dataService;
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


    public async Task<List<FusionPlayer>> FindPlayers (HashSet<FusionReport> reports, ProgramConfig programConfig)
    {
        var players = await dataService.GetPlayers(reports, programConfig.PlayersToTrack, programConfig.UseReportCache);

        Console.WriteLine($"Found {players.Count} players");

        players = players.FindAll(player =>
        {
            if (data.ReportCodesByPlayer.TryGetValue(player.Name, out var codes))
            {
                return !codes.Contains(player.Report.Code);
            }
            else
            {
                return true;
            }
        });

        Console.WriteLine($"Found {players.Count} players to check in reports");

        players = players.GetRange(0, programConfig.PlayerCountToUpdate);

        Console.WriteLine($"Updating gear for {players.Count} players");

        return players;
    }


    public async Task UpdateGear (List<FusionPlayer> players, ProgramConfig programConfig)
    {
        var gearSetByPlayer = await dataService.GetGearSetByPlayer(players, programConfig.ItemsToTrack);

        foreach ((var player, var gearSet) in gearSetByPlayer)
        {
            Dictionary<string, FusionGear> playerGearById;

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
                var gearHash = gear.GetHashString();

                if (playerGearById.ContainsKey(gearHash))
                {
                    playerGearById[gearHash] = gear;
                }
                else
                {
                    playerGearById.Add(gearHash, gear);
                }
            }
        }

        foreach (var player in players)
        {
            // Save this report as checked for this player
            if (data.ReportCodesByPlayer.TryGetValue(player.Name, out var codes))
            {
                codes.Add(player.Report.Code);
            }
            else
            {
                data.ReportCodesByPlayer.Add(player.Name, new() { player.Report.Code });
            }
        }
    }


    public Program (DataService dataService, FusionData data)
    {
        this.dataService = dataService;
        this.data = data;
    }
}