using System.Text;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Fintracker.Identity;

public static class ConfigureIdentity
{
    public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme =
                o.DefaultChallengeScheme =
                    o.DefaultForbidScheme =
                        o.DefaultScheme =
                            o.DefaultSignInScheme =
                                o.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = cfg["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = cfg["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["JWT:SigningKey"]!))
            };
            
    
        }).AddCookie(IdentityConstants.ApplicationScheme);

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}