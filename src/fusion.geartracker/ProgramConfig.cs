namespace fusion.geartracker;

public class ProgramConfig
{
    public string AppDataPath { get; set; } = string.Empty;
    public string BaseAddress { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public int GuildId { get; set; }
    public string SheetsClientId { get; set; } = string.Empty;
    public string SheetsClientSecret { get; set; } = string.Empty;
    public string SheetsSpreadsheetId { get; set; } = string.Empty;
    public bool UseReportCache { get; set; }
    public bool UpdateGear { get; set; }
    public int PlayerCountToUpdate { get; set; }
    public DateTime FirstReportDate { get; set; }
    public DateTime LastReportDate { get; set; }
    public HashSet<TrackedItem> ItemsToTrack { get; set; } = new();
    public HashSet<TrackedPlayer> PlayersToTrack { get; set; } = new();


    public static ProgramConfig Load (string appSettingsPath)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: false)
            .Build();

        var programConfig = new ProgramConfig();

        config.Bind(programConfig);

        return programConfig;
    }
}