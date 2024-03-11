using System.Net;
using System.Net.Mail;
using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Models.Mail;
using FluentEmail.Core;
using FluentEmail.Core.Defaults;
using FluentEmail.Smtp;
using Microsoft.Extensions.Options;

namespace Fintracker.Infrastructure.Mail;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;
    private readonly IHtmlPageHelper _htmlPageHelper;
    private readonly SmtpSender _sender;

    public EmailSender(IOptions<EmailSettings> emailOptions, IHtmlPageHelper htmlPageHelper)
    {
        _htmlPageHelper = htmlPageHelper;
        _emailSettings = emailOptions.Value;
        _sender = new SmtpSender(() => new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailSettings.FromAddress, _emailSettings.FromPassword)
        });
    }

    public async Task<bool> SendEmail<T>(EmailModel email, T model)
    {
        var htmlMsg = email.HtmlPath == string.Empty
            ? email.HtmlMessage
            : await _htmlPageHelper.GetPageContent(email.HtmlPath);
        
        Email.DefaultSender = _sender;
        Email.DefaultRenderer = new ReplaceRenderer();
        var emailToSend = await Email
            .From(_emailSettings.FromAddress)
            .To(email.Email, email.Name)
            .Subject(email.Subject)
            .UsingTemplate(htmlMsg, model)
            .SendAsync();


        return emailToSend.Successful;
    }
}