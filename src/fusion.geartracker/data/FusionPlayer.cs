namespace fusion.geartracker.data;

public class FusionPlayer : IEquatable<FusionPlayer>
{
    public int ActorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Raid { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public string Spec { get; set; } = string.Empty;
    public FusionReport Report { get; set; } = new();
    public Dictionary<string, FusionGear> GearById { get; set; } = new();


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

        return itemLevel / itemCount;
    }


    public DateTimeOffset GetLast10 ()
    {
        var result = DateTimeOffset.MinValue;

        foreach (var gear in GearById.Values.Where(gear => gear.InstanceSize.Equals(10)))
        {
            result = result > gear.FirstSeenAt ? result : gear.FirstSeenAt;
        }

        return result;
    }


    public DateTimeOffset GetLast25 ()
    {
        var result = DateTimeOffset.MinValue;

        foreach (var gear in GearById.Values.Where(gear => gear.InstanceSize.Equals(25)))
        {
            result = result > gear.FirstSeenAt ? result : gear.FirstSeenAt;
        }

        return result;
    }


    public bool Equals (FusionPlayer? other)
    {
        return Name.Equals(other?.Name);
    }


    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }


    public override string ToString ()
    {
        return $"{Name}";
    }


    public static FusionPlayer FromActor (ReportActor actor, FusionReport report, TrackedPlayer trackedPlayer)
    {
        return new()
        {
            ActorId = actor.Id ?? 0,
            Name = actor.Name ?? string.Empty,
            Raid = trackedPlayer.Raid,
            Class = trackedPlayer.Class,
            Spec = trackedPlayer.Spec,
            Report = report,
        };
    }
}