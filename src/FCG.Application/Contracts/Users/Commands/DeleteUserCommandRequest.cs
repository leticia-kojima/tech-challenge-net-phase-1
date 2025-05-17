namespace FCG.Application.Contracts.Users.Commands;
public class DeleteUserCommandRequest : ICommand
{
    public Guid Key { get; set; }
}
