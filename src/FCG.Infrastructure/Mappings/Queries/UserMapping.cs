using FCG.Domain.Users;

namespace FCG.Infrastructure.Mappings.Queries;
public class UserMapping : QueryMappingBase<User>
{
    public UserMapping(EntityDefinitionBuilder<User> builder)
        : base(builder, nameof(FCGQueriesDbContext.Users)) { }

    protected override void ConfigureCollection(EntityDefinitionBuilder<User> builder)
    {
        // TODO: Finish the mapping for User collection
        //builder
        //    .HasIndex(u => u.FirstName)
        //    .HasIndex(u => u.LastName);
    }
}
