namespace Fintracker.Application.Models.Identity;

public class LoginRequest
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}