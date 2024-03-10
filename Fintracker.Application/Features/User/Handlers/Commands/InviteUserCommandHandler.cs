using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Invite.Validators;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Models.Mail;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class InviteUserCommandHandler : IRequestHandler<InviteUserCommand, Unit>
{
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IHtmlPageHelper _htmlPageHelper;
    private readonly AppSettings _appSettings;

    public InviteUserCommandHandler(IEmailSender emailSender, IUserRepository userRepository, IUnitOfWork unitOfWork,
        ITokenService tokenService, IOptions<AppSettings> options, IHtmlPageHelper htmlPageHelper)
    {
        _emailSender = emailSender;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _htmlPageHelper = htmlPageHelper;
        _appSettings = options.Value;
    }

    public async Task<Unit> Handle(InviteUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new InviteUseValidator(_userRepository, _unitOfWork);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid)
        {
            var inviteEmailModel = new InviteEmailModel()
            {
                WhoInvited = request.WhoInvited ?? "User"
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
            var res = await _emailSender.SendEmailAsync(new EmailModel()
            {
                Email = request.UserEmail,
                Subject = $"{inviteEmailModel.WhoInvited} invites you to join his wallet",
                HtmlMessage = await _htmlPageHelper.GetPageContent("inviteForm.html"),
                Name = "",
                PlainMessage = ""
            });
        }


        return Unit.Value;
    }
}