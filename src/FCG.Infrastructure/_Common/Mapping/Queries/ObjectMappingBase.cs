using FCG.Domain._Common;

namespace FCG.Infrastructure._Common.Mapping.Queries;
public abstract class ObjectMappingBase<TObject> where TObject : EntityBase
{
    private readonly string _collectionName;
    private readonly EntityDefinitionBuilder<TObject> _builder;

    protected ObjectMappingBase(EntityDefinitionBuilder<TObject> builder)
    {
        _builder = builder;
    }

    protected abstract void ConfigureObject(EntityDefinitionBuilder<TObject> builder);

    public void Configure()
    {
        ConfigureObject(_builder);
    }
}
