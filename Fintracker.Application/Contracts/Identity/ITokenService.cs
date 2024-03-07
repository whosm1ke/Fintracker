using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Identity;

public interface ITokenService
{
    Task<string> CreateToken(User user);
}