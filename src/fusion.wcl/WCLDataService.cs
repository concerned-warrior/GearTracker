namespace fusion.wcl;

public class WCLDataService : IWCLService
{
    private WCLData data;


    public async Task<List<WCLReport>> GetReports (int guildId, DateTimeOffset firstReportDate, DateTimeOffset lastReportDate)
    {
        var reports = new List<WCLReport>();

        foreach ((var code, var report) in data.ReportsByCode)
        {
            if (report.StartTime < firstReportDate && report.EndTime > lastReportDate)
            {
                reports.Add(report);
            }
        }

        return await Task.FromResult(reports);
    }


    public async Task<List<WCLPlayer>> GetReportPlayers (List<WCLReport> reports)
    {
        var players = new List<WCLPlayer>();

        foreach (var report in reports)
        {
            players.AddRange(report.Actors.ToList().ConvertAll(kvp =>
            {
                return WCLPlayer.Create(new() { Id = kvp.Key, Name = kvp.Value }, report);
            }));
        }

        players.Sort((a, b) => a.Report.StartTime.CompareTo(b.Report.StartTime));

        return await Task.FromResult(players);
    }


    public async Task<List<WCLCombatantInfo>> GetCombatantInfos (List<WCLPlayer> players)
    {
        var combatantInfos = new List<WCLCombatantInfo>();

        foreach (var player in players)
        {
            var combatantInfo = player.Report.CombatantInfoByActor[player.GetActorString()];

            combatantInfo.Player = player;

            combatantInfos.Add(combatantInfo);
        }

        return await Task.FromResult(combatantInfos);
    }


    public WCLDataService (WCLData data)
    {
        this.data = data;
    }
}