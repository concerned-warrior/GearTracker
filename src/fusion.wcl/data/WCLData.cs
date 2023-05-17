namespace fusion.wcl.data;

public class WCLData
{
    [JsonIgnore]
    public Dictionary<string, WCLReport> ReportsByCode { get; set; } = new();
    public HashSet<WCLPlayer> PlayersToTrack { get; set; } = new();
    public HashSet<WCLGear> KnownItems { get; set; } = new();
    public Dictionary<string, WCLPlayer> PlayersByName { get; set; } = new();

    public static JsonSerializerOptions DataJsonSerializerOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter(),
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    private const string reportsDirectoryName = "reports";


    public static WCLData Load (string path)
    {
        WCLData data;

        try
        {
            var dataDirectory = getDataDirectory(path);
            var reportsDirectory = $"{dataDirectory}/{reportsDirectoryName}";

            Directory.CreateDirectory(dataDirectory);
            Directory.CreateDirectory(reportsDirectory);

            var reportFiles = Directory.GetFiles(reportsDirectory, "*.json");
            using var stream = File.OpenRead(path);

            data = JsonSerializer.Deserialize<WCLData>(stream, DataJsonSerializerOptions) ?? new();

            foreach (var file in reportFiles)
            {
                var code = Path.GetFileNameWithoutExtension(file);
                using var reportStream = File.OpenRead(file);

                data.ReportsByCode.Add(code, JsonSerializer.Deserialize<WCLReport>(reportStream, DataJsonSerializerOptions) ?? new());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WCLData Load - {ex.Message}");

            data = new();
        }

        return data;
    }


    public void Save (string path)
    {
        try
        {
            var dataDirectory = getDataDirectory(path);
            var reportsDirectory = $"{dataDirectory}/{reportsDirectoryName}";

            Directory.CreateDirectory(dataDirectory);
            Directory.CreateDirectory(reportsDirectory);

            using var stream = File.Create(path);

            JsonSerializer.Serialize(stream, this, DataJsonSerializerOptions);

            foreach ((var code, var report) in ReportsByCode)
            {
                using var reportStream = File.Create($"{reportsDirectory}/{code}.json");

                JsonSerializer.Serialize(reportStream, report, DataJsonSerializerOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WCLData Save - {ex.Message}");
        }
    }


    private static string getDataDirectory (string path) => Path.GetDirectoryName(path) ?? string.Empty;
}