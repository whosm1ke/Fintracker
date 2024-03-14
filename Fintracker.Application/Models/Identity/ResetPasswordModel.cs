namespace Fintracker.Application.Models.Identity;

public class ResetPasswordModel
{
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string Password { get; set; } = default!;
}