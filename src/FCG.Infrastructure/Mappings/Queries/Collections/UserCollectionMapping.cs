using FCG.Domain.Users;
using FCG.Infrastructure._Common.Mapping.Queries;

namespace FCG.Infrastructure.Mappings.Queries.Collections;
public class UserCollectionMapping : CollectionMappingBase<User>
{
    public UserCollectionMapping(EntityDefinitionBuilder<User> builder)
        : base(builder, nameof(FCGQueriesDbContext.Users)) { }

    protected override void ConfigureCollection(EntityDefinitionBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email, i => i.IsUnique());
    }
}
