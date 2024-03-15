using Fintracker.Application.DTO.User;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Features.User.Requests.Queries;
using Fintracker.Application.Responses.API_Responses;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Roles = "Admin,User")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserBaseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserBaseDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetUserByIdRequest
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("accessed-wallets/{walletId:guid}")]
    [ProducesResponseType(typeof(List<UserBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<UserBaseDTO>>> GetWithAccessedWallets(Guid walletId)
    {
        var response = await _mediator.Send(new GetUsersAccessedToWalletRequest
        {
            WalletId = walletId
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}/with-budgets")]
    [ProducesResponseType(typeof(UserWithBudgetsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserWithBudgetsDTO>> GetWithBudgets(Guid id)
    {
        var response = await _mediator.Send(new GetUserWithBudgetsByIdRequest
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}/with-own-wallets")]
    [ProducesResponseType(typeof(UserWithOwnedWalletsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserWithOwnedWalletsDTO>> GetWithOwnWallets(Guid id)
    {
        var response = await _mediator.Send(new GetUserWithOwnedWalletsByIdRequest
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}/with-member-wallets")]
    [ProducesResponseType(typeof(UserWithMemberWalletsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserWithMemberWalletsDTO>> GetWithMemberWallets(Guid id)
    {
        var response = await _mediator.Send(new GetUserWithMemberWalletsByIdRequest
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteCommandResponse<UserBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteCommandResponse<UserBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteUserCommand
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UpdateCommandResponse<UserBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateCommandResponse<UserBaseDTO>>> Put([FromForm] UpdateUserDTO user,
        [FromServices] IWebHostEnvironment env)
    {
        if (user.Avatar != null)
        {
            var avatar = user.Avatar;
            var fileName = Path.GetFileName(avatar.FileName);
            var filePath = Path.Combine(env.WebRootPath,"images" ,fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await avatar.CopyToAsync(stream);
            }

            user.UserDetails.Avatar = filePath;
        }
        
        var response = await _mediator.Send(new UpdateUserCommand
        {
            User = user,
            WWWRoot = env.WebRootPath
        });

        return Ok(response);
    }
}