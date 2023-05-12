namespace fusion.wcl;

public interface IWCLService
{
    static JsonSerializerOptions DataJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };


    Task<List<WCLReport>> GetReports (int guildId, DateTimeOffset firstReportDate, DateTimeOffset lastReportDate);


    Task AddPlayerInfoToReports (List<WCLReport> reports);
}