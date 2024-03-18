namespace Fintracker.Application.Models.Identity;

public class ResetEmailRequest : ResetRequestBase
{
    public string NewEmail { get; set; } = default!;
}