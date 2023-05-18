namespace fusion.geartracker.update.test;

internal class Program
{
    private ProgramConfig config;
    private WCLData data;


    private static void Main(string[] args)
    {
        var programConfig = ProgramConfig.Load();
        var data = WCLData.Load(programConfig.AppDataPath);
        var program = new Program(programConfig, data);
        var players = WCLReport.GetPlayers(data.ReportsByCode.Values, data.PlayersToTrack);

        program.UpdateGear(players);
        program.UpdateData(players);
        program.RemoveUntrackedPlayers();
        program.RemoveBlacklistedGear();

        data.Save(programConfig.AppDataPath);
    }


    public void UpdateGear (List<WCLPlayer> players)
    {
        foreach (var player in players)
        {
            foreach (var gear in player.Report.GetCombatantInfo(player.GetActorKey()).Gear)
            {
                gear.UpdateReportInfo(player.Report);

                if (data.KnownItems.TryGetValue(gear, out var trackedItem)) gear.UpdateCustomInfo(trackedItem);
            }
        }
    }


    public void UpdateData (List<WCLPlayer> players)
    {
        foreach (var player in players)
        {
            WCLPlayer playerData;

            // Get current player data, if any
            if (data.PlayersByName.ContainsKey(player.Name))
            {
                playerData = data.PlayersByName[player.Name];
            }
            else
            {
                playerData = player;

                data.PlayersByName.Add(player.Name, playerData);
            }

            // Update gear on player data
            foreach (var gear in player.Report.GetCombatantInfo(player.GetActorKey()).Gear)
            {
                var gearHash = gear.GetHashString();

                if (playerData.GearById.ContainsKey(gearHash))
                {
                    playerData.GearById[gearHash].Update(gear);
                }
                else
                {
                    playerData.GearById.Add(gearHash, gear);
                }
            }

            playerData.Update(player);
            // Only save necessary information on the player's report reference
            playerData.Report = WCLReport.CreateSlimReport(playerData.Report);
        }
    }


    public void RemoveBlacklistedGear ()
    {
        foreach ((var name, var player) in data.PlayersByName)
        {
            var gearIds = new List<string>();

            foreach ((var id, var gear) in player.GearById)
            {
                if (config.ReportBlacklist.Contains(gear.ReportCodeFirstSeen))
                {
                    gearIds.Add(id);
                }
            }

            gearIds.ForEach(id => player.GearById.Remove(id));
        }
    }


    public void RemoveUntrackedPlayers ()
    {
        var names = new List<string>();

        foreach ((var name, var player) in data.PlayersByName)
        {
            if (!data.PlayersToTrack.Contains(player))
            {
                names.Add(name);
            }
        }

        names.ForEach(name => data.PlayersByName.Remove(name));
    }


    public Program (ProgramConfig programConfig, WCLData data)
    {
        this.config = programConfig;
        this.data = data;
    }
}