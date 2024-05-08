using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Requests.Commands;

public class UpdateUserUsernameCommand : IRequest<BaseCommandResponse>, INotGetRequest
{
    public string NewUsername { get; set; }

    public Guid UserId { get; set; }
}