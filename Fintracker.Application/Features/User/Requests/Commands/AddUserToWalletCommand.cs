using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class AddUserToWalletCommand : IRequest<BaseCommandResponse>, INotGetRequest
{
    public Guid WalletId { get; set; }

    public Guid UserId { get; set; } = default!;

    public string PathToCategories { get; set; } = default!;
}

