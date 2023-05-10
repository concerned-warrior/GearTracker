namespace fusion.geartracker.data;

public class FusionPlayer : IEquatable<FusionPlayer>
{
    public int ActorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Raid { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public string Spec { get; set; } = string.Empty;
    public FusionReport Report { get; set; } = new();
    public Dictionary<string, FusionGear> GearById { get; set; } = new();


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


    public static FusionPlayer FromActor (ReportActor actor, FusionReport report, TrackedPlayer trackedPlayer)
    {
        return new()
        {
            ActorId = actor.Id ?? 0,
            Name = actor.Name ?? string.Empty,
            Raid = trackedPlayer.Raid,
            Class = trackedPlayer.Class,
            Spec = trackedPlayer.Spec,
            Report = report,
        };
    }
}