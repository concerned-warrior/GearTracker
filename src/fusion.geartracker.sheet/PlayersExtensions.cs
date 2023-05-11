namespace fusion.geartracker.sheet;

public static class PlayersExtensions
{
    // Name
    public static List<WCLPlayer> ByName (this Dictionary<string, WCLPlayer> playersByName)
    {
        var players = playersByName.Values.ToList();

        players.Sort((a, b) => a.Name.CompareTo(b.Name));

        return players;
    }


    // Raid,Last25,Last10
    public static List<WCLPlayer> ByRaidLastUpgrade (this Dictionary<string, WCLPlayer> playersByName)
    {
        var players = playersByName.Values.ToList();

        return players
            .OrderBy(p => p.Raid)
            .ThenBy(p => p.GetLast25(), Comparer<DateTimeOffset>.Create((a, b) => a.CompareTo(b)))
            .ThenBy(p => p.GetLast10(), Comparer<DateTimeOffset>.Create((a, b) => a.CompareTo(b)))
            .ToList();
    }


    // Raid,Class,Spec,aiLvl
    public static List<WCLPlayer> ByRaidSpec (this Dictionary<string, WCLPlayer> playersByName)
    {
        var players = playersByName.Values.ToList();

        return players
            .OrderBy(p => p.Raid)
            .ThenBy(p => p.Class)
            .ThenBy(p => p.Spec)
            .ThenBy(p => p.GetAverageItemLevel(), Comparer<int>.Create((a, b) => b.CompareTo(a)))
            .ToList();
    }


    // Class,Spec,aiLvl
    public static List<WCLPlayer> BySpec (this Dictionary<string, WCLPlayer> playersByName)
    {
        var players = playersByName.Values.ToList();

        return players
            .OrderBy(p => p.Class)
            .ThenBy(p => p.Spec)
            .ThenBy(p => p.GetAverageItemLevel(), Comparer<int>.Create((a, b) => b.CompareTo(a)))
            .ToList();
    }
}