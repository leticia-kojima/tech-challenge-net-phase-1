using FCG.Application._Common.Contracts;
using FCG.Application.Contracts.Users.Abstract;

namespace FCG.Application.Contracts.Users.Commands;
public class UpdateUserCommandRequest : UserCommandModelBase, ICommand<UpdateUserCommandResponse>
{
    public UpdateUserCommandRequest()
    {
    }

    public Guid Key { get; set; }
}
