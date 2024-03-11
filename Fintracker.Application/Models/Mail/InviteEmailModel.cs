namespace Fintracker.Application.Models.Mail;

public class InviteEmailModel
{
    public string WhoInvited { get; set; }

    public string Ref { get; set; }

    public string? TempPass { get; set; }
}