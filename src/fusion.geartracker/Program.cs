var config = new ConfigurationBuilder()
    .AddJsonFile("./appsettings/appsettings.json", optional: false, reloadOnChange: false)
    .Build();
var gearTrackerConfig = new GearTrackerConfig();
var httpClient = new HttpClient();
var graphQLClient = new TestServerGraphQLClient(httpClient);

config.Bind(nameof(GearTrackerConfig), gearTrackerConfig);

httpClient.BaseAddress = new Uri(config[nameof(httpClient.BaseAddress)]!);
httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config["BearerToken"]!);


var response = await graphQLClient.Execute(new GuildReportsWithPlayers(gearTrackerConfig.GuildId));
var reports = response.Data?.__Reports.__Data ?? new Report[0];

Console.WriteLine($"Number of reports: {reports.Length}");

for (int i = 0; i < reports.Length; i++)
{
    var report = reports[i];
    var endTime = DateTimeOffset.FromUnixTimeMilliseconds((long)report.EndTime);

    // if (endTime < gearTrackerConfig.LastReportDate)
    // {
    //     break;
    // }

    Console.WriteLine($"{i}) {report.Title} {report.Code} {endTime} {endTime > gearTrackerConfig.LastReportDate}");
}


// Console.WriteLine(response.Query);
// Console.WriteLine("----- DATA -----");
// Console.WriteLine(JsonSerializer.Serialize(response.Data, new JsonSerializerOptions
// {
//     WriteIndented = true,
//     DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
// }));

// Console.WriteLine($"Title: {report.Title}");
// Console.WriteLine($"Code: {report.Code}");
// Console.WriteLine($"EndTime: {endTime}");
// Console.WriteLine($"EndTimeOffset: {endTimeOffset}");
Console.WriteLine($"LastReportDate: {gearTrackerConfig.LastReportDate}");
// Console.WriteLine($"endTime > LastReportDate: {endTime > gearTrackerConfig.LastReportDate}");
// Console.WriteLine($"endTimeOffset > LastReportDate: {endTimeOffset > gearTrackerConfig.LastReportDate}");