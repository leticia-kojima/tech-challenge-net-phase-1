using FCG.Domain._Common;
using FCG.Domain.Users;

namespace FCG.Infrastructure._Common.Mapping;
public abstract class QueryMappingBase<TEntity> where TEntity : EntityBase
{
    private readonly string _collectionName;
    private readonly EntityDefinitionBuilder<User> _builder;

    protected QueryMappingBase(EntityDefinitionBuilder<User> builder, string collectionName)
    {
        _builder = builder;
        _collectionName = collectionName;
    }

    protected abstract void ConfigureCollection(EntityDefinitionBuilder<User> builder);

    public void Configure()
    {
        _builder.ToCollection(_collectionName);

        ConfigureCollection(_builder);
    }
}
