namespace fusion.geartracker.data;

public class FusionPlayer : IEquatable<FusionPlayer>
{
    public int ActorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public FusionReport Report { get; set; } = new();
    public Dictionary<int, FusionGear> GearById { get; set; } = new();


    public bool Equals (FusionPlayer? other)
    {
        return Name.Equals(other?.Name);
    }


    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }


    public override string ToString ()
    {
        return $"{Name}";
    }


    public static FusionPlayer FromActor (FusionReport report, ReportActor actor)
    {
        return new FusionPlayer
        {
            ActorId = actor.Id ?? 0,
            Name = actor.Name ?? string.Empty,
            Report = report,
        };
    }
}