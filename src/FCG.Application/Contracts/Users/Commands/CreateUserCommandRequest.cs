using FCG.Application._Common.Contracts;
using FCG.Application.Contracts.Users.Abstract;

namespace FCG.Application.Contracts.Users.Commands;
public class CreateUserCommandRequest : UserCommandModelBase, ICommand<CreateUserCommandResponse>
{
    public CreateUserCommandRequest()
    {
    }

    public string Password { get; set; }
}
