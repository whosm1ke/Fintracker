using System.Text;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Invite.Validators;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Models.Mail;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class InviteUserCommandHandler : IRequestHandler<InviteUserCommand, Unit>
{
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppSettings _appSettings;

    public InviteUserCommandHandler(IEmailSender emailSender, IUserRepository userRepository, IUnitOfWork unitOfWork,
        ITokenService tokenService, IOptions<AppSettings> options, IHttpContextAccessor httpContextAccessor)
    {
        _emailSender = emailSender;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _appSettings = options.Value;
    }

    public async Task<Unit> Handle(InviteUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new InviteUseValidator(_userRepository, _unitOfWork, _httpContextAccessor);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid)
        {
            var whoInvited = _httpContextAccessor.HttpContext?.User.FindFirst("name")?.Value;
            var inviteEmailModel = new InviteEmailModel()
            {
                WhoInvited = whoInvited ?? "User"
            };
            var existingUser = await _userRepository.GetAsNoTrackingAsync(request.UserEmail);

            if (existingUser == null)
            {
                var token = await _tokenService.CreateInviteToken(request.UserEmail);
                inviteEmailModel.Ref =
                    $"{_appSettings.BaseUrl}/api/account/invite/accept?token={token}&walletId={request.WalletId}";
            }
            else
            {
                inviteEmailModel.WalletId = request.WalletId;
                var token =
                    await _tokenService.CreateLoginToken(
                        (await _userRepository.GetAsNoTrackingAsync(request.UserEmail))!);

                inviteEmailModel.Ref = $"{_appSettings.BaseUrl}/api/account/invite/add-wallet?token={token}";
            }


            _emailSender.Model = inviteEmailModel;
            await _emailSender.SendEmailAsync(new EmailModel()
            {
                Email = request.UserEmail,
                Subject = $"{inviteEmailModel.WhoInvited} invites you to join his wallet",
                HtmlMessage = GetHtmlInviteString(),
                Name = "",
                PlainMessage = ""
            });
        }


        return Unit.Value;
    }

    private string GetHtmlInviteString()
    {
        var sb = new StringBuilder();
        sb.Append("<!DOCTYPE html>");
        sb.Append("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<style>");
        sb.AppendLine("body {");
        sb.AppendLine("font-family: Arial, sans-serif;}");
        sb.AppendLine(".container {");
        sb.AppendLine("width: 80%; margin: auto; padding: 20px; border: 1px solid #ddd;border-radius: 5px;}");
        sb.AppendLine(".button {");
        sb.AppendLine(
            "display: inline-block; padding: 10px 20px; margin: 20px 0; color: #fff; background-color: #007BFF; border-radius: 5px;" +
            "text-decoration: none;}");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>" +
                      "<body>" +
                      "<div class='container'>" +
                      "<h2>You've been invited!</h2>" +
                      "<p>Hello,</p>" +
                      "<p>##WhoInvited## has invited you to join our platform. Click the link below to accept the invitation:</p>" +
                      " <form action='##Ref##' method='post'>" +
                      " <input type='text' name='walletId' value='##WalletId##' hidden>" +
                      "  <input type='submit' class='button' value='Submit'>" +
                      " </form>" +
                      "<p>If you have any questions, feel free to reply to this email.</p>" +
                      "<p>Best regards,</p>" +
                      "<p>The Team</p>" +
                      "</div>" +
                      "</body>" +
                      "</html>");
        return sb.ToString();
    }
}