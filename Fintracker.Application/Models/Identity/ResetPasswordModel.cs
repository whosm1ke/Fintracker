namespace Fintracker.Application.Models.Identity;

public class ResetPasswordModel
{
    public Guid UserId { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string Password { get; set; } = default!;
}