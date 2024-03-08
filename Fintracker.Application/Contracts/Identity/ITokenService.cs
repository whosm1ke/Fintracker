using System.IdentityModel.Tokens.Jwt;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Identity;

public interface ITokenService
{
    Task<string> CreateToken(User user);

    Task<Tuple<bool, JwtSecurityToken>> ValidateToken(string token);
}