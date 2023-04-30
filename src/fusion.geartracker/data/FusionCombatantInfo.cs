namespace fusion.geartracker.data;

public class FusionCombatantInfo
{
    public HashSet<FusionGear> Gear { get; set; } = new();
    public FusionPlayer Player { get; set; } = new();


    public static FusionCombatantInfo FromJsonArrayString (FusionPlayer player, string json, FusionCombatantInfo? seed = null)
    {
        var combatantInfoList = JsonSerializer.Deserialize<List<FusionCombatantInfo>>(json, DataService.DataJsonSerializerOptions) ?? new();

        seed = seed is null ? new FusionCombatantInfo()
        {
            Player = player,
        } : seed;

        return combatantInfoList.Aggregate(seed, (seed, combatantInfo) =>
        {
            seed.Gear.UnionWith(combatantInfo.Gear);

            return seed;
        });
    }
}