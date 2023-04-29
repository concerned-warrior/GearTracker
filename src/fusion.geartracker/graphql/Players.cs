namespace fusion.geartracker.graphql;

public record Players(string code) : GraphQL<Query, ReportData>
{
    public override ReportData Execute(Query query)
    {
        return query.ReportData(reportData => new ReportData
        {
            __Report = reportData.Report(code, report => new Report
            {
                __MasterData = report.MasterData(translate: null, reportMasterData => new ReportMasterData
                {
                    __Actors = reportMasterData.Actors(type: "Player", subType: null, reportActor => new ReportActor
                    {
                        Id = reportActor.Id,
                        Name = reportActor.Name,
                    })!,
                })!,
            })!,
        })!;
    }
}