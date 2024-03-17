using Fintracker.Application.Models.Identity;

namespace Fintracker.Application.Contracts.Identity;

public interface IAccountService
{
    Task<RegisterResponse> Register(RegisterRequest register);
    Task<LoginResponse> Login(LoginRequest login);

    Task<bool> ResetPassword(ResetPasswordModel model);

    Task<string> GenerateResetPasswordToken(string email);
    
    Task Logout();
}