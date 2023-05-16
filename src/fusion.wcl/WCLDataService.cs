namespace fusion.wcl;

public class WCLDataService : IWCLService
{
    private WCLData data;


    public async Task<List<WCLReport>> GetReports (int guildId, DateTimeOffset newestReportDate, DateTimeOffset oldestReportDate)
    {
        var reports = new List<WCLReport>();

        return await Task.FromResult(data.ReportsByCode.Values.ToList());
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