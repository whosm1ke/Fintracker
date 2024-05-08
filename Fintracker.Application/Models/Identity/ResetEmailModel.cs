namespace Fintracker.Application.Models.Identity;

public class ResetEmailModel
{
    public Guid UserId { get; set; } = default!;
    public string NewEmail { get; set; } = default!;
    public string Token { get; set; } = default!;
}