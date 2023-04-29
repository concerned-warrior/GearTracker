namespace fusion.geartracker;

public class ProgramConfig
{
    public string AppDataPath { get; set; } = string.Empty;
    public string BaseAddress { get; set; } = string.Empty;
    public string BearerToken { get; set; } = string.Empty;
    public int GuildId { get; set; }
    public DateTime FirstReportDate { get; set; }
    public DateTime LastReportDate { get; set; }
}