using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using Fintracker.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/wallet")]
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WalletBaseDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetWalletByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("owner/{ownerId:guid}")]
    public async Task<ActionResult<List<WalletBaseDTO>>> Get(Guid ownerId, [FromQuery] string sortBy,
        [FromQuery] bool isDescnending)
    {
        var sortRequest = new GetWalletsByOwnerIdSortedRequest()
        {
            OwnerId = ownerId,
            IsDescending = isDescnending,
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
    public async Task<ActionResult<WalletWithBudgetsDTO>> GetWithBudget(Guid id)
    {
        var response = await _mediator.Send(new GetWalletWithBudgetsByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }
    
    [HttpGet("{id:guid}/with-members")]
    public async Task<ActionResult<WalletWithMembersDTO>> GetWithMembers(Guid id)
    {
        var response = await _mediator.Send(new GetWalletWithMembersByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }
    
    [HttpGet("{id:guid}/with-transactions")]
    public async Task<ActionResult<WalletWithTransactionsDTO>> GetWithTransactions(Guid id)
    {
        var response = await _mediator.Send(new GetWalletWithTransactionsByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CreateCommandResponse<WalletBaseDTO>>> Post([FromBody] CreateWalletDTO wallet)
    {
        var response = await _mediator.Send(new CreateWalletCommand()
        {
            Wallet = wallet
        });

        return Ok(response);
    }
    
    [HttpPut]
    public async Task<ActionResult<UpdateCommandResponse<WalletBaseDTO>>> Put([FromBody] UpdateWalletDTO wallet)
    {
        var response = await _mediator.Send(new UpdateWalletCommand()
        {
            Wallet = wallet
        });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<DeleteCommandResponse<WalletBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteWalletCommand()
        {
            Id = id
        });

        return Ok(response);
    }
}