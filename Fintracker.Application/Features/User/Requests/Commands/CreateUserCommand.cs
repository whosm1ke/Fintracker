using Fintracker.Application.DTO.User;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class CreateUserCommand : IRequest<CreateCommandResponse<UserDTO>>
{
    public CreateUserDTO User { get; set; }
}