using Fintracker.Application.DTO.User;
using Fintracker.Application.Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class UpdateUserCommand : IRequest<UpdateCommandResponse<UserDTO>>
{
    public UpdateUserDTO User { get; set; }
}