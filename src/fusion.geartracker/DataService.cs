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


    public async Task<HashSet<FusionReport>> GetReports ()
    {
        ReportPagination reportPagination = new();
        FusionReport lastReport = new();
        HashSet<FusionReport> reports = new();

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


    public async Task<List<FusionPlayer>> GetPlayers (HashSet<FusionReport> reports, HashSet<string> playersToTrack)
    {
        var players = new List<FusionPlayer>();

        await Parallel.ForEachAsync(reports, async (report, cancellationToken) =>
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
        });

        players.Sort((a, b) => a.Report.StartTime.CompareTo(b.Report.StartTime));

        return players;
    }


    private async Task<FusionCombatantInfo> getCombatantInfo (FusionPlayer player)
    {
        var report = player.Report;
        var startTime = 0.0;
        var endTime = report.EndTime.ToUnixTimeMilliseconds() - report.StartTime.ToUnixTimeMilliseconds();
        var result = await graphQLClient.Execute(new Gear(report.Code, startTime, endTime, player.ActorId));
        var reportEventPaginator = result.Data?.__Report.__Events ?? new();
        var combatantInfo = FusionCombatantInfo.FromJsonArrayString(player, reportEventPaginator.Data?.ToString() ?? "[]");

        while (reportEventPaginator.NextPageTimestamp > 0)
        {
            startTime = (double)reportEventPaginator.NextPageTimestamp;
            result = await graphQLClient.Execute(new Gear(report.Code, startTime, endTime, player.ActorId));
            reportEventPaginator = result.Data?.__Report.__Events ?? new();

            combatantInfo = FusionCombatantInfo.FromJsonArrayString(player, reportEventPaginator.Data?.ToString() ?? "[]", combatantInfo);
        }

        return combatantInfo;
    }


    public async Task<Dictionary<FusionPlayer, HashSet<FusionGear>>> GetGearSetByPlayer (List<FusionPlayer> players, HashSet<int> itemsToTrack)
    {
        var gearListByPlayer = new Dictionary<FusionPlayer, List<FusionGear>>();
        var gearSetByPlayer = new Dictionary<FusionPlayer, HashSet<FusionGear>>();
        var gearToTrack = itemsToTrack.Aggregate(new HashSet<FusionGear>(itemsToTrack.Count), (seed, itemId) =>
        {
            seed.Add(new FusionGear { Id = itemId });

            return seed;
        });

        await Parallel.ForEachAsync(players, async (player, cancellationToken) =>
        {
            var combatantInfo = await getCombatantInfo(player);

            combatantInfo.Gear.IntersectWith(gearToTrack);

            foreach (var gear in combatantInfo.Gear)
            {
                gear.FirstSeenAt = player.Report.StartTime;
            }

            if (gearListByPlayer.ContainsKey(player))
            {
                gearListByPlayer[player].AddRange(combatantInfo.Gear);
            }
            else
            {
                gearListByPlayer.Add(player, new(combatantInfo.Gear));
            }
        });

        foreach ((var player, var gearList) in gearListByPlayer)
        {
            gearList.Sort((a, b) => a.FirstSeenAt.CompareTo(b.FirstSeenAt));
            gearList.ForEach(gear =>
            {
                if (gearSetByPlayer.ContainsKey(player))
                {
                    gearSetByPlayer[player].Add(gear);
                }
                else
                {
                    gearSetByPlayer.Add(player, new() { gear });
                }
            });
        }

        return gearSetByPlayer;
    }


    public DataService (WCLGraphQLClient graphQLClient, ProgramConfig programConfig)
    {
        this.programConfig = programConfig;
        this.graphQLClient = graphQLClient;
    }
}