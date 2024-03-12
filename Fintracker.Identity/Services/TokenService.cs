using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Fintracker.Identity.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _cfg;
    private readonly SymmetricSecurityKey _key;
    private readonly UserManager<User> _userManager;
    public TokenService(IConfiguration cfg, UserManager<User> userManager)
    {
        _cfg = cfg;
        _userManager = userManager;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["JWT:SigningKey"]!));
    }

    public async Task<string> CreateToken(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim(ClaimTypeConstants.Role, roles[i]));
        }

        var claims = new[]
            {
                new Claim(ClaimTypeConstants.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypeConstants.Email, user.Email!),
                new Claim(ClaimTypeConstants.Uid, user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            Issuer = _cfg["JWT:Issuer"],
            Audience = _cfg["JWT:Audience"],
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    

    public async Task<Tuple<bool, JwtSecurityToken>> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        var validationResult = await tokenHandler.ValidateTokenAsync(token, tokenValidationParams);
        return new Tuple<bool, JwtSecurityToken>(validationResult.IsValid,(JwtSecurityToken)validationResult.SecurityToken);
    }
}