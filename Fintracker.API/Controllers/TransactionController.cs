using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using Fintracker.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/transaction")]
[Authorize(Roles = "Admin,User")]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TransactionBaseDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionBaseDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetTransactionByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("category/{categoryId:guid}")]
    [ProducesResponseType(typeof(List<TransactionBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<TransactionBaseDTO>>> GetByCategoryId(Guid categoryId,
        [FromQuery] string sortBy,
        [FromQuery] bool isDescending)
    {
        var sortRequest = new GetTransactionsByCategoryIdSortedRequest()
        {
            CategoryId = categoryId,
            IsDescending = isDescending,
            SortBy = sortBy
        };

        var simpleRequest = new GetTransactionsByCategoryIdRequest()
        {
            CategoryId = categoryId
        };

        IReadOnlyList<TransactionBaseDTO> response;

        if (!string.IsNullOrEmpty(sortBy))
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(List<TransactionBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<TransactionBaseDTO>>> GetByUserId(Guid userId, [FromQuery] string sortBy,
        [FromQuery] bool isDescending)
    {
        var sortRequest = new GetTransactionsByUserIdSortedRequest()
        {
            UserId = userId,
            IsDescending = isDescending,
            SortBy = sortBy
        };

        var simpleRequest = new GetTransactionsByUserIdRequest()
        {
            UserId = userId
        };

        IReadOnlyList<TransactionBaseDTO> response;

        if (!string.IsNullOrEmpty(sortBy))
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }

    [HttpGet("wallet/{walletId:guid}")]
    [ProducesResponseType(typeof(List<TransactionBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<TransactionBaseDTO>>> GetByWalletId(Guid walletId, [FromQuery] string sortBy,
        [FromQuery] bool isDescending)
    {
        var sortRequest = new GetTransactionsByWalletIdSortedRequest()
        {
            WalletId = walletId,
            IsDescending = isDescending,
            SortBy = sortBy
        };

        var simpleRequest = new GetTransactionsByWalletIdRequest()
        {
            WalletId = walletId
        };

        IReadOnlyList<TransactionBaseDTO> response;

        if (!string.IsNullOrEmpty(sortBy))
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TransactionBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<TransactionBaseDTO>>> Get()
    {
        var response = await _mediator.Send(new GetTransactionsRequest());

        return Ok(response);
    }

    [HttpGet("{id:guid}/with-wallet")]
    [ProducesResponseType(typeof(TransactionWithWalletDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionWithWalletDTO>> GetWithWallet(Guid id)
    {
        var response = await _mediator.Send(new GetTransactionWithWalletByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}/with-user")]
    [ProducesResponseType(typeof(TransactionWithUserDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionWithUserDTO>> GetWithUser(Guid id)
    {
        var response = await _mediator.Send(new GetTransactionWithUserByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateCommandResponse<TransactionBaseDTO>),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateCommandResponse<TransactionBaseDTO>>> Post(
        [FromBody] CreateTransactionDTO transaction)
    {
        var response = await _mediator.Send(new CreateTransactionCommand()
        {
            Transaction = transaction
        });

        return Ok(response);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(UpdateCommandResponse<TransactionBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateCommandResponse<TransactionBaseDTO>>> Put(
        [FromBody] UpdateTransactionDTO transaction)
    {
        var response = await _mediator.Send(new UpdateTransactionCommand()
        {
            Transaction = transaction
        });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteCommandResponse<TransactionBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteCommandResponse<TransactionBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteTransactionCommand()
        {
            Id = id
        });

        return Ok(response);
    }
}