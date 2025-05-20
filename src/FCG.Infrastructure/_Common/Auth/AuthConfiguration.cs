using FCG.Domain._Common.Consts;
using FCG.Domain._Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FCG.Infrastructure._Common.Auth;
public static class AuthConfiguration
{
    public static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        ArgumentNullException.ThrowIfNull(jwtSettings);

        var symmetricSecurityKey = new SymmetricSecurityKey(
            Convert.FromBase64String(jwtSettings.Secret)
        );

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(bearerOptions =>
        {
            var paramsValidation = bearerOptions.TokenValidationParameters;
            paramsValidation.ValidAudience = jwtSettings.Audience;
            paramsValidation.ValidIssuer = jwtSettings.Issuer;
            paramsValidation.IssuerSigningKey = symmetricSecurityKey;
            paramsValidation.ValidateIssuer = true;
            paramsValidation.ValidateAudience = true;
            paramsValidation.ValidateIssuerSigningKey = true;
            paramsValidation.ValidateLifetime = true;
            paramsValidation.ClockSkew = TimeSpan.Zero;
        });

        return services;
    }

    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(auth =>
        {
            const string claim = ClaimTypes.Role;

            auth.AddPolicy(Policies.OnlyAdmin, p => p
                .RequireClaim(claim, UserRoles.Admin));

            auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}
