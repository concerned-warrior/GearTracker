namespace fusion.wcl;

public class WCLAPIService : IWCLService
{
    private WCLGraphQLClient graphQLClient;
    private const int degreeOfParallelism = 2;


    public async Task<List<WCLReport>> GetReports (int guildId, DateTimeOffset firstReportDate, DateTimeOffset lastReportDate)
    {
        ReportPagination reportPagination = new();
        WCLReport lastReport = new();
        List<WCLReport> reports = new();

        do
        {
            var result = await graphQLClient.Execute(new Reports(guildId, page: reportPagination.Current_page + 1));

            reportPagination = result.Data?.__Reports ?? new();

            foreach (var report in reportPagination.__Data)
            {
                lastReport = WCLReport.FromReport(report);

                if (lastReport.StartTime < firstReportDate && lastReport.EndTime > lastReportDate)
                {
                    reports.Add(lastReport);
                }
            }
        } while (lastReport.EndTime > lastReportDate && reportPagination.Has_more_pages);

        return reports;
    }


    public async Task<List<WCLPlayer>> GetReportPlayers (List<WCLReport> reports)
    {
        var playersBag = new ConcurrentBag<WCLPlayer>();
        var semaphore = new SemaphoreSlim(degreeOfParallelism);

        await Parallel.ForEachAsync(reports, async (report, cancellationToken) =>
        {
            await semaphore.WaitAsync();

            try
            {
                var result = await graphQLClient.Execute(new Players(report.Code));
                var actors = result.Data?.__Report.__MasterData.__Actors ?? new ReportActor[0];

                foreach (var actor in actors)
                {
                    report.Actors.Add((int)actor.Id!, actor.Name!);

                    playersBag.Add(WCLPlayer.Create(actor, report));
                }
            }
            finally
            {
                semaphore.Release();
            }
        });

        var playersList = playersBag.ToList();

        playersList.Sort((a, b) => a.Report.StartTime.CompareTo(b.Report.StartTime));

        return playersList;
    }


    public async Task<List<WCLCombatantInfo>> GetCombatantInfos (List<WCLPlayer> players)
    {
        var combatantInfos = new ConcurrentBag<WCLCombatantInfo>();
        var semaphore = new SemaphoreSlim(degreeOfParallelism);

        await Parallel.ForEachAsync(players, async (player, cancellationToken) =>
        {
            await semaphore.WaitAsync();

            try
            {
                var report = player.Report;
                var startTime = 0.0;
                var endTime = report.EndTime.ToUnixTimeMilliseconds() - report.StartTime.ToUnixTimeMilliseconds();
                var result = await graphQLClient.Execute(new Gear(report.Code, startTime, endTime, player.ActorId));
                var reportEventPaginator = result.Data?.__Report.__Events ?? new();
                var combatantInfo = WCLCombatantInfo.FromJsonArrayString(player, reportEventPaginator.Data?.ToString() ?? "[]");

                while (reportEventPaginator.NextPageTimestamp > 0)
                {
                    startTime = (double)reportEventPaginator.NextPageTimestamp;
                    result = await graphQLClient.Execute(new Gear(report.Code, startTime, endTime, player.ActorId));
                    reportEventPaginator = result.Data?.__Report.__Events ?? new();

                    combatantInfo = WCLCombatantInfo.FromJsonArrayString(player, reportEventPaginator.Data?.ToString() ?? "[]", combatantInfo);
                }

                combatantInfos.Add(combatantInfo);
            }
            finally
            {
                semaphore.Release();
            }
        });

        return combatantInfos.ToList();
    }


    public WCLAPIService (WCLGraphQLClient graphQLClient)
    {
        this.graphQLClient = graphQLClient;
    }
}