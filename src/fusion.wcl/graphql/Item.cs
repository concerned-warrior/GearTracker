namespace fusion.wcl.graphql;

public record Item(int id) : GraphQL<Query, GameData>
{
    public override GameData Execute(Query query)
    {
        return query.GameData(gameData => new GameData
        {
            __Item = gameData.Item(id, item => new GameItem
            {
                Id = item.Id,
                Icon = item.Icon,
                Name = item.Name,
            })!,
        })!;
    }
}