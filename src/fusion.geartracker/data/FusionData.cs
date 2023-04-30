namespace fusion.geartracker.data;

public class FusionData
{
    public HashSet<int> ItemsToTrack { get; set; } = new();
    public HashSet<string> PlayersToTrack { get; set; } = new();
    public Dictionary<string, FusionReport> ReportsByCode { get; set; } = new();
    public Dictionary<string, FusionPlayer> PlayersByName { get; set; } = new();


    public void AddGearToPlayer (FusionCombatantInfo combatantInfo)
    {
        if (PlayersByName.TryGetValue(combatantInfo.Player.Name, out var player))
        {
            foreach (var gear in combatantInfo.Gear)
            {
                if (!player.GearById.ContainsKey(gear.Id))
                {
                    player.GearById.Add(gear.Id, gear);
                }
            }
        }
        else
        {
            PlayersByName.Add(combatantInfo.Player.Name, combatantInfo.Player);
        }
    }
}