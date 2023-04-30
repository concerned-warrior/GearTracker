namespace fusion.geartracker.data;

public class FusionGear
{
    public int Id { get; set; }
    public string Icon { get; set; } = string.Empty;
    public int ItemLevel { get; set; }
    public int Enchant { get; set; }
    public DateTimeOffset FirstSeenAt { get; set; }


    public override string ToString ()
    {
        return $"Id: {Id} Icon: {Icon} ItemLevel: {ItemLevel} Enchant: {Enchant}";
    }
}