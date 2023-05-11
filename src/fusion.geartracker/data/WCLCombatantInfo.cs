namespace fusion.geartracker.data;

public class WCLCombatantInfo
{
    public List<WCLGear> Gear { get; set; } = new();
    public WCLPlayer Player { get; set; } = new();


    public static WCLCombatantInfo FromJsonArrayString (WCLPlayer player, string json, WCLCombatantInfo? seed = null)
    {
        var combatantInfoList = JsonSerializer.Deserialize<List<WCLCombatantInfo>>(json, DataService.DataJsonSerializerOptions) ?? new();

        seed = seed is null ? new WCLCombatantInfo()
        {
            Player = player,
        } : seed;

        combatantInfoList.ForEach(combatantInfo =>
        {
            for (var i = 0; i < combatantInfo.Gear.Count; i++)
            {
                combatantInfo.Gear[i].SlotId = i + 1;
            }
        });

        return combatantInfoList.Aggregate(seed, (seed, combatantInfo) =>
        {
            seed.Gear = seed.Gear.Union(combatantInfo.Gear).ToList();

            return seed;
        });
    }
}