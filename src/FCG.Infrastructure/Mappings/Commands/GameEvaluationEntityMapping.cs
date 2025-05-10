using FCG.Domain.Games;
using FCG.Infrastructure._Common.Mapping.Commands;

namespace FCG.Infrastructure.Mappings.Commands;
public class GameEvaluationEntityMapping : EntityMappingBase<GameEvaluation>
{
    protected override void ConfigureEntity(EntityTypeBuilder<GameEvaluation> builder)
    {
        builder.HasKey(e => new { e.UserKey, e.GameKey });

        builder
            .HasOne(ge => ge.User)
            .WithMany()
            .HasForeignKey(ge => ge.UserKey);

        builder
            .HasOne(ge => ge.Game)
            .WithMany()
            .HasForeignKey(ge => ge.GameKey);
    }
}
