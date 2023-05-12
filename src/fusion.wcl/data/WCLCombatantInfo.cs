namespace fusion.wcl.data;

public class WCLCombatantInfo
{
    public int SourceID { get; set; }
    public List<WCLGear> Gear { get; set; } = new();


    public static List<WCLCombatantInfo> FromJsonArrayString (string json)
    {
        var combatantInfoList = JsonSerializer.Deserialize<List<WCLCombatantInfo>>(json, IWCLService.DataJsonSerializerOptions) ?? new();

        // Assign slot identifiers based on their position in the WCL response
        combatantInfoList.ForEach(combatantInfo =>
        {
            for (var i = 0; i < combatantInfo.Gear.Count; i++)
            {
                var gear = combatantInfo.Gear[i];

                gear.Name = gear.Id.ToString();
                gear.SlotId = i + 1;
                gear.Slot = gear.GetSlotFromId();
            }
        });

        return combatantInfoList;
    }
}