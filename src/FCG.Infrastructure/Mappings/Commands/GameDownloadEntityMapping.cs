using FCG.Domain.Games;
using FCG.Infrastructure._Common.Mapping.Commands;

namespace FCG.Infrastructure.Mappings.Commands;
public class GameDownloadEntityMapping : EntityMappingBase<GameDownload>
{
    protected override void ConfigureEntity(EntityTypeBuilder<GameDownload> builder)
    {
        builder.HasKey(e => new { e.UserKey, e.GameKey });

        builder
            .HasOne(gd => gd.User)
            .WithMany()
            .HasForeignKey(gd => gd.UserKey);

        builder
            .HasOne(gd => gd.Game)
            .WithMany(g => g.Downloads)
            .HasForeignKey(gd => gd.GameKey);
    }
}
