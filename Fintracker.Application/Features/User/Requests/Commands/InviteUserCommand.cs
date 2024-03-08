using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class InviteUserCommand : IRequest<Unit>
{
    public string WhoInvited { get; set; }

    public Guid WalletId { get; set; }
    public string UserEmail { get; set; }
    
    public string CurrentUserEmail { get; set; }
}