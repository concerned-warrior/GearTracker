namespace fusion.geartracker.graphql;

public record Gear(string code, double startTime, double endTime, int playerId) : GraphQL<Query, ReportData>
{
    public override ReportData Execute(Query query)
    {
        return query.ReportData(reportData => new ReportData
        {
            __Report = reportData.Report(code, report => new Report
            {
                __Events = report.Events(abilityID: null, dataType: EventDataType.CombatantInfo, death: null, difficulty: null, encounterID: null, endTime, fightIDs: null, filterExpression: null, hostilityType: null, includeResources: null, killType: null, limit: null, sourceAurasAbsent: null, sourceAurasPresent: null, sourceClass: "Any", sourceID: playerId, sourceInstanceID: null, startTime, targetAurasAbsent: null, targetAurasPresent: null, targetClass: null, targetID: null, targetInstanceID: null, translate: null, useAbilityIDs: null, useActorIDs: null, viewOptions: null, wipeCutoff: null, reportEventPaginator => new ReportEventPaginator
                {
                    Data = reportEventPaginator.Data,
                    NextPageTimestamp = reportEventPaginator.NextPageTimestamp,
                })!,
            })!,
        })!;
    }
}