using FCG.Domain._Common;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure._Common.Mapping;
public abstract class CommandMappingBase<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase
{
    private readonly string _tableName;

    protected CommandMappingBase(string tableName)
    {
        _tableName = tableName;
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(_tableName);

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
