namespace fusion.geartracker.data;

public class FusionReport : IEquatable<FusionReport>
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public int ExportedSegments { get; set; }
    public int Segments { get; set; }
    public Dictionary<string, int> Actors { get; set; } = new();


    public bool Equals (FusionReport? other)
    {
        return Code.Equals(other?.Code);
    }


    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }


    public override string ToString ()
    {
        return $"{Title} {Code} {StartTime.ToLocalTime()}-{EndTime.ToLocalTime()}";
    }


    public static FusionReport FromReport (Report report)
    {
        return new FusionReport
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