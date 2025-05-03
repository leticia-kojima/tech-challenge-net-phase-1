using FCG.Domain._Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder
            .Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder
            .Property(e => e.DeletedAt)
            .HasDefaultValue(null);

        ConfigureEntity(builder);
    }
}
