using Fintracker.Application.DTO.Account;

namespace Fintracker.Application.Contracts.Identity;

public interface IAccountService
{
    Task<RegisterResponse> Register(RegisterRequest register);
    Task<LoginResponse> Login(LoginRequest login);

    Task Logout();
}