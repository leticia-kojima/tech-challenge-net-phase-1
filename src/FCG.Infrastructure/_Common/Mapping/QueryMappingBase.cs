using FCG.Domain._Common;
using MongoDB.Bson.Serialization;

namespace FCG.Infrastructure._Common.Mapping;
public abstract class QueryMappingBase<TEntity> where TEntity : EntityBase
{
    protected virtual void ConfigureCollection(BsonClassMap<TEntity> builder) { }

    public QueryMappingBase()
    {
        BsonClassMap.RegisterClassMap<TEntity>(cm =>
        {
            cm.AutoMap();
            ConfigureCollection(cm);
        });
    }
}
