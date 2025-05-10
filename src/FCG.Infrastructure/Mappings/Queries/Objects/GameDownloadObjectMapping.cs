using FCG.Domain.Games;
using FCG.Infrastructure._Common.Mapping.Queries;

namespace FCG.Infrastructure.Mappings.Queries.Objects;
public class GameDownloadObjectMapping : ObjectMappingBase<GameDownload>
{
    public GameDownloadObjectMapping(EntityDefinitionBuilder<GameDownload> builder) : base(builder) { }
    
    protected override void ConfigureObject(EntityDefinitionBuilder<GameDownload> builder)
    {
        builder.Ignore(g => g.Game);
    }
}
