namespace fusion.geartracker;

public class DataService
{
    private ProgramConfig programConfig;
    private WCLGraphQLClient graphQLClient;


    public async Task<List<FusionReport>> GetReports ()
    {
        ReportPagination reportPagination = new();
        FusionReport lastReport = new();
        List<FusionReport> reports = new();

        do
        {
            var result = await graphQLClient.Execute(new Reports(programConfig.GuildId, page: reportPagination.Current_page + 1));

            reportPagination = result.Data?.__Reports ?? new();

            foreach (var report in reportPagination.__Data)
            {
                lastReport = FusionReport.FromReport(report);

                if (lastReport.StartTime < programConfig.FirstReportDate && lastReport.EndTime > programConfig.LastReportDate)
                {
                    reports.Add(lastReport);
                }
            }
        } while (lastReport.EndTime > programConfig.LastReportDate && reportPagination.Has_more_pages);

        return reports;
    }


    public FusionData Load ()
    {
        FusionData data;

        try
        {
            using var stream = File.OpenRead(programConfig.AppDataPath);

            data = JsonSerializer.Deserialize<FusionData>(stream) ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DataService Load - {ex.Message}");

            data = new();
        }

        return data;
    }


    public void Save (FusionData data)
    {
        try
        {
            using var stream = File.OpenWrite(programConfig.AppDataPath);

            JsonSerializer.Serialize(stream, data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DataService Save - {ex.Message}");
        }
    }


    public DataService (WCLGraphQLClient graphQLClient, ProgramConfig programConfig)
    {
        this.programConfig = programConfig;
        this.graphQLClient = graphQLClient;
    }
}