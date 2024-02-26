using Fintracker.Application.DTO.User;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Queries;

public class GetUserByIdRequest : IRequest<UserDTO>
{
    public Guid Id { get; set; }
}