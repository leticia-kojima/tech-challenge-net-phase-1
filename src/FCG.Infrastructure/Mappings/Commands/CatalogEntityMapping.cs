using FCG.Domain.Catalogs;
using FCG.Infrastructure._Common.Mapping.Commands;

namespace FCG.Infrastructure.Mappings.Commands;
public class CatalogEntityMapping : EntityMappingBase<Catalog>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Catalog> builder)
    {
        builder
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(150);
        
        builder
            .Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(500);
        
        builder
            .HasIndex(c => c.Name)
            .IsUnique();
    }
}
