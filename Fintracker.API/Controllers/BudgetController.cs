using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Features.Budget.Requests.Queries;
using Fintracker.Application.Responses.API_Responses;
using Fintracker.Application.Responses.Commands_Responses;
using Fintracker.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/budget")]
[Authorize(Roles = "Admin,User")]
public class BudgetController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}/with-wallet")]
    [ProducesResponseType(typeof(BudgetWithWalletDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BudgetWithWalletDTO>> GetBudgetWithWallet(Guid id)
    {
        var response = await _mediator.Send(new GetBudgetWithWalletByIdRequest
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}/with-user")]
    [ProducesResponseType(typeof(BudgetWithUserDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BudgetWithUserDTO>> GetBudgetWithUser(Guid id)
    {
        var response = await _mediator.Send(new GetBudgetWithUserByIdRequest
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BudgetBaseDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BudgetBaseDTO>> GetById(Guid id)
    {
        var budget = await _mediator.Send(new GetBudgetByIdRequest
        {
            Id = id
        });

        return Ok(budget);
    }

    [HttpGet("{id:guid}/list")]
    [ProducesResponseType(typeof(List<BudgetBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<BudgetBaseDTO>>> GetBudgetsById(Guid id, [FromQuery] bool? isPublic,
        [FromQuery] string type = "wallet")
    {
        if (string.IsNullOrEmpty(type) ||
            (type.ToLower() != $"{nameof(Wallet).ToLower()}" && type.ToLower() != $"{nameof(User).ToLower()}"))
        {
            return BadRequest(
                $"Invalid 'type' parameter. Use '{nameof(Wallet).ToLower()}' or '{nameof(User).ToLower()}'");
        }

        IReadOnlyList<BudgetBaseDTO> budgets;

        if (type == nameof(User).ToLower())
            budgets = await _mediator.Send(new GetBudgetsByUserIdRequest
            {
                UserId = id,
                IsPublic = isPublic.HasValue && isPublic.Value
            });
        else
            budgets = await _mediator.Send(new GetBudgetsByWalletIdRequest
            {
                WalletId = id,
                IsPublic = isPublic.HasValue && isPublic.Value
            });

        return Ok(budgets);
    }

    [HttpGet("sorted")]
    [ProducesResponseType(typeof(List<BudgetBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<BudgetBaseDTO>>> GetBudgetsByIdSorted([FromQuery] Guid id,
        [FromQuery] bool? isPublic,
        [FromQuery] string? sortBy,
        [FromQuery] bool? isDescending,
        [FromQuery] string type = "wallet")
    {
        if (string.IsNullOrEmpty(type) ||
            (type.ToLower() != $"{nameof(Wallet).ToLower()}" && type.ToLower() != $"{nameof(User).ToLower()}"))
        {
            return BadRequest(
                $"Invalid 'type' parameter. Use '{nameof(Wallet).ToLower()}' or '{nameof(User).ToLower()}'");
        }
        
        IReadOnlyList<BudgetBaseDTO> budgets;

        if (type == nameof(User).ToLower())
            budgets = await _mediator.Send(new GetBudgetsByUserIdSortedRequest
            {
                UserId = id,
                IsDescending = isDescending.HasValue && isDescending.Value,
                SortBy = sortBy!,
                IsPublic = isPublic.HasValue && isPublic.Value
            });
        else
            budgets = await _mediator.Send(new GetBudgetsByWalletIdSortedRequest
            {
                WalletId = id,
                IsDescending = isDescending.HasValue && isDescending.Value,
                SortBy = sortBy!,
                IsPublic = isPublic.HasValue && isPublic.Value
            });

        return Ok(budgets);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateCommandResponse<CreateBudgetDTO>),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCommandResponse<CreateBudgetDTO>>> Post([FromBody] CreateBudgetDTO budget)
    {
        
        var response = await _mediator.Send(new CreateBudgetCommand
        {
            Budget = budget
        });

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UpdateCommandResponse<BudgetBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateCommandResponse<BudgetBaseDTO>>> Put([FromBody] UpdateBudgetDTO budget)
    {
        var response = await _mediator.Send(new UpdateBudgetCommand
        {
            Budget = budget
        });

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteCommandResponse<BudgetBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteCommandResponse<BudgetBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteBudgetCommand
        {
            Id = id
        });

        return Ok(response);
    }
}