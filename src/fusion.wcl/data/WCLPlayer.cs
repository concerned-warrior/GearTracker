namespace fusion.wcl.data;

public class WCLPlayer : IEquatable<WCLPlayer>
{
    public int ActorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public WCLReport Report { get; set; } = new();

    // Set in Program.GetPlayers
    public string Raid { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public string Spec { get; set; } = string.Empty;
    // Set in Program.UpdateData
    public Dictionary<string, WCLGear> GearById { get; set; } = new();

    private List<string> ignoredGearSlots = new() { "Tabard", "Shirt" };
    private List<string> doubledGearSlots = new() { "Finger", "Trinket" };


    public int GetAverageItemLevel (IOrderedEnumerable<WCLGear> gear)
    {
        var itemCount = 0;
        var itemLevel = 0;
        var gearSlots = gear.GroupBy(gear => gear.Slot);

        foreach (var gearSlot in gearSlots)
        {
            if (ignoredGearSlots.Contains(gearSlot.Key)) continue;

            var item1 = gearSlot.ElementAtOrDefault(0);
            var item2 = gearSlot.ElementAtOrDefault(1);

            if (item1 is not null && item1.ItemLevel > 0)
            {
                itemCount += 1;
                itemLevel += item1.ItemLevel;
            }

            if (!doubledGearSlots.Contains(gearSlot.Key)) continue;

            if (item2 is not null && item2.ItemLevel > 0)
            {
                itemCount += 1;
                itemLevel += item2.ItemLevel;
            }
        }

        return itemCount == 0 ? 0 : itemLevel / itemCount;
    }


    public IOrderedEnumerable<WCLGear> GetOrderedGear ()
    {
        return GearById.Values.ToList()
            .Where(g => !g.Ignore)
            .OrderBy(g => g.LastSeenAt, Comparer<DateTimeOffset>.Create((a, b) => b.CompareTo(a)))
            .ThenBy(g => g.IsBIS)
            .ThenBy(g => g.SizeOfUpgrade, Comparer<UpgradeType>.Create((a, b) => b.CompareTo(a)))
            .ThenBy(g => g.ItemLevel, Comparer<int>.Create((a, b) => b.CompareTo(a)));
    }


    public DateTimeOffset GetLast10 () => getLast(gear => gear.InstanceSize == 10);
    public DateTimeOffset GetLast25 () => getLast(gear => gear.InstanceSize == 25);
    public DateTimeOffset GetLastMin () => getLast(gear => gear.SizeOfUpgrade == UpgradeType.Minor);
    public DateTimeOffset GetLastMod () => getLast(gear => gear.SizeOfUpgrade == UpgradeType.Moderate);
    public DateTimeOffset GetLastMaj () => getLast(gear => gear.SizeOfUpgrade == UpgradeType.Major);
    public DateTimeOffset GetLastBIS () => getLast(gear => gear.IsBIS);
    public int GetBISCount () => GearById.Values.DistinctBy(gear => gear.Id).Where(gear => gear.IsBIS).Count();
    private DateTimeOffset getLast (Func<WCLGear, bool> predicate)
    {
        var result = GearById.Values.Aggregate(DateTimeOffset.Now, (date, gear) => date < gear.FirstSeenAt ? date : gear.FirstSeenAt);

        foreach (var gear in GearById.Values.Where(predicate))
        {
            result = result > gear.FirstSeenAt ? result : gear.FirstSeenAt;
        }

        return result;
    }


    public string GetActorKey () => GetActorKey(ActorId, Name);
    public static string GetActorKey (int actorId, string name)
    {
        return $"{actorId}_{name}";
    }


    public bool Equals (WCLPlayer? other)
    {
        return Name.Equals(other?.Name);
    }


    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }


    public void Update (WCLPlayer trackedPlayer)
    {
        Raid = trackedPlayer.Raid;
        Class = trackedPlayer.Class;
        Spec = trackedPlayer.Spec;
    }


    public static WCLPlayer Create (int actorId, string name, WCLReport report)
    {
        return new()
        {
            ActorId = actorId,
            Name = name,
            Report = report,
        };
    }
}