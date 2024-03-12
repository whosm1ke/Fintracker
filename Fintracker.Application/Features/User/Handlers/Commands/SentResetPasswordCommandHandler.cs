using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Commands;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class SentResetPasswordCommandHandler : IRequestHandler<SentResetPasswordCommand, Unit>
{
    private readonly IAccountService _accountService;
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;

    public SentResetPasswordCommandHandler(IAccountService accountService,
        IUserRepository userRepository, IEmailSender emailSender, IOptions<AppSettings> appSettings)
    {
        _accountService = accountService;
        _userRepository = userRepository;
        _emailSender = emailSender;
        _appSettings = appSettings.Value;
    }

    public async Task<Unit> Handle(SentResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email);

        if (user is null)
            throw new BadRequestException(new ExceptionDetails
            {
                PropertyName = nameof(request.Email),
                ErrorMessage = "Invalid email. Check spelling."
            });

        var token = await _accountService.GenerateResetPasswordToken(request.Email);

        await _emailSender.SendEmail(new()
        {
            Email = request.Email,
            Subject = "Reset Password Confirmation",
            HtmlPath = "resetPassword.html"
        }, new { Ref = $"{_appSettings.BaseUrl}/{request.UrlCallback}?token={token}" });

        return Unit.Value;
    }
}