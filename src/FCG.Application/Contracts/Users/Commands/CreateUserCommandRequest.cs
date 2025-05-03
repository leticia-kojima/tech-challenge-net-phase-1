using FCG.Application.Contracts.Users.Abstract;
using MediatR;

namespace FCG.Application.Contracts.Users.Commands;
public class CreateUserCommandRequest : UserCommandModelBase, IRequest<CreateUserCommandResponse>
{
    public CreateUserCommandRequest()
    {
    }
}
