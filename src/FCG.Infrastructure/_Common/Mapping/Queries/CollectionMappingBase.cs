using FCG.Domain._Common;

namespace FCG.Infrastructure._Common.Mapping.Queries;
public abstract class CollectionMappingBase<TCollection> where TCollection : EntityBase
{
    private readonly string _collectionName;
    private readonly EntityDefinitionBuilder<TCollection> _builder;

    protected CollectionMappingBase(EntityDefinitionBuilder<TCollection> builder, string collectionName)
    {
        _builder = builder;
        _collectionName = collectionName;
    }

    protected abstract void ConfigureCollection(EntityDefinitionBuilder<TCollection> builder);

    public void Configure()
    {
        _builder.ToCollection(_collectionName);

        ConfigureCollection(_builder);
    }
}
