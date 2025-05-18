using FCG.Domain._Common.Abstract;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure._Common.Mapping.Commands;
public abstract class EntityMappingBase<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase
{
    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Key);
        
        builder
            .Property(e => e.Key)
            .ValueGeneratedOnAdd();
        
        builder
            .Property(e => e.CreatedAt)
            .HasColumnType("datetime(6)")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnType("datetime(6)")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        builder
            .Property(e => e.DeletedAt)
            .HasDefaultValue(null);

        builder
            .HasQueryFilter(e => e.DeletedAt == null);

        ConfigureEntity(builder);
    }
}
