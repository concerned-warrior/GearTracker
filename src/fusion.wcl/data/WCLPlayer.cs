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


    public int GetAverageItemLevel ()
    {
        var itemCount = 0;
        var itemLevel = 0;
        var gearSlots = GearById.Values
            .GroupBy(gear => gear.Slot)
            .Select(group =>
            {
                var groupList = group.ToList();

                groupList.Sort((a, b) => b.ItemLevel.CompareTo(a.ItemLevel));

                return groupList;
            })
            .ToList();

        foreach (var gear in gearSlots)
        {
            var item = gear.ElementAtOrDefault(0);

            if (item is not null)
            {
                itemCount += 1;
                itemLevel += item.ItemLevel;
            }
        }

        return itemCount == 0 ? 0 : itemLevel / itemCount;
    }


    public DateTimeOffset GetLast10 () => getLastInstanceSize(10);
    public DateTimeOffset GetLast25 () => getLastInstanceSize(25);
    private DateTimeOffset getLastInstanceSize (int instanceSize)
    {
        var result = DateTimeOffset.MinValue;

        foreach (var gear in GearById.Values.Where(gear => gear.InstanceSize.Equals(instanceSize)))
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