using MediatR;

namespace FCG.Application.Contracts.Users;
public class CreateUserCommandRequest : UserCommandModelBase, IRequest<CreateUserCommandResponse>
{

}
