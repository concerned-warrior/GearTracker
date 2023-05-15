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
    public int SheetsWeeksOldToIgnore { get; set; }
    public bool UseReportCache { get; set; }
    public bool UpdateItemCache { get; set; }
    public int ReportCountToUpdate { get; set; }
    public DateTime NewestReportDate { get; set; }
    public DateTime OldestReportDate { get; set; }
    public HashSet<string> ReportBlacklist { get; set; } = new();
    public HashSet<WCLPlayer> PlayersToTrack { get; set; } = new();


    public static ProgramConfig Load ()
    {
        var appSettingsPath = $"{Directory.GetCurrentDirectory()}/../../appsettings/appsettings.json";
        var config = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: false)
            .Build();

        var programConfig = new ProgramConfig();

        config.Bind(programConfig);

        return programConfig;
    }
}