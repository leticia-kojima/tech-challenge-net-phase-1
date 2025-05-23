using FCG.Domain.Users;
using FCG.Domain.ValueObjects;
using FCG.Infrastructure._Common.Interfaces;

namespace FCG.Infrastructure.Seeders;
public class UserSeeder : ISeeder<User>
{
    // TODO: Update the hash pass
    public IReadOnlyCollection<User> GetData() => new List<User> {
        new (
            new ("a6b04fe0-e7e1-4385-996f-5525a955734f"),
            "Administrador",
            new("admin@fcg.test.com.br"),
            ERole.Admin,
            Password.FromHash("$2a$11$PCCNp26qNsiEdRsX1iStfOe5mDjfnK2UL/tFnYT96CIOjMvCBWVbe")
        ),
        new (
            new ("7d1456ec-272a-4ad6-9ee7-2281e84d68c0"),
            "Usuário",
            new("user@fcg.test.com.br"),
            ERole.User,
            Password.FromHash("$2a$11$TKxDQGXY/rzdA0LJBE3.yO6FGHrgwMplsOkiSctXl4nBNaJlS9h6y")
        )
    };
}
