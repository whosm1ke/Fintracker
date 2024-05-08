using Fintracker.Application.Contracts.Helpers;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class SentResetPasswordCommand : IRequest<Unit>, INotGetRequest
{
    public Guid UserId { get; set; } = default!;
    public string Email { get; set; } = default!;

    public string UrlCallback { get; set; } = default!;
}