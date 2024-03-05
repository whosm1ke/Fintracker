using Fintracker.Application.DTO.User;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Queries;

public class GetUserWithOwnedWalletsByIdRequest : IRequest<UserWithOwnedWalletsDTO>
{
    public Guid Id { get; set; }
}