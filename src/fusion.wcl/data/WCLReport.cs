namespace fusion.wcl.data;

public class WCLReport : IEquatable<WCLReport>
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public int ExportedSegments { get; set; }
    public int Segments { get; set; }

    // Set in IWCLService.AddPlayerInfoToReports
    public Dictionary<int, string> Actors { get; set; } = new();
    // Set in IWCLService.AddPlayerInfoToReports
    public Dictionary<string, WCLCombatantInfo> CombatantInfoByActor { get; set; } = new();


    public WCLCombatantInfo GetCombatantInfo (string actorKey)
    {
        if (CombatantInfoByActor.TryGetValue(actorKey, out var combatantInfo))
        {
            return combatantInfo;
        }
        else
        {
            return new();
        }
    }


    public bool Equals (WCLReport? other)
    {
        return Code.Equals(other?.Code);
    }


    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }


    public static WCLReport FromReport (Report report)
    {
        return new WCLReport
        {
            Code = report.Code,
            Title = report.Title,
            StartTime = DateTimeOffset.FromUnixTimeMilliseconds((long)report.StartTime),
            EndTime = DateTimeOffset.FromUnixTimeMilliseconds((long)report.EndTime),
            ExportedSegments = report.ExportedSegments,
            Segments = report.Segments,
        };
    }
}