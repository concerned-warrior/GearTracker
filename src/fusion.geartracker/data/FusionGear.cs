namespace fusion.geartracker.data;

public class FusionGear : IEquatable<FusionGear>
{
    public int Id { get; set; }
    public string Icon { get; set; } = string.Empty;
    public int ItemLevel { get; set; }
    public int Enchant { get; set; }
    public List<FusionGem> Gems { get; set; } = new();
    public DateTimeOffset FirstSeenAt { get; set; }


    public bool Equals (FusionGear? other)
    {
        return Id.Equals(other?.Id);
    }


    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }


    public override string ToString ()
    {
        return $"Id: {Id} Icon: {Icon} ItemLevel: {ItemLevel} Enchant: {Enchant}";
    }
}