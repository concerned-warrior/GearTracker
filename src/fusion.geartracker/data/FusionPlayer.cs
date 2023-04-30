namespace fusion.geartracker.data;

public class FusionPlayer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public FusionReport Report { get; set; } = new();
    public Dictionary<int, FusionGear> GearById { get; set; } = new();


    public override string ToString ()
    {
        return $"{Name}";
    }


    public static FusionPlayer FromActor (FusionReport report, ReportActor actor)
    {
        return new FusionPlayer
        {
            Id = actor.Id ?? 0,
            Name = actor.Name ?? string.Empty,
            Report = report,
        };
    }
}