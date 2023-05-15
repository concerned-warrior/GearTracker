namespace fusion.wcl;

public class WCLDataService : IWCLService
{
    private WCLData data;


    public async Task<List<WCLReport>> GetReports (int guildId, DateTimeOffset newestReportDate, DateTimeOffset oldestReportDate)
    {
        var reports = new List<WCLReport>();

        foreach ((var code, var report) in data.ReportsByCode)
        {
            if (report.StartTime < newestReportDate && report.EndTime > oldestReportDate)
            {
                reports.Add(report);
            }
        }

        return await Task.FromResult(reports);
    }


    public async Task AddPlayerInfoToReports (List<WCLReport> reports)
    {
        await Task.CompletedTask;
    }


    public WCLDataService (WCLData data)
    {
        this.data = data;
    }
}