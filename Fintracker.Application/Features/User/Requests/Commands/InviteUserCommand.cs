using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class InviteUserCommand : IRequest<Unit>
{
    public Guid WalletId { get; set; }
    public string UserEmail { get; set; } = default!;
}