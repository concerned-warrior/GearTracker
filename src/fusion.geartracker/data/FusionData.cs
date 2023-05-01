namespace fusion.geartracker.data;

public class FusionData
{
    public Dictionary<string, FusionReport> ReportsByCode { get; set; } = new();
    public Dictionary<string, FusionPlayer> PlayersByName { get; set; } = new();
    public Dictionary<string, HashSet<string>> ReportCodesByPlayer { get; set; } = new();
}