using Fintracker.Application.Contracts.Helpers;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class RemoveUserFromWallet : IRequest<Unit>, INotGetRequest
{
    public string Token { get; set; } = default!;
}