namespace fusion.geartracker;

public class DataService
{
    private ProgramConfig programConfig;
    private WCLGraphQLClient graphQLClient;

    public static JsonSerializerOptions DataJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };


    public async Task<List<FusionReport>> GetReports ()
    {
        ReportPagination reportPagination = new();
        FusionReport lastReport = new();
        List<FusionReport> reports = new();

        do
        {
            var result = await graphQLClient.Execute(new Reports(programConfig.GuildId, page: reportPagination.Current_page + 1));

            reportPagination = result.Data?.__Reports ?? new();

            foreach (var report in reportPagination.__Data)
            {
                lastReport = FusionReport.FromReport(report);

                if (lastReport.StartTime < programConfig.FirstReportDate && lastReport.EndTime > programConfig.LastReportDate)
                {
                    reports.Add(lastReport);
                }
            }
        } while (lastReport.EndTime > programConfig.LastReportDate && reportPagination.Has_more_pages);

        return reports;
    }


    public async Task<List<FusionPlayer>> GetPlayers (List<FusionReport> reports, List<string> playersToTrack)
    {
        var players = new List<FusionPlayer>();

        foreach (var report in reports)
        {
            var result = await graphQLClient.Execute(new Players(report.Code));
            var actors = result.Data?.__Report.__MasterData.__Actors ?? new ReportActor[0];

            foreach (var actor in actors)
            {
                if (playersToTrack.Contains(actor.Name ?? string.Empty))
                {
                    players.Add(FusionPlayer.FromActor(report, actor));
                }
            }
        }

        return players;
    }


    public async Task<List<FusionCombatantInfo>> GetCombatantInfoList (List<FusionPlayer> players, List<int> itemsToTrack)
    {
        var combatantInfoList = new List<FusionCombatantInfo>();

        foreach (var player in players)
        {
            var report = player.Report;
            var startTime = 0.0;
            var endTime = report.EndTime.ToUnixTimeMilliseconds() - report.StartTime.ToUnixTimeMilliseconds();
            Console.WriteLine($"{report.Code}, {startTime}, {endTime}, {player.Id}");
            var result = await graphQLClient.Execute(new Gear(report.Code, startTime, endTime, player.Id));
            Console.WriteLine(result.Query);
            foreach (var error in result.Errors ?? new GraphQueryError[0])
            {
                Console.WriteLine(error.Message);
            }
            Console.WriteLine(result.Data?.__Report.__Events.Data ?? "{}");
            var combatantInfo = FusionCombatantInfo.FromJSONString(player, result.Data?.__Report.__Events.Data ?? "{}");

            combatantInfo.Gear = combatantInfo.Gear.FindAll(gear => itemsToTrack.Contains(gear.Id));
        }

        return combatantInfoList;
    }


    public FusionData Load ()
    {
        FusionData data;

        try
        {
            using var stream = File.OpenRead(programConfig.AppDataPath);

            data = JsonSerializer.Deserialize<FusionData>(stream, DataJsonSerializerOptions) ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DataService Load - {ex.Message}");

            data = new();
        }

        return data;
    }


    public void Save (FusionData data)
    {
        try
        {
            using var stream = File.OpenWrite(programConfig.AppDataPath);

            JsonSerializer.Serialize(stream, data, DataJsonSerializerOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DataService Save - {ex.Message}");
        }
    }


    public DataService (WCLGraphQLClient graphQLClient, ProgramConfig programConfig)
    {
        this.programConfig = programConfig;
        this.graphQLClient = graphQLClient;
    }
}