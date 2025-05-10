using FCG.Domain.Catalogs;
using FCG.Infrastructure._Common.Mapping.Queries;

namespace FCG.Infrastructure.Mappings.Queries.Collections;
public class CatalogCollectionMapping : CollectionMappingBase<Catalog>
{
    public CatalogCollectionMapping(EntityDefinitionBuilder<Catalog> builder)
        : base(builder, nameof(FCGQueriesDbContext.Catalogs)) { }

    protected override void ConfigureCollection(EntityDefinitionBuilder<Catalog> builder)
    {
        builder.HasIndex(c => c.Name, i => i.IsUnique());
    }
}
