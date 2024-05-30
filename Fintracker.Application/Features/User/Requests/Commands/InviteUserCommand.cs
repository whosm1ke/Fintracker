using Fintracker.Application.Contracts.Helpers;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class InviteUserCommand : IRequest<Unit>, INotGetRequest
{
    public Guid WalletId { get; set; }
    public string UserEmail { get; set; } = default!;
    public string? WhoInvited { get; set; }

    public string UrlCallback { get; set; } = default!;
}