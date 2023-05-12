var tabCount = 6;
var classes = new List<string> { "Death Knight", "Paladin", "Druid", "Paladin", "Priest", "Shaman", "Death Knight", "Druid", "Paladin", "Rogue", "Shaman", "Warrior", "Hunter", "Mage", "Warlock", "Druid", "Priest" };
// var classIndexes = new List<int> { 6, 8, 11 };
var specs = new Dictionary<string, Dictionary<string, string>>
{
    { "Death Knight", new() {
        { "U", "Unholy" },
        { "F", "Frost" },
        { "B", "Blood" },
    } },
    { "Druid", new() {
        { "B", "Balance" },
        { "F", "Feral" },
        { "R", "Restoration" },
    } },
    { "Hunter", new() {
        { "B", "Beast Mastery" },
        { "M", "Marksmanship" },
        { "S", "Survival" },
    } },
    { "Mage", new() {
        { "A", "Arcane" },
        { "Fi", "Fire" },
        { "Fr", "Frost" },
    } },
    { "Paladin", new() {
        { "H", "Holy" },
        { "P", "Protection" },
        { "R", "Retribution" },
    } },
    { "Priest", new() {
        { "D", "Discipline" },
        { "H", "Holy" },
        { "S", "Shadow" },
    } },
    { "Rogue", new() {
        { "A", "Assassination" },
        { "C", "Combat" },
        { "S", "Subtlety" },
    } },
    { "Shaman", new() {
        { "El", "Elemental" },
        { "En", "Enhancement" },
        { "R", "Restoration" },
    } },
    { "Warlock", new() {
        { "A", "Affliction" },
        { "DM", "Demonology" },
        { "DS", "Destruction" },
    } },
    { "Warrior", new() {
        { "A", "Arms" },
        { "F", "Fury" },
        { "P", "Protection" },
    } },
};

var rosterLines = File.ReadAllLines("../../appdata/roster.txt");
var trackedPlayers = new List<WCLPlayer>();

foreach (var line in rosterLines)
{
    var strings = line.Split("\t");

    for (var i = 0; i < classes.Count; i++)
    {
        var count = i + 1;
        var sectionStartsAt = i * tabCount;
        var name = strings[sectionStartsAt + 2];
        var raidLetter = strings[sectionStartsAt];
        var raid = $"Raid {raidLetter}";
        var @class = classes[i];

        specs[@class].TryGetValue(strings[sectionStartsAt + 1], out var spec);

        if (string.IsNullOrWhiteSpace(name)) continue;
        if (raidLetter.Equals("-")) continue;
        // if (!classIndexes.Contains(i)) continue;

        trackedPlayers.Add(new WCLPlayer
        {
            Name = name,
            Raid = raid,
            Class = @class,
            Spec = spec ?? string.Empty,
        });
    }
}

File.WriteAllText("../../appdata/roster.json", JsonSerializer.Serialize(new Dictionary<string, List<WCLPlayer>>
{
    { "playersToTrack", trackedPlayers },
}, new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
}));