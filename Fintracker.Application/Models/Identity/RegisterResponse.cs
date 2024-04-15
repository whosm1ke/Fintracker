namespace Fintracker.Application.Models.Identity;

public class RegisterResponse
{
    public Guid UserId { get; set; }

    public string Email { get; set; } = default!;
}