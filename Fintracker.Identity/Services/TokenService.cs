using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Fintracker.Identity.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _cfg;
    private readonly SymmetricSecurityKey _key;
    public TokenService(IConfiguration cfg)
    {
        _cfg = cfg;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["JWT:SigningKey"]!));
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Sub, user.UserName!),
            new("Uid", user.Id.ToString()),
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
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
}