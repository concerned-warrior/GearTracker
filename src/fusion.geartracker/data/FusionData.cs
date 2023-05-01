namespace fusion.geartracker.data;

public class FusionData
{
    public Dictionary<string, FusionReport> ReportsByCode { get; set; } = new();
    public Dictionary<string, FusionPlayer> PlayersByName { get; set; } = new();
    public Dictionary<string, HashSet<string>> ReportCodesByPlayer { get; set; } = new();


    public static FusionData Load (string path)
    {
        FusionData data;

        try
        {
            using var stream = File.OpenRead(path);

            data = JsonSerializer.Deserialize<FusionData>(stream, DataService.DataJsonSerializerOptions) ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"FusionData Load - {ex.Message}");

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
            Console.WriteLine($"FusionData Save - {ex.Message}");
        }
    }
}