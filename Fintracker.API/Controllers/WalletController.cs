using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using Fintracker.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/wallet")]
[Authorize(Roles = "Admin,User")]
public class WalletController : ControllerBase
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
        var response = await _mediator.Send(new GetWalletByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("owner/{ownerId:guid}")]
    [ProducesResponseType(typeof(List<WalletBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<WalletBaseDTO>>> Get(Guid ownerId, [FromQuery] string sortBy,
        [FromQuery] bool isDescending)
    {
        var sortRequest = new GetWalletsByOwnerIdSortedRequest()
        {
            OwnerId = ownerId,
            IsDescending = isDescending,
            SortBy = sortBy
        };

        var simpleRequest = new GetWalletsByOwnerIdRequest()
        {
            OwnerId = ownerId
        };

        IReadOnlyList<WalletBaseDTO> response;

        if (!string.IsNullOrEmpty(sortBy))
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }

    [HttpGet("{id:guid}/with-budget")]
    [ProducesResponseType(typeof(WalletWithBudgetsDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalletWithBudgetsDTO>> GetWithBudget(Guid id)
    {
        var response = await _mediator.Send(new GetWalletWithBudgetsByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }
    
    [HttpGet("{id:guid}/with-members")]
    [ProducesResponseType(typeof(WalletWithMembersDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalletWithMembersDTO>> GetWithMembers(Guid id)
    {
        var response = await _mediator.Send(new GetWalletWithMembersByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }
    
    [HttpGet("{id:guid}/with-transactions")]
    [ProducesResponseType(typeof(WalletWithTransactionsDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalletWithTransactionsDTO>> GetWithTransactions(Guid id)
    {
        var response = await _mediator.Send(new GetWalletWithTransactionsByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DeleteCommandResponse<WalletBaseDTO>),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCommandResponse<WalletBaseDTO>>> Post([FromBody] CreateWalletDTO wallet)
    {
        var response = await _mediator.Send(new CreateWalletCommand()
        {
            Wallet = wallet
        });

        return Ok(response);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(DeleteCommandResponse<WalletBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateCommandResponse<WalletBaseDTO>>> Put([FromBody] UpdateWalletDTO wallet)
    {
        var response = await _mediator.Send(new UpdateWalletCommand()
        {
            Wallet = wallet
        });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteCommandResponse<WalletBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteCommandResponse<WalletBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteWalletCommand()
        {
            Id = id
        });

        return Ok(response);
    }
}

