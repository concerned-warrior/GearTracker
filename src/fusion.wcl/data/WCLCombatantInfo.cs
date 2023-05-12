namespace fusion.wcl.data;

public class WCLCombatantInfo
{
    public List<WCLGear> Gear { get; set; } = new();
    [JsonIgnore]
    public WCLPlayer Player { get; set; } = new();


    public static WCLCombatantInfo FromJsonArrayString (WCLPlayer player, string json, WCLCombatantInfo? seed = null)
    {
        var combatantInfoList = JsonSerializer.Deserialize<List<WCLCombatantInfo>>(json, IWCLService.DataJsonSerializerOptions) ?? new();

        seed = seed is null ? new()
        {
            Player = player,
        } : seed;

        // Assign slot identifiers based on their position in the WCL response
        combatantInfoList.ForEach(combatantInfo =>
        {
            for (var i = 0; i < combatantInfo.Gear.Count; i++)
            {
                var gear = combatantInfo.Gear[i];

                gear.Name = gear.Id.ToString();
                gear.SlotId = i + 1;
                gear.Slot = gear.GetSlotFromId();
            }
        });

        return combatantInfoList.Aggregate(seed, (seed, combatantInfo) =>
        {
            seed.Gear = seed.Gear.Union(combatantInfo.Gear).ToList();

            return seed;
        });
    }
}