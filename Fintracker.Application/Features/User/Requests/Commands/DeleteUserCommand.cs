using Fintracker.Application.DTO.User;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class DeleteUserCommand : IRequest<DeleteCommandResponse<UserDTO>>
{
    public Guid Id { get; set; }
}