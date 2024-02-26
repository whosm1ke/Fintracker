using Fintracker.Application.DTO.User;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Queries;

public class GetUserByIdRequest : IRequest<UserBaseDTO>
{
    public Guid Id { get; set; }
}