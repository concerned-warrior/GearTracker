namespace fusion.wcl;

public interface IWCLService
{
    static JsonSerializerOptions DataJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };


    Task<List<WCLReport>> GetReports (int guildId, DateTimeOffset firstReportDate, DateTimeOffset lastReportDate);


    Task<List<WCLPlayer>> GetReportPlayers (List<WCLReport> reports);


    Task<List<WCLCombatantInfo>> GetCombatantInfos (List<WCLPlayer> players);
}