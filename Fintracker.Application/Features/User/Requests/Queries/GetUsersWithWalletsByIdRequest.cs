using Fintracker.Application.DTO.User;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Queries;

public class GetUsersWithWalletsByIdRequest : IRequest<IReadOnlyList<UserWithWalletsDTO>>
{
    public Guid Id { get; set; }
}