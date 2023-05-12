namespace fusion.geartracker.test;

internal class Program
{
    private ProgramConfig config;
    private IWCLService wclService;
    private WCLData data;


    private static async Task Main(string[] args)
    {
        var programConfig = ProgramConfig.Load();
        var data = WCLData.Load(programConfig.AppDataPath);
        var wclService = await CreateWCLService(programConfig, data);
        var program = new Program(programConfig, wclService, data);

        program.RemoveBlacklistedGear();

        var reports = await program.GetReports();
        var players = await program.GetPlayers(reports);

        program.UpdateGear(players);
        program.UpdateData(players);

        program.RemoveUntrackedPlayers();

        await program.UpdateItemCache();

        data.Save(programConfig.AppDataPath);
    }


    private static async Task SetBearerToken (HttpClient httpClient, ProgramConfig programConfig)
    {
        var clientAddress = new Uri(programConfig.BaseAddress);
        var clientId = HttpUtility.UrlEncode(programConfig.ClientId);
        var clientSecret = HttpUtility.UrlEncode(programConfig.ClientSecret);
        var encodedPair = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{clientAddress.Scheme}://{clientAddress.Host}/oauth/token"),
            Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"),
            Headers = { { "Authorization", $"Basic {encodedPair}" } },
        };
        var response = await httpClient.SendAsync(request);
        var contentStream = await response.Content.ReadAsStreamAsync();
        var bearerObject = JsonDocument.Parse(contentStream);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerObject.RootElement.GetProperty("access_token").GetString());
    }


    private static async Task<IWCLService> CreateWCLService (ProgramConfig programConfig, WCLData data)
    {
        IWCLService wclService;

        if (programConfig.UseReportCache)
        {
            wclService = new WCLDataService(data);
        }
        else
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(programConfig.BaseAddress),
            };
            var graphQLClient = new WCLGraphQLClient(httpClient);

            await SetBearerToken(httpClient, programConfig);

            wclService = new WCLAPIService(graphQLClient);
        }

        return wclService;
    }


    public async Task<List<WCLReport>> GetReports ()
    {
        var reports = await wclService.GetReports(config.GuildId, config.FirstReportDate, config.LastReportDate);

        reports = reports.FindAll(report => !config.ReportBlacklist.Contains(report.Code));

        Console.WriteLine($"Pulled {reports.Count} reports");

        if (wclService is WCLDataService) return reports;

        reports = reports.FindAll(report => !data.ReportsByCode.ContainsKey(report.Code));

        var newReportCount = reports.Count;

        reports = reports.GetRange(0, config.ReportCountToUpdate < newReportCount ? config.ReportCountToUpdate : newReportCount);

        Console.WriteLine($"Fetching data from {reports.Count}/{newReportCount} new reports");

        return reports;
    }


    public async Task<List<WCLPlayer>> GetPlayers (List<WCLReport> reports)
    {
        var players = new List<WCLPlayer>();

        await wclService.AddPlayerInfoToReports(reports);

        foreach (var report in reports)
        {
            foreach ((var actorId, var name) in report.Actors)
            {
                players.Add(WCLPlayer.Create(actorId, name, report));
            }
        }

        players.ForEach(player =>
        {
            if (config.PlayersToTrack.TryGetValue(player, out var trackedPlayer))
            {
                player.Raid = trackedPlayer.Raid;
                player.Class = trackedPlayer.Class;
                player.Spec = trackedPlayer.Spec;
            }
        });

        players.Sort((a, b) => a.Report.StartTime.CompareTo(b.Report.StartTime));

        return players;
    }


    public async Task UpdateItemCache ()
    {
        var gearSet = new HashSet<WCLGear>();

        foreach ((var name, var player) in data.PlayersByName)
        {
            player.GearById.Values.ToList().ForEach(gear => gearSet.Add(gear));
        }

        if (wclService is WCLAPIService wclAPIService && config.UpdateItemCache)
        {
            var knownItems = await wclAPIService.GetKnownItems(gearSet);

            knownItems.ForEach(item => { var itemAdded = !data.KnownItems.Contains(item) && data.KnownItems.Add(item); });
        }
    }


    public void UpdateGear (List<WCLPlayer> players)
    {
        // This is done to deal with multiple slots of the same name, e.g. Finger & Trinket
        var itemsToTrack = data.KnownItems.Aggregate(new HashSet<WCLGear>(data.KnownItems.Count), (itemsToTrack, trackedItem) =>
        {
            foreach (var gear in WCLGear.FromKnownItem(trackedItem))
            {
                itemsToTrack.Add(gear);
            }

            return itemsToTrack;
        });

        foreach (var player in players)
        {
            foreach (var gear in player.Report.GetCombatantInfo(player.GetActorKey()).Gear)
            {
                gear.FirstSeenAt = player.Report.StartTime;
                gear.LastSeenAt = player.Report.StartTime;
                gear.ReportCodeFirstSeen = player.Report.Code;

                if (itemsToTrack.TryGetValue(gear, out var trackedItem))
                {
                    gear.Name = trackedItem.Name;
                    gear.InstanceSize = trackedItem.InstanceSize;
                    gear.Ignore = trackedItem.Ignore;
                    gear.IsBIS = trackedItem.IsBIS;
                    gear.SizeOfUpgrade = trackedItem.SizeOfUpgrade;
                }
            }
        }
    }


    public void UpdateData (List<WCLPlayer> players)
    {
        foreach (var player in players)
        {
            WCLPlayer playerData;

            // Save this report
            if (data.ReportsByCode.ContainsKey(player.Report.Code))
            {
                data.ReportsByCode[player.Report.Code] = player.Report;
            }
            else
            {
                data.ReportsByCode.Add(player.Report.Code, player.Report);
            }

            // Get current player data, if any
            if (data.PlayersByName.ContainsKey(player.Name))
            {
                playerData = data.PlayersByName[player.Name];
            }
            else
            {
                playerData = player;

                data.PlayersByName.Add(player.Name, playerData);
            }

            // Update gear on player data
            foreach (var gear in player.Report.GetCombatantInfo(player.GetActorKey()).Gear)
            {
                var gearHash = gear.GetHashString();

                if (playerData.GearById.ContainsKey(gearHash))
                {
                    playerData.GearById[gearHash].Update(gear);
                }
                else
                {
                    playerData.GearById.Add(gearHash, gear);
                }
            }

            playerData.Raid = player.Raid;
            playerData.Class = player.Class;
            playerData.Spec = player.Spec;
            // Only save necessary information on the player's report reference
            playerData.Report = new()
            {
                Code = playerData.Report.Code,
                Title = playerData.Report.Title,
                StartTime = playerData.Report.StartTime,
                EndTime = playerData.Report.EndTime,
            };
        }
    }


    public void RemoveBlacklistedGear ()
    {
        foreach ((var name, var player) in data.PlayersByName)
        {
            var gearIds = new List<string>();

            foreach ((var id, var gear) in player.GearById)
            {
                if (config.ReportBlacklist.Contains(gear.ReportCodeFirstSeen))
                {
                    gearIds.Add(id);
                }
            }

            gearIds.ForEach(id => player.GearById.Remove(id));
        }
    }


    public void RemoveUntrackedPlayers ()
    {
        var names = new List<string>();

        foreach ((var name, var player) in data.PlayersByName)
        {
            if (!config.PlayersToTrack.Contains(player))
            {
                names.Add(name);
            }
        }

        names.ForEach(name => data.PlayersByName.Remove(name));
    }


    public Program (ProgramConfig programConfig, IWCLService wclService, WCLData data)
    {
        this.config = programConfig;
        this.wclService = wclService;
        this.data = data;
    }
}