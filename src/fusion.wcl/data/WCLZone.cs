namespace fusion.wcl.data;

public class WCLZone : IEquatable<WCLZone>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;


    public bool Equals (WCLZone? other)
    {
        return Id.Equals(other?.Id);
    }


    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}