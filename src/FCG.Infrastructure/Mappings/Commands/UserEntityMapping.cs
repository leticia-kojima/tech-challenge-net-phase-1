using FCG.Domain.Users;
using FCG.Domain.ValueObjects;
using FCG.Infrastructure._Common.Mapping.Commands;

namespace FCG.Infrastructure.Mappings.Commands;
public class UserEntityMapping : EntityMappingBase<User>
{
    protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder
            .Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(150)
            .HasConversion(
                v => v.ToString(),
                v => new Email(v)
            );

        builder
            .Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(450)
            .HasConversion(
                v => v.Hash,
                v => Password.FromHash(v)
            );

        builder
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
