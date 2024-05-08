using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;

namespace Fintracker.Application.Features.User.Handlers.Commands;

public class UpdateUserUsernameCommandHandler : IRequestHandler<UpdateUserUsernameCommand, BaseCommandResponse>
{
    private readonly IAccountService _accountService;

    public UpdateUserUsernameCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    public async Task<BaseCommandResponse> Handle(UpdateUserUsernameCommand request, CancellationToken cancellationToken)
    {
        return new()
        {
            Message = await _accountService.UpdateUserUsername(request.NewUsername, request.UserId),
            Id = request.UserId,
            Success = true
        };
    }
}