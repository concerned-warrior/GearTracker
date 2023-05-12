namespace fusion.wcl.graphql;

public record Reports(int guildId, int limit = 50, int page = 1) : GraphQL<Query, ReportData>
{
    public override ReportData Execute(Query query)
    {
        return query.ReportData(reportData => new ReportData
        {
            __Reports = reportData.Reports(endTime: null, guildID: guildId, guildName: null, guildServerSlug: null, guildServerRegion: null, guildTagID: null, userID: null, limit, page, startTime: null, zoneID: null, gameZoneID: null, reportPagination => new ReportPagination
            {
                __Data = reportPagination.Data(report => new Report
                {
                    Code = report.Code,
                    Title = report.Title,
                    StartTime = report.StartTime,
                    EndTime = report.EndTime,
                    ExportedSegments = report.ExportedSegments,
                    Segments = report.Segments,
                })!,
                Total = reportPagination.Total,
                From = reportPagination.From,
                To = reportPagination.To,
                Per_page = reportPagination.Per_page,
                Current_page = reportPagination.Current_page,
                Last_page = reportPagination.Last_page,
                Has_more_pages = reportPagination.Has_more_pages,
            })!,
        })!;
    }
}