namespace fusion.geartracker;

public class ProgramConfig
{
    public string AppDataPath { get; set; } = string.Empty;
    public string BaseAddress { get; set; } = string.Empty;
    public string BearerToken { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public int GuildId { get; set; }
    public DateTime FirstReportDate { get; set; }
    public DateTime LastReportDate { get; set; }
    public HashSet<int> ItemsToTrack { get; set; } = new();
    public HashSet<string> PlayersToTrack { get; set; } = new();
}