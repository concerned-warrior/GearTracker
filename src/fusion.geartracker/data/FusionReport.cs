namespace fusion.geartracker.data;

public class FusionReport
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public int ExportedSegments { get; set; }
    public int Segments { get; set; }


    public override string ToString ()
    {
        return $"{Title} {Code} {StartTime}-{EndTime}";
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