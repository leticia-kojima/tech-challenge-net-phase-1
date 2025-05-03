using FCG.Domain.Users;
using FCG.Infrastructure._Common.Mapping;
using FCG.Infrastructure.Contexts.FCGCommands;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Mappings.Commands;
public class UserMapping : CommandMappingBase<User>
{
    public UserMapping() : base(nameof(FCGCommandsDbContext.Users)) { }

    protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(e => e.FistName)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);
    }
}
