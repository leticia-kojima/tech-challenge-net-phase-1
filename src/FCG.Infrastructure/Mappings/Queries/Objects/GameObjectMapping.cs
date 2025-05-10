using FCG.Domain.Games;
using FCG.Infrastructure._Common.Mapping.Queries;

namespace FCG.Infrastructure.Mappings.Queries.Objects;
public class GameObjectMapping : ObjectMappingBase<Game>
{
    public GameObjectMapping(EntityDefinitionBuilder<Game> builder) : base(builder) { }

    protected override void ConfigureObject(EntityDefinitionBuilder<Game> builder)
    {
        builder.Ignore(g => g.Catalog);

        builder.HasIndex(c => c.Title, i => i.IsUnique());
    }
}
