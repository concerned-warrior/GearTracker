namespace fusion.geartracker.csv;

internal class Program
{
    private static void Main(string[] args)
    {
        var programConfig = ProgramConfig.Load($"{Directory.GetCurrentDirectory()}/../../appsettings/appsettings.json");
        var data = FusionData.Load(programConfig.AppDataPath);

        Console.WriteLine($"We're tracking gear for {data.PlayersByName.Count} players.");
    }
}