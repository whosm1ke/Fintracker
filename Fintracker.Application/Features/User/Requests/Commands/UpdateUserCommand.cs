using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class UpdateUserCommand : IRequest<UpdateCommandResponse<UserBaseDTO>>, INotGetRequest
{
    public UpdateUserDTO User { get; set; } = default!;

    public string WWWRoot { get; set; } = default!;

}