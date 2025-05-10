using FCG.Domain.Games;
using FCG.Infrastructure._Common.Mapping.Commands;

namespace FCG.Infrastructure.Mappings.Commands;
public class GameEntityMapping : EntityMappingBase<Game>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Game> builder)
    {
        builder
            .Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder
            .Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder
            .HasOne(g => g.Catalog)
            .WithMany(c => c.Games)
            .HasForeignKey(g => g.CatalogKey);

        builder.HasIndex(g => g.Title)
            .IsUnique();
    }
}
