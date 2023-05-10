namespace fusion.geartracker.data;

public class TrackedPlayer : IEquatable<TrackedPlayer>
{
    public string Name { get; set; } = string.Empty;
    public string Raid { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public string Spec { get; set; } = string.Empty;


    public bool Equals (TrackedPlayer? other)
    {
        return Name.Equals(other?.Name);
    }


    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}