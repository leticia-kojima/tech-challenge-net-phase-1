using FCG.Domain.Users;

namespace FCG.Infrastructure.Mappings.Commands;
public class UserMapping : CommandMappingBase<User>
{
    public UserMapping() : base(nameof(FCGCommandsDbContext.Users)) { }

    protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        // TODO: Finish the mapping for User entity
        //builder
        //    .Property(e => e.FirstName)
        //    .IsRequired()
        //    .HasMaxLength(50);

        //builder
        //    .Property(e => e.LastName)
        //    .IsRequired()
        //    .HasMaxLength(50);
    }
}
