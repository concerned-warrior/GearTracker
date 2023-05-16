namespace fusion.wcl.data;

public class WCLGear : IEquatable<WCLGear>
{
    public const int DefaultInstanceSize = 25;

    public int Id { get; set; }
    public int SlotId { get; set; }
    public string Icon { get; set; } = string.Empty;
    public int ItemLevel { get; set; }
    public int PermanentEnchant { get; set; }
    public List<WCLGem> Gems { get; set; } = new();

    // Set in Program.UpdateGear
    public string Name { get; set; } = string.Empty;
    public string Slot { get; set; } = string.Empty;
    public int InstanceSize { get; set; } = DefaultInstanceSize;
    public bool Ignore { get; set; }
    public bool IsBIS { get; set; }
    public UpgradeType SizeOfUpgrade { get; set; }
    public DateTimeOffset FirstSeenAt { get; set; }
    public DateTimeOffset LastSeenAt { get; set; }
    public string ReportCodeFirstSeen { get; set; } = string.Empty;

    private static List<string> slots = new() { "Ammo", "Head", "Neck", "Shoulder", "Shirt", "Chest", "Waist", "Legs", "Feet", "Wrist", "Hands", "Finger", "Finger", "Trinket", "Trinket", "Back", "Main Hand", "Off Hand", "Ranged", "Tabard" };


    public bool Equals (WCLGear? other)
    {
        return Id.Equals(other?.Id) && SlotId.Equals(other?.SlotId);
    }


    public override int GetHashCode()
    {
        return this.GetHashString().GetHashCode();
    }


    public string GetHashString ()
    {
        return $"{Id}_{SlotId}";
    }


    public string GetSlotFromId ()
    {
        return slots[SlotId];
    }


    public void Update (WCLGear gear)
    {
        Id = gear.Id;
        Icon = gear.Icon;
        Slot = gear.Slot;
        SlotId = gear.SlotId;
        ItemLevel = gear.ItemLevel;
        PermanentEnchant = gear.PermanentEnchant;
        Gems = gear.Gems;
        Name = gear.Name;
        InstanceSize = gear.InstanceSize;
        Ignore = gear.Ignore;
        IsBIS = gear.IsBIS;
        SizeOfUpgrade = gear.SizeOfUpgrade;
        // No FirstSeenAt in update to preserve item history
        LastSeenAt = gear.LastSeenAt;
    }


    public void UpdateCustomInfo (WCLGear trackedItem)
    {
        Name = trackedItem.Name;
        InstanceSize = trackedItem.InstanceSize;
        Ignore = trackedItem.Ignore;
        IsBIS = trackedItem.IsBIS;
        SizeOfUpgrade = trackedItem.SizeOfUpgrade;
    }


    public void UpdateReportInfo (WCLReport report)
    {
        FirstSeenAt = report.StartTime;
        LastSeenAt = report.StartTime;
        ReportCodeFirstSeen = report.Code;
    }


    public void UpdateWCLInfo (WCLGear gear)
    {
        Id = gear.Id;
        Icon = gear.Icon;
        Slot = gear.Slot;
        SlotId = gear.SlotId;
        ItemLevel = gear.ItemLevel;
    }


    public static List<WCLGear> FromKnownItem (WCLGear knownItem)
    {
        var gear = new List<WCLGear>();
        var index = slots.IndexOf(knownItem.Slot);

        while (index > -1)
        {
            gear.Add(new()
            {
                Id = knownItem.Id,
                SlotId = index,
                Name = knownItem.Name,
                Slot = knownItem.Slot,
                InstanceSize = knownItem.InstanceSize,
                Ignore = knownItem.Ignore,
                IsBIS = knownItem.IsBIS,
                SizeOfUpgrade = knownItem.SizeOfUpgrade,
            });

            index = slots.IndexOf(knownItem.Slot, index + 1);
        }

        return gear;
    }
}