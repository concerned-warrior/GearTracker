namespace fusion.geartracker.graphql;

public record GuildReportsWithPlayers(int guildID, int limit = 20, int page = 1) : GraphQL<Query, ReportData>
{
    public override ReportData Execute(Query query)
    {
        return query.ReportData(reportData => new ReportData
        {
            __Reports = reportData.Reports(endTime: null, guildID, guildName: null, guildServerSlug: null, guildServerRegion: null, guildTagID: null, userID: null, limit, page, startTime: null, zoneID: null, gameZoneID: null, reportPagination => new ReportPagination
            {
                __Data = reportPagination.Data(report => new Report
                {
                    Code = report.Code,
                    Title = report.Title,
                    StartTime = report.StartTime,
                    EndTime = report.EndTime,
                    ExportedSegments = report.ExportedSegments,
                    Segments = report.Segments,
                    // __MasterData = report.MasterData(translate: null, reportMasterData => new ReportMasterData
                    // {
                    //     __Actors = reportMasterData.Actors(type: "Player", subType: null, reportActor => new ReportActor
                    //     {
                    //         Id = reportActor.Id,
                    //         Name = reportActor.Name,
                    //     })!,
                    // })!,
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