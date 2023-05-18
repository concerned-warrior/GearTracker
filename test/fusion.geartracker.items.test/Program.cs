namespace fusion.geartracker.items.test;

internal class Program
{
    private ProgramConfig config;
    private GoogleSheetsService sheetsService;
    private WCLData data;

    private const string KnownItemsTitle = "Known Items";


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

        var spreadsheet = await sheetsService.GetSpreadsheet($"'{KnownItemsTitle}'");
        var sheet = spreadsheet.Sheets.Single(sheet => sheet.Properties.Title.Equals(KnownItemsTitle));
        var knownItems = program.GetKnownItems(sheet);

        Console.WriteLine($"Got {knownItems.Count} known items");
        Console.WriteLine($"Currently tracking {data.KnownItems.Count} known items");

        program.UpdateKnownItems(knownItems);

        Console.WriteLine($"Now tracking {data.KnownItems.Count} known items");

        data.Save(programConfig.AppDataPath);
    }


    public HashSet<WCLGear> GetKnownItems (Sheet sheet)
    {
        var knownItems = new HashSet<WCLGear>();
        var gridData = sheet.Data.First();
        var header = gridData.RowData.First();
        var idIndex = 0;
        var ignoreIndex = 0;
        var slotIndex = 0;
        var iconIndex = 0;
        var nameIndex = 0;
        var iLvlIndex = 0;
        var instanceSizeIndex = 0;
        var isBISIndex = 0;
        var sizeOfUpgradeIndex = 0;

        for (var i = 0; i < header.Values.Count; i++)
        {
            var cell = header.Values[i];

            idIndex = cell?.FormattedValue?.Trim().Equals("Id") ?? false ? i : idIndex;
            ignoreIndex = cell?.FormattedValue?.Trim().Equals("Ignore") ?? false ? i : ignoreIndex;
            slotIndex = cell?.FormattedValue?.Trim().Equals("Slot") ?? false ? i : slotIndex;
            iconIndex = cell?.FormattedValue?.Trim().Equals("Icon") ?? false ? i : iconIndex;
            nameIndex = cell?.FormattedValue?.Trim().Equals("Name") ?? false ? i : nameIndex;
            iLvlIndex = cell?.FormattedValue?.Trim().Equals("iLvl") ?? false ? i : iLvlIndex;
            instanceSizeIndex = cell?.FormattedValue?.Trim().Equals("10 Man") ?? false ? i : instanceSizeIndex;
            isBISIndex = cell?.FormattedValue?.Trim().Equals("BIS") ?? false ? i : isBISIndex;
            sizeOfUpgradeIndex = cell?.FormattedValue?.Trim().Equals("Size of Upgrade") ?? false ? i : sizeOfUpgradeIndex;
        }

        for (var i = 1; i < gridData.RowData.Count; i++)
        {
            var row = gridData.RowData[i];

            var id = (int?)row?.Values[idIndex]?.EffectiveValue?.NumberValue ?? 0;
            var ignore = row?.Values[ignoreIndex]?.EffectiveValue?.BoolValue ?? false;
            var slot = row?.Values[slotIndex]?.EffectiveValue?.StringValue?.Trim() ?? string.Empty;
            var icon = row?.Values[iconIndex]?.UserEnteredValue?.FormulaValue?.Trim() ?? string.Empty;
            var name = row?.Values[nameIndex]?.EffectiveValue?.StringValue?.Trim() ?? string.Empty;
            var iLvl = (int?)row?.Values[iLvlIndex]?.EffectiveValue?.NumberValue ?? 0;
            var is10Man = row?.Values[instanceSizeIndex]?.EffectiveValue?.BoolValue ?? false;
            var isBIS = row?.Values[isBISIndex]?.EffectiveValue?.BoolValue ?? false;
            var sizeOfUpgrade = row?.Values[sizeOfUpgradeIndex]?.EffectiveValue?.StringValue?.Trim() ?? string.Empty;

            if (id == 0 || string.IsNullOrWhiteSpace(slot) || string.IsNullOrWhiteSpace(icon) || string.IsNullOrWhiteSpace(name) || iLvl == 0)
            {
                continue;
            }

            var gearList = WCLGear.FromKnownItem(new WCLGear
            {
                Id = id,
                Icon = Path.GetFileName(icon.Replace("=image(\"https://", string.Empty).Replace("\")", string.Empty)),
                Name = name,
                ItemLevel = iLvl,
                Slot = slot,
                InstanceSize = is10Man ? 10 : WCLGear.DefaultInstanceSize,
                Ignore = ignore,
                IsBIS = isBIS,
                SizeOfUpgrade = Enum.Parse<UpgradeType>(sizeOfUpgrade),
            });

            gearList.ForEach(gear => knownItems.Add(gear));
        }

        return knownItems;
    }


    public void UpdateKnownItems (HashSet<WCLGear> knownItems)
    {
        data.KnownItems = knownItems;
    }


    public Program (ProgramConfig programConfig, GoogleSheetsService sheetsService, WCLData data)
    {
        this.config = programConfig;
        this.sheetsService = sheetsService;
        this.data = data;
    }
}