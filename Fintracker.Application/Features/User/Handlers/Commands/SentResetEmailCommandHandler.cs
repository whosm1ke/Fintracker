using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.User.Requests.Commands;
using MediatR;
using Microsoft.Extensions.Options;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class SentResetEmailCommandHandler : IRequestHandler<SentResetEmailCommand, Unit>
{
    private readonly IAccountService _accountService;
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;

    public SentResetEmailCommandHandler(IAccountService accountService, IEmailSender emailSender,
        IUserRepository userRepository, IOptions<AppSettings> appSettings)
    {
        _accountService = accountService;
        _emailSender = emailSender;
        _userRepository = userRepository;
        _appSettings = appSettings.Value;
    }

    public async Task<Unit> Handle(SentResetEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsNoTrackingAsync(request.UserId);

        if (user is null)
            throw new BadRequestException(new ExceptionDetails
            {
                PropertyName = nameof(request.UserId),
                ErrorMessage = "Invalid email. Check spelling."
            });

        var token = await _accountService.GenerateResetEmailToken(user, request.NewEmail);

        await _emailSender.SendEmail(new()
        {
            Email = request.NewEmail,
            Subject = "Reset Email Confirmation",
            HtmlPath = "resetEmail.html"
        }, new { Ref = $"{_appSettings.UiUrl}/{request.UrlCallback}?token={token}&userId={request.UserId}&newEmail={request.NewEmail}" });

        return Unit.Value;
    }
}