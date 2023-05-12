namespace fusion.wcl;

public class WCLAPIService : IWCLService
{
    private WCLGraphQLClient graphQLClient;


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


    public async Task AddPlayerInfoToReports (List<WCLReport> reports)
    {
        foreach (var report in reports)
        {
            var reportActors = await getReportActors(report);
            var combatantInfos = await getCombatantInfos(report);

            reportActors.ForEach(actor => report.Actors.Add((int)actor.Id!, actor.Name!));

            foreach (var combatantInfo in combatantInfos)
            {
                var actorName = report.Actors[combatantInfo.SourceID];
                var actorKey = WCLPlayer.GetActorKey(combatantInfo.SourceID, actorName);

                if (report.CombatantInfoByActor.ContainsKey(actorKey))
                {
                    report.CombatantInfoByActor[actorKey].Gear.Union(combatantInfo.Gear);
                }
                else
                {
                    report.CombatantInfoByActor.Add(actorKey, combatantInfo);
                }
            }
        };
    }


    private async Task<List<ReportActor>> getReportActors (WCLReport report)
    {
        var result = await graphQLClient.Execute(new Players(report.Code));

        return new(result.Data?.__Report.__MasterData.__Actors ?? new ReportActor[0]);
    }


    private async Task<List<WCLCombatantInfo>> getCombatantInfos (WCLReport report)
    {
        var combatantInfos = new List<WCLCombatantInfo>();
        var startTime = 0.0;
        var endTime = (double)(report.EndTime.ToUnixTimeMilliseconds() - report.StartTime.ToUnixTimeMilliseconds());

        do
        {
            var result = await graphQLClient.Execute(new Gear(report.Code, startTime, endTime));
            var reportEventPaginator = result.Data?.__Report.__Events ?? new();

            startTime = reportEventPaginator.NextPageTimestamp > 0 ? (double)reportEventPaginator.NextPageTimestamp : endTime;

            combatantInfos.AddRange(WCLCombatantInfo.FromJsonArrayString(reportEventPaginator.Data?.ToString() ?? "[]"));
        } while (startTime < endTime);

        return combatantInfos;
    }


    public WCLAPIService (WCLGraphQLClient graphQLClient)
    {
        this.graphQLClient = graphQLClient;
    }
}