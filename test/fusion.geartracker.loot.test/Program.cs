namespace fusion.geartracker.loot.test;

internal class Program
{
    private ProgramConfig config;
    private GoogleSheetsService sheetsService;
    private WCLData data;

    private const string LootDumpTitle = "Loot Dump";


    private static async Task Main(string[] args)
    {
        var programConfig = ProgramConfig.Load();
        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets()
        {
            ClientId = programConfig.SheetsClientId,
            ClientSecret = programConfig.SheetsClientSecret,
        }, new[] { SheetsService.Scope.Spreadsheets }, "console", CancellationToken.None);
        var sheetsService = new GoogleSheetsService(programConfig, credential);
        var data = WCLData.Load(programConfig.AppDataPath);
        var program = new Program(programConfig, sheetsService, data);

        var spreadsheet = await sheetsService.GetSpreadsheet();
        var sheet = spreadsheet.Sheets.Single(sheet => sheet.Properties.Title.Equals(LootDumpTitle));
        var lootDump = program.GetLootDump(sheet);

        Console.WriteLine($"Got {lootDump.Count} players with {lootDump.Aggregate(0, (count, player) => count += player.GearById.Count)} items");

        var addedItemCount = program.UpdatePlayers(lootDump);

        Console.WriteLine($"Added {addedItemCount} items to players");

        data.Save(programConfig.AppDataPath);
    }


    public HashSet<WCLPlayer> GetLootDump (Sheet sheet)
    {
        var lootDump = new HashSet<WCLPlayer>();
        var knownItems = data.KnownItems.ToList();
        var gridData = sheet.Data.First();
        var header = gridData.RowData.First();
        var itemIdIndex = 0;
        var playerNameIndex = 0;
        var dateReceivedIndex = 0;

        for (var i = 0; i < header.Values.Count; i++)
        {
            var cell = header.Values[i];

            itemIdIndex = cell?.FormattedValue?.Trim().Equals("Item Id") ?? false ? i : itemIdIndex;
            playerNameIndex = cell?.FormattedValue?.Trim().Equals("Player Name") ?? false ? i : playerNameIndex;
            dateReceivedIndex = cell?.FormattedValue?.Trim().Equals("Date Received") ?? false ? i : dateReceivedIndex;
        }

        for (var i = 1; i < gridData.RowData.Count; i++)
        {
            var row = gridData.RowData[i];

            var itemId = (int?)row?.Values[itemIdIndex]?.EffectiveValue?.NumberValue ?? 0;
            var playerName = row?.Values[playerNameIndex]?.EffectiveValue?.StringValue?.Trim() ?? string.Empty;
            var dateReceivedString = row?.Values[dateReceivedIndex]?.FormattedValue?.Trim() ?? string.Empty;

            if (itemId == 0 || string.IsNullOrWhiteSpace(playerName) || string.IsNullOrWhiteSpace(dateReceivedString))
            {
                continue;
            }

            var dateReceived = DateTimeOffset.Parse(dateReceivedString); // "MM/dd/yyyy"
            var player = new WCLPlayer { Name = playerName };
            var gear = new WCLGear { Id = itemId };
            var foundItem = knownItems.Find(item => item.Id == gear.Id);

            Console.WriteLine(dateReceived);

            if (foundItem is null) continue;

            gear.Update(foundItem);
            gear.FirstSeenAt = dateReceived;
            gear.LastSeenAt = dateReceived;

            player = lootDump.TryGetValue(player, out var existingPlayer) ? existingPlayer : player;

            if (!player.GearById.ContainsKey(gear.GetHashString()))
            {
                player.GearById.Add(gear.GetHashString(), gear);
            }

            lootDump.Add(player);
        }

        return lootDump;
    }


    public int UpdatePlayers (HashSet<WCLPlayer> lootDump)
    {
        var count = 0;

        foreach (var player in lootDump)
        {
            if (data.PlayersByName.TryGetValue(player.Name, out var currentPlayer))
            {
                foreach ((var id, var gear) in player.GearById)
                {
                    if (!currentPlayer.GearById.ContainsKey(id))
                    {
                        count += 1;

                        Console.WriteLine($"Adding {gear.Name} to {currentPlayer.Name}");

                        currentPlayer.GearById.Add(id, gear);
                    }
                }
            }
        }

        return count;
    }


    public Program (ProgramConfig programConfig, GoogleSheetsService sheetsService, WCLData data)
    {
        this.config = programConfig;
        this.sheetsService = sheetsService;
        this.data = data;
    }
}