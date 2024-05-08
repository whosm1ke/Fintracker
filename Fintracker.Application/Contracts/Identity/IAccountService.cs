using Fintracker.Application.Models.Identity;
using Fintracker.Domain.Entities;

namespace Fintracker.Application.Contracts.Identity;

public interface IAccountService
{
    Task<RegisterResponse> Register(RegisterRequest register);
    Task<LoginResponse> Login(LoginRequest login);

    Task<string> UpdateUserUsername(string newUsername, Guid userId);

    Task<bool> ResetPassword(ResetPasswordModel model);
    Task<bool> ResetEmail(ResetEmailModel model);

    Task<string> GenerateResetPasswordToken(User user);
    Task<string> GenerateResetEmailToken(User user, string newEmail);
    
    Task Logout();
}