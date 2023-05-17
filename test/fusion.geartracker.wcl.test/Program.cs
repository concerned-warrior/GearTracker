namespace fusion.geartracker.wcl.test;

internal class Program
{
    private ProgramConfig config;
    private WCLService wclService;
    private WCLData data;


    private static async Task Main(string[] args)
    {
        var programConfig = ProgramConfig.Load();
        var data = WCLData.Load(programConfig.AppDataPath);
        var wclService = await CreateWCLService(programConfig, data);
        var program = new Program(programConfig, wclService, data);

        var reports = await program.GetReports();
        var players = await program.GetPlayers(reports);

        await program.UpdateItemCache(players);

        program.SaveReports(players);

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


    private static async Task<WCLService> CreateWCLService (ProgramConfig programConfig, WCLData data)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(programConfig.BaseAddress),
        };
        var graphQLClient = new WCLGraphQLClient(httpClient);

        await SetBearerToken(httpClient, programConfig);

        return new WCLService(graphQLClient);
    }


    public async Task<List<WCLReport>> GetReports ()
    {
        Console.WriteLine($"Getting reports between {config.OldestReportDate.ToLongDateString()} and {config.NewestReportDate.ToLongDateString()}");

        var reportsPulled = await wclService.GetReports(config.GuildId, config.NewestReportDate, config.OldestReportDate);
        var validReportsPulled = reportsPulled.FindAll(report =>
        {
            var result = !config.ReportBlacklist.Contains(report.Code);

            if (result && config.ValidReportDateByZoneId.TryGetValue(report.Zone.Id, out var dateValid))
            {
                result = report.StartTime > dateValid;
            }
            else
            {
                result = false;
            }

            return result;
        });
        var newReports = validReportsPulled.FindAll(report => !data.ReportsByCode.ContainsKey(report.Code));
        var reportsToUpdate = newReports.GetRange(0, config.ReportCountToUpdate < newReports.Count ? config.ReportCountToUpdate : newReports.Count);

        Console.WriteLine($"Pulled {validReportsPulled.Count}/{reportsPulled.Count} valid reports");
        Console.WriteLine($"Updating {reportsToUpdate.Count}/{newReports.Count} new reports");

        return reportsToUpdate;
    }


    public async Task<List<WCLPlayer>> GetPlayers (List<WCLReport> reports)
    {
        Console.WriteLine($"Adding player information to reports. This may take a while...");

        await wclService.AddPlayerInfoToReports(reports);

        return WCLReport.GetPlayers(reports, data.PlayersToTrack);
    }


    public async Task UpdateItemCache (List<WCLPlayer> players)
    {
        var gearSet = new HashSet<WCLGear>();

        foreach (var player in players)
        {
            foreach ((var id, var gear) in player.GearById)
            {
                if (data.KnownItems.TryGetValue(gear, out var knownItem)) knownItem.UpdateWCLInfo(gear);
                else gearSet.Add(gear);
            }
        }

        Console.WriteLine($"Getting item information for {gearSet.Count} items. This may take a while...");

        var knownItems = await wclService.GetKnownItems(gearSet);

        knownItems.ForEach(item => data.KnownItems.Add(item));

        Console.WriteLine($"Item cache updated");
    }


    public void SaveReports (List<WCLPlayer> players)
    {
        foreach (var player in players)
        {
            if (data.ReportsByCode.ContainsKey(player.Report.Code))
            {
                data.ReportsByCode[player.Report.Code] = player.Report;
            }
            else
            {
                data.ReportsByCode.Add(player.Report.Code, player.Report);
            }
        }

        Console.WriteLine($"Reports saved");
    }


    public Program (ProgramConfig programConfig, WCLService wclService, WCLData data)
    {
        this.config = programConfig;
        this.wclService = wclService;
        this.data = data;
    }
}