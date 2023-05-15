namespace fusion.wcl;

public interface IWCLService
{
    static JsonSerializerOptions DataJsonSerializerOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter(),
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };


    Task<List<WCLReport>> GetReports (int guildId, DateTimeOffset newestReportDate, DateTimeOffset oldestReportDate);


    Task AddPlayerInfoToReports (List<WCLReport> reports);
}