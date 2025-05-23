using FCG.Domain._Common.Settings;
using Microsoft.IdentityModel.Tokens;

namespace FCG.Application._Common.Extensions;
public static class JwtSettingsExtensions
{
    public static SymmetricSecurityKey GetSymmetricSecurityKey(this JwtSettings jwtSettings)
        => new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.Secret));

    public static SigningCredentials GetSigningCredentials(this JwtSettings jwtSettings)
        => new SigningCredentials(
            jwtSettings.GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256
        );
}
