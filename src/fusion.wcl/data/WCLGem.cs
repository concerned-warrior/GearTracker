namespace fusion.wcl.data;

public class WCLGem : IEquatable<WCLGem>
{
    public int Id { get; set; }
    public string Icon { get; set; } = string.Empty;
    public int ItemLevel { get; set; }


    public bool Equals (WCLGem? other)
    {
        return Id.Equals(other?.Id);
    }


    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}