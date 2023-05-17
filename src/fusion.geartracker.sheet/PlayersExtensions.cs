namespace fusion.geartracker.sheet;

public static class PlayersExtensions
{
    public static List<WCLPlayer> ByBISCount (this Dictionary<string, WCLPlayer> playersByName)
    {
        var players = playersByName.Values.ToList();

        return players
            .OrderBy(p => p.GetBISCount(), Comparer<int>.Create((a, b) => b.CompareTo(a)))
            .ToList();
    }


    public static List<WCLPlayer> ByName (this Dictionary<string, WCLPlayer> playersByName)
    {
        var players = playersByName.Values.ToList();

        players.Sort((a, b) => a.Name.CompareTo(b.Name));

        return players;
    }


    public static List<WCLPlayer> ByRaidBISCount (this Dictionary<string, WCLPlayer> playersByName)
    {
        var players = playersByName.Values.ToList();

        return players
            .OrderBy(p => p.Raid)
            .ThenBy(p => p.GetBISCount(), Comparer<int>.Create((a, b) => b.CompareTo(a)))
            .ToList();
    }


    public static List<WCLPlayer> ByRaidLastUpgrade (this Dictionary<string, WCLPlayer> playersByName)
    {
        var players = playersByName.Values.ToList();

        return players
            .OrderBy(p => p.Raid)
            .ThenBy(p => p.GetLastBIS(), Comparer<DateTimeOffset>.Create((a, b) => a.CompareTo(b)))
            .ThenBy(p => p.GetLastMaj(), Comparer<DateTimeOffset>.Create((a, b) => a.CompareTo(b)))
            .ThenBy(p => p.GetLastMod(), Comparer<DateTimeOffset>.Create((a, b) => a.CompareTo(b)))
            .ThenBy(p => p.GetLastMin(), Comparer<DateTimeOffset>.Create((a, b) => a.CompareTo(b)))
            .ThenBy(p => p.GetLast25(), Comparer<DateTimeOffset>.Create((a, b) => a.CompareTo(b)))
            .ThenBy(p => p.GetLast10(), Comparer<DateTimeOffset>.Create((a, b) => a.CompareTo(b)))
            .ToList();
    }


    public static List<WCLPlayer> ByRaidSpec (this Dictionary<string, WCLPlayer> playersByName)
    {
        var players = playersByName.Values.ToList();

        return players
            .OrderBy(p => p.Raid)
            .ThenBy(p => p.Class)
            .ThenBy(p => p.Spec)
            .ThenBy(p => p.GetAverageItemLevel(p.GetOrderedGear()), Comparer<int>.Create((a, b) => b.CompareTo(a)))
            .ToList();
    }


    public static List<WCLPlayer> BySpec (this Dictionary<string, WCLPlayer> playersByName)
    {
        var players = playersByName.Values.ToList();

        return players
            .OrderBy(p => p.Class)
            .ThenBy(p => p.Spec)
            .ThenBy(p => p.GetAverageItemLevel(p.GetOrderedGear()), Comparer<int>.Create((a, b) => b.CompareTo(a)))
            .ToList();
    }
}