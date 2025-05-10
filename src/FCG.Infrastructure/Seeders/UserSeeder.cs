using FCG.Domain.Users;
using FCG.Infrastructure._Common.Interfaces;

namespace FCG.Infrastructure.Seeders;
public class UserSeeder : ISeeder<User>
{
    // TODO: Update the hash pass
    public IReadOnlyCollection<User> GetData() => new List<User> {
        new (
            new ("a6b04fe0-e7e1-4385-996f-5525a955734f"),
            "Administrador",
            "admin@fcg.test.com.br",
            ERole.Admin,
            "7d6721d6-6cb4-4ade-aab2-38a549964b09"
        ),
        new (
            new ("7d1456ec-272a-4ad6-9ee7-2281e84d68c0"),
            "Usuário",
            "user@fcg.test.com.br",
            ERole.User,
            "7d6721d6-6cb4-4ade-aab2-38a549964b09"
        )
    };
}
