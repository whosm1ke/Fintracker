using System.Text;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Models.Mail;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class InviteUserCommandHandler : IRequestHandler<InviteUserCommand, Unit>
{
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly AppSettings _appSettings;
    private readonly string _tempPass;

    public InviteUserCommandHandler(IEmailSender emailSender, IUserRepository userRepository,
        ITokenService tokenService, IOptions<AppSettings> options)
    {
        _emailSender = emailSender;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _appSettings = options.Value;
        _tempPass = GenerateTempPassword();
    }

    public async Task<Unit> Handle(InviteUserCommand request, CancellationToken cancellationToken)
    {
        var inviteEmailModel = new InviteEmailModel
        {
            WhoInvited = request.WhoInvited ?? "User",
            TempPass = _tempPass
        };

        var isUserExists = await _userRepository.ExistsAsync(request.UserEmail);
        var user = new Domain.Entities.User();

        if (!isUserExists)
        {
            user = await _userRepository.RegisterUserWithTemporaryPassword(request.UserEmail, Guid.NewGuid(),
                _tempPass);
            inviteEmailModel.Ref =
                $"{_appSettings.UiUrl}/{request.UrlCallback}?userId={user.Id}&walletId={request.WalletId}";

        }
        else
        {
            var existingUser = await _userRepository.GetAsNoTrackingAsync(request.UserEmail);
            
            inviteEmailModel.Ref =
                $"{_appSettings.UiUrl}/{request.UrlCallback}?userId={existingUser!.Id}&walletId={request.WalletId}";

        }

        var isEmailSent = await _emailSender.SendEmail(new EmailModel
        {
            Email = request.UserEmail,
            Subject = "Invitation to a new wallet",
            HtmlPath = isUserExists ? "inviteForm.html" : "inviteFormWithNewUser.html",
            Name = "",
            PlainMessage = ""
        }, inviteEmailModel);

        if (!isEmailSent)
        {
            await _userRepository.DeleteAsync(user);
            throw new BadRequestException(new ExceptionDetails
            {
                PropertyName = "Email",
                ErrorMessage = $"Was not sent to {request.UserEmail}. Check spelling"
            });
        }

        return Unit.Value;
    }


    private string GenerateTempPassword()
    {
        string chars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
        string signs = "!@#$%^&*(){}:'~`";
        var sb = new StringBuilder();
        for (int i = 0; i < 8; i++)
        {
            sb.Append(chars[Random.Shared.Next(0, chars.Length)]);
        }

        sb.Append(chars[Random.Shared.Next(26, chars.Length)]);
        sb.Append(signs[Random.Shared.Next(0, signs.Length)]);

        for (int i = 0; i < 3; i++)
        {
            sb.Append(Random.Shared.Next(0, 9));
        }

        return sb.ToString();
    }
}