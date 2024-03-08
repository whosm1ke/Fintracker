using Fintracker.Application.Models.Mail;

namespace Fintracker.Application.Contracts.Infrastructure;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(EmailModel email);

    public InviteEmailModel Model { get; set; }
}