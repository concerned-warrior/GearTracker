namespace fusion.geartracker.data;

public class WCLData
{
    public Dictionary<string, WCLReport> ReportsByCode { get; set; } = new();
    public Dictionary<string, WCLPlayer> PlayersByName { get; set; } = new();
    public Dictionary<string, HashSet<string>> ReportCodesByPlayer { get; set; } = new();


    public static WCLData Load (string path)
    {
        WCLData data;

        try
        {
            using var stream = File.OpenRead(path);

            data = JsonSerializer.Deserialize<WCLData>(stream, DataService.DataJsonSerializerOptions) ?? new();
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
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);

            using var stream = File.OpenWrite(path);

            JsonSerializer.Serialize(stream, this, DataService.DataJsonSerializerOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WCLData Save - {ex.Message}");
        }
    }
}