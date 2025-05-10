using FCG.Domain.Games;
using FCG.Infrastructure._Common.Mapping.Queries;

namespace FCG.Infrastructure.Mappings.Queries.Objects;
public class GameEvaluationObjectMapping : ObjectMappingBase<GameEvaluation>
{
    public GameEvaluationObjectMapping(EntityDefinitionBuilder<GameEvaluation> builder) : base(builder) { }

    protected override void ConfigureObject(EntityDefinitionBuilder<GameEvaluation> builder)
    {
        builder.Ignore(g => g.Game);
    }
}
