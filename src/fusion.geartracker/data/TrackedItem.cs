namespace fusion.geartracker.data;

public class TrackedItem : IEquatable<TrackedItem>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slot { get; set; } = string.Empty;
    public int InstanceSize { get; set; }


    public bool Equals (TrackedItem? other)
    {
        return Id.Equals(other?.Id) && Slot.Equals(other?.Slot);
    }


    public override int GetHashCode()
    {
        return $"{Id}_{Slot}".GetHashCode();
    }
}