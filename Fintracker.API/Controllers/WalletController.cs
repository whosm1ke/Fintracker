using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using Fintracker.Application.Models;
using Fintracker.Application.Responses.API_Responses;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[Route("api/wallet")]
[Authorize(Roles = "Admin,User")]
public class WalletController : BaseController
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WalletBaseDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalletBaseDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetWalletByIdRequest
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("user/{ownerId:guid}")]
    [ProducesResponseType(typeof(List<WalletBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<WalletBaseDTO>>> Get(Guid ownerId,[FromQuery] QueryParams? query)
    {
        var sortRequest = new GetWalletsByUserIdSortedRequest
        {
            UserId = ownerId,
            Params = query!
        };

        var simpleRequest = new GetWalletsByOwnerIdRequest
        {
            OwnerId = ownerId
        };

        IReadOnlyList<WalletBaseDTO> response;

        if (query is not null)
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }
    

    [HttpPost]
    [ProducesResponseType(typeof(DeleteCommandResponse<WalletBaseDTO>),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCommandResponse<WalletBaseDTO>>> Post([FromBody] CreateWalletDTO wallet)
    {
        var response = await _mediator.Send(new CreateWalletCommand
        {
            Wallet = wallet
        });

        return Ok(response);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(UpdateCommandResponse<WalletBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateCommandResponse<WalletBaseDTO>>> Put([FromBody] UpdateWalletDTO wallet)
    {
        var response = await _mediator.Send(new UpdateWalletCommand
        {
            Wallet = wallet
        });

        return Ok(response);
    }

    [HttpPut("transfer")]
    [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<BaseCommandResponse>> Transfer([FromBody] TransferMoneyFromWalletToWalletCommand command)
    {
        var response = await _mediator.Send(command);

        return Ok(response);
    }
    

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteCommandResponse<WalletBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteCommandResponse<WalletBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteWalletCommand
        {
            Id = id,
            UserId = GetCurrentUserId()
        });

        return Ok(response);
    }
}

