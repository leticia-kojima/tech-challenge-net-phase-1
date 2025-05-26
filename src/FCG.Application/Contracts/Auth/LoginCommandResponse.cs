namespace FCG.Application.Contracts.Auth;
public class LoginCommandResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
