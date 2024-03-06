using Fintracker.Application.DTO.User;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Features.User.Requests.Queries;
using Fintracker.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserBaseDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetUserByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }
    
    [HttpGet("accessed-wallets/{walletId:guid}")]
    public async Task<ActionResult<List<UserBaseDTO>>> GetWithAccessedWallets(Guid walletId)
    {
        var response = await _mediator.Send(new GetUsersAccessedToWalletRequest()
        {
            WalletId = walletId
        });

        return Ok(response);
    }
    
    [HttpGet("{id:guid}/with-budgets")]
    public async Task<ActionResult<UserWithBudgetsDTO>> GetWithBudgets(Guid id)
    {
        var response = await _mediator.Send(new GetUserWithBudgetsByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }
    
    [HttpGet("{id:guid}/with-own-wallets")]
    public async Task<ActionResult<UserWithOwnedWalletsDTO>> GetWithOwnWallets(Guid id)
    {
        var response = await _mediator.Send(new GetUserWithOwnedWalletsByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }
    
    [HttpGet("{id:guid}/with-member-wallets")]
    public async Task<ActionResult<UserWithMemberWalletsDTO>> GetWithMemberWallets(Guid id)
    {
        var response = await _mediator.Send(new GetUserWithMemberWalletsByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<DeleteCommandResponse<UserBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteUserCommand()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateCommandResponse<UserBaseDTO>>> Put([FromBody] UpdateUserDTO user)
    {
        var response = await _mediator.Send(new UpdateUserCommand()
        {
            User = user
        });

        return Ok(response);
    }
}