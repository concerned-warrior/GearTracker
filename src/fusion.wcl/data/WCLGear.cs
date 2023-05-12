namespace fusion.wcl.data;

public class WCLGear : IEquatable<WCLGear>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SlotId { get; set; }
    public string Slot { get; set; } = string.Empty;
    public int InstanceSize { get; set; } = 25;
    public string Icon { get; set; } = string.Empty;
    public int ItemLevel { get; set; }
    public int PermanentEnchant { get; set; }
    public List<WCLGem> Gems { get; set; } = new();

    // Set in Program.UpdateGear
    public DateTimeOffset FirstSeenAt { get; set; }

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
        SlotId = gear.SlotId;
        Name = gear.Name;
        Slot = gear.Slot;
        InstanceSize = gear.InstanceSize;
        Icon = gear.Icon;
        ItemLevel = gear.ItemLevel;
        PermanentEnchant = gear.PermanentEnchant;
        Gems = gear.Gems;
    }


    public static List<WCLGear> FromTrackedItem (TrackedItem trackedItem)
    {
        var gear = new List<WCLGear>();
        var index = slots.IndexOf(trackedItem.Slot);

        while (index > -1)
        {
            gear.Add(new()
            {
                Id = trackedItem.Id,
                SlotId = index,
                Name = trackedItem.Name,
                Slot = trackedItem.Slot,
                InstanceSize = trackedItem.InstanceSize,
            });

            index = slots.IndexOf(trackedItem.Slot, index + 1);
        }

        return gear;
    }
}