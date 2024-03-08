using System.Net;
using System.Net.Mail;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Models.Mail;
using FluentEmail.Core;
using FluentEmail.Core.Defaults;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Fintracker.Infrastructure.Mail;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;
    private readonly IWebHostEnvironment _hostEnvironment;
    public InviteEmailModel Model { get; set; }

    public EmailSender(IOptions<EmailSettings> emailOptions, IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _emailSettings = emailOptions.Value;
    }

    public async Task<bool> SendEmailAsync(EmailModel email)
    {
        var sender = new SmtpSender(() => new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailSettings.FromAddress, _emailSettings.FromPassword)
        });

        Email.DefaultSender = sender;
        Email.DefaultRenderer = new ReplaceRenderer();
        var emailToSend = await Email
            .From(_emailSettings.FromAddress)
            .To(email.Email, email.Name)
            .Subject(email.Subject)
            .UsingTemplate(email.HtmlMessage, Model)
            .SendAsync();


        return emailToSend.Successful;
    }
}