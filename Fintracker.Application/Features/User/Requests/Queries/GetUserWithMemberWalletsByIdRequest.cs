using Fintracker.Application.DTO.User;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Queries;

public class GetUserWithMemberWalletsByIdRequest : IRequest<UserWithMemberWalletsDTO>
{
    public Guid Id { get; set; }
}