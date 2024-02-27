using Fintracker.Application.DTO.User;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Queries;

public class GetUserWithWalletsByIdRequest : IRequest<UserWithWalletsDTO>
{
    public Guid Id { get; set; }
}