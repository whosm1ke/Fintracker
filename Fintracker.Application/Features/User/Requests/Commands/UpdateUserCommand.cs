using Fintracker.Application.DTO.User;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class UpdateUserCommand : IRequest<UpdateCommandResponse<UserBaseDTO>>
{
    public UpdateUserDTO User { get; set; }
}