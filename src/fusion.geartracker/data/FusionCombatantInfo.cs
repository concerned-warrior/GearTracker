namespace fusion.geartracker.data;

public class FusionCombatantInfo
{
    public List<FusionGear> Gear { get; set; } = new();
    public FusionPlayer Player { get; set; } = new();


    public static FusionCombatantInfo FromJsonArrayString (FusionPlayer player, string json, FusionCombatantInfo? seed = null)
    {
        var combatantInfoList = JsonSerializer.Deserialize<List<FusionCombatantInfo>>(json, DataService.DataJsonSerializerOptions) ?? new();

        seed = seed is null ? new FusionCombatantInfo()
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