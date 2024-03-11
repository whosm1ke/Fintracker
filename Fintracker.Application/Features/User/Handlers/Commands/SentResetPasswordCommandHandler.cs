using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class SentResetPasswordCommandHandler : IRequestHandler<SentResetPasswordCommand, Unit>
{
    private readonly IAccountService _accountService;
    private readonly IEmailSender _emailSender;
    private readonly AppSettings _appSettings;
    private readonly UserManager<Domain.Entities.User> _userManager;

    public SentResetPasswordCommandHandler(IAccountService accountService,
        UserManager<Domain.Entities.User> userManager, IEmailSender emailSender, IOptions<AppSettings> appSettings)
    {
        _accountService = accountService;
        _userManager = userManager;
        _emailSender = emailSender;
        _appSettings = appSettings.Value;
    }

    public async Task<Unit> Handle(SentResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            throw new BadRequestException("Invalid email");

        var token = await _accountService.GenerateResetPasswordToken(request.Email);

        await _emailSender.SendEmail(new()
        {
            Email = request.Email,
            Subject = "Reset Password Confirmation",
            HtmlPath = "resetPassword.html",
        }, new { Ref = $"{_appSettings.BaseUrl}/{request.UrlCallback}?token={token}"});

        return Unit.Value;
    }
}