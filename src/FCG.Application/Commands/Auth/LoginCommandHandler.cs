using FCG.Application._Common.Extensions;
using FCG.Application.Contracts.Auth;
using FCG.Domain._Common.Settings;
using FCG.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FCG.Application.Commands.Auth;
public class LoginCommandHandler : ICommandHandler<LoginCommandRequest, LoginCommandResponse>
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUserCommandRepository _userCommandRepository;

    public LoginCommandHandler(
        IOptions<JwtSettings> jwtSettings,
        IUserCommandRepository userCommandRepository
    )
    {
        ArgumentNullException.ThrowIfNull(jwtSettings.Value);

        _jwtSettings = jwtSettings.Value;
        _userCommandRepository = userCommandRepository;
    }

    public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new FCGValidationException(
                nameof(request.Email),
                $"{nameof(request.Email)} is required."
            );

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new FCGValidationException(
                nameof(request.Password),
                $"{nameof(request.Password)} is required."
            );

        var user = await _userCommandRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !user.PasswordHash.Matches(request.Password))
            throw new FCGValidationException(nameof(request), $"Invalid credentials for user with email '{request.Email}'.");

        var claims = new ClaimsIdentity([
            new(ClaimTypes.NameIdentifier, user.Key.ToString()),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email)
        ]);

        var utcNow = DateTime.UtcNow;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = claims,
            IssuedAt = utcNow,
            NotBefore = utcNow,
            Expires = utcNow.AddHours(_jwtSettings.ExpirationInHours),
            SigningCredentials = _jwtSettings.GetSigningCredentials()
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return new LoginCommandResponse
        {
            Token = tokenHandler.WriteToken(securityToken),
            Expiration = tokenDescriptor.Expires.Value
        };
    }
}
