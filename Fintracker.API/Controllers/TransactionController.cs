using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using Fintracker.Application.Models;
using Fintracker.Application.Responses.API_Responses;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;


[Route("api/transaction")]
[Authorize(Roles = "Admin,User")]
public class TransactionController : BaseController
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TransactionBaseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionBaseDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetTransactionByIdRequest
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("category/{categoryId:guid}/user/{userId:guid?}")]
    [ProducesResponseType(typeof(List<TransactionBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<TransactionBaseDTO>>> GetByCategoryId(Guid categoryId, Guid? userId,
        [FromQuery] TransactionQueryParams? query)
    {
        var sortRequest = new GetTransactionsByCategoryIdSortedRequest
        {
            CategoryId = categoryId,
            Params = query!,
            UserId = userId ?? GetCurrentUserId()
        };

        var simpleRequest = new GetTransactionsByCategoryIdRequest
        {
            CategoryId = categoryId,
            UserId = userId ?? GetCurrentUserId()
        };

        IReadOnlyList<TransactionBaseDTO> response;

        if (query is not null)
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(List<TransactionBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<TransactionBaseDTO>>> GetByUserId(Guid userId, [FromQuery] TransactionQueryParams? query)
    {
        var sortRequest = new GetTransactionsByUserIdSortedRequest
        {
            UserId = userId,
            Params = query!
        };

        var simpleRequest = new GetTransactionsByUserIdRequest
        {
            UserId = userId
        };

        IReadOnlyList<TransactionBaseDTO> response;

        if (query is not null)
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }

    [HttpGet("wallet/{walletId:guid}")]
    [ProducesResponseType(typeof(List<TransactionBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<TransactionBaseDTO>>> GetByWalletId(Guid walletId,
        [FromQuery] TransactionQueryParams? query)
    {
        var sortRequest = new GetTransactionsByWalletIdSortedRequest
        {
            WalletId = walletId,
            Params = query!
        };

        var simpleRequest = new GetTransactionsByWalletIdRequest
        {
            WalletId = walletId
        };

        IReadOnlyList<TransactionBaseDTO> response;
       
        if (query is not null)
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }

    [HttpGet("user/{userId:guid}/all")]
    [ProducesResponseType(typeof(List<TransactionBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<TransactionBaseDTO>>> GetAllForUser(Guid userId)
    {
        var response = await _mediator.Send(new GetTransactionsRequest
        {
            UserId = userId
        });

        return Ok(response);
    }


    [HttpPost]
    [ProducesResponseType(typeof(CreateCommandResponse<TransactionBaseDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCommandResponse<TransactionBaseDTO>>> Post(
        [FromBody] CreateTransactionDTO transaction)
    {
        var response = await _mediator.Send(new CreateTransactionCommand
        {
            Transaction = transaction
        });

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UpdateCommandResponse<TransactionBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateCommandResponse<TransactionBaseDTO>>> Put(
        [FromBody] UpdateTransactionDTO transaction)
    {
        var response = await _mediator.Send(new UpdateTransactionCommand
        {
            Transaction = transaction
        });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteCommandResponse<TransactionBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteCommandResponse<TransactionBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteTransactionCommand
        {
            Id = id
        });

        return Ok(response);
    }
}