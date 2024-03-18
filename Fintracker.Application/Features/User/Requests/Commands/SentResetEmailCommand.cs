using Fintracker.Application.Contracts.Helpers;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class SentResetEmailCommand : IRequest<Unit>, INotGetRequest
{
    public string Email { get; set; } = default!;

    public string UrlCallback { get; set; } = default!;
    public string NewEmail { get; set; } = default!;
}