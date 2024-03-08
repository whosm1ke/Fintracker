using System.IdentityModel.Tokens.Jwt;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Identity;

public interface ITokenService
{
    Task<string> CreateLoginToken(User user);

    Task<string> CreateInviteToken(string email);

    Task<Tuple<bool, JwtSecurityToken>> ValidateToken(string token);
}