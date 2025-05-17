namespace FCG.Application.Contracts.Auth;
public class LoginCommandRequest : ICommand<LoginCommandResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

