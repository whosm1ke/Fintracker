using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Identity;

public interface ITokenService
{
    string CreateToken(User user);
}