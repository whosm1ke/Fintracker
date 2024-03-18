namespace Fintracker.Application.Models.Mail;

public class InviteEmailModel
{
    public string WhoInvited { get; set; } = default!;

    public string Ref { get; set; } = default!;
    public string DeclineRef { get; set; } = default!;

    public string? TempPass { get; set; }
}