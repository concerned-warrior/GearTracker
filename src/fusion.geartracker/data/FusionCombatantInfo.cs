namespace fusion.geartracker.data;

public class FusionCombatantInfo
{
    public long Timestamp { get; set; }
    public int Fight { get; set; }
    public List<FusionGear> Gear { get; set; } = new();
    public FusionPlayer Player { get; set; } = new();


    public static FusionCombatantInfo FromJSONString (FusionPlayer player, string json)
    {
        var combatantInfo = JsonSerializer.Deserialize<FusionCombatantInfo>(json, DataService.DataJsonSerializerOptions) ?? new();

        combatantInfo.Player = player;

        return combatantInfo;
    }
}