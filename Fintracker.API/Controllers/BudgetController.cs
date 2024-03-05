using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Features.Budget.Requests.Queries;
using Fintracker.Application.Responses;
using Fintracker.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/budget")]
public class BudgetController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}/with-wallet")]
    public async Task<ActionResult<BudgetWithWalletDTO>> GetBudgetWithWallet(Guid id)
    {
        var response = await _mediator.Send(new GetBudgetWithWalletByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}/with-user")]
    public async Task<ActionResult<BudgetWithUserDTO>> GetBudgetWithUser(Guid id)
    {
        var response = await _mediator.Send(new GetBudgetWithUserByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BudgetBaseDTO>> GetById(Guid id)
    {
        var budget = await _mediator.Send(new GetBudgetByIdRequest()
        {
            Id = id
        });

        return Ok(budget);
    }

    [HttpGet("{id:guid}/list")]
    public async Task<ActionResult<List<BudgetBaseDTO>>> GetBudgetsById(Guid id,
        [FromQuery] string type)
    {
        if (string.IsNullOrEmpty(type) ||
            (type.ToLower() != $"{nameof(Wallet).ToLower()}" && type.ToLower() != $"{nameof(User).ToLower()}"))
        {
            return BadRequest(
                $"Invalid 'type' parameter. Use '{nameof(Wallet).ToLower()}' or '{nameof(User).ToLower()}'");
        }

        IReadOnlyList<BudgetBaseDTO> budgets;

        if (type == nameof(User).ToLower())
            budgets = await _mediator.Send(new GetBudgetsByUserIdRequest()
            {
                UserId = id
            });
        else
            budgets = await _mediator.Send(new GetBudgetsByWalletIdRequest()
            {
                WalletId = id
            });

        return Ok(budgets);
    }

    [HttpGet("sorted")]
    public async Task<ActionResult<List<BudgetBaseDTO>>> GetBudgetsByIdSorted([FromQuery] Guid id,
        [FromQuery] string type,
        [FromQuery] string sortBy,
        [FromQuery] bool isDescending)
    {
        if (string.IsNullOrEmpty(type) ||
            (type.ToLower() != $"{nameof(Wallet).ToLower()}" && type.ToLower() != $"{nameof(User).ToLower()}"))
        {
            return BadRequest(
                $"Invalid 'type' parameter. Use '{nameof(Wallet).ToLower()}' or '{nameof(User).ToLower()}'");
        }
        
        IReadOnlyList<BudgetBaseDTO> budgets;

        if (type == nameof(User).ToLower())
            budgets = await _mediator.Send(new GetBudgetsByUserIdSortedRequest()
            {
                UserId = id,
                IsDescending = isDescending,
                SortBy = sortBy
            });
        else
            budgets = await _mediator.Send(new GetBudgetsByWalletIdSortedRequest()
            {
                WalletId = id,
                IsDescending = isDescending,
                SortBy = sortBy
            });

        return Ok(budgets);
    }

    [HttpPost]
    public async Task<ActionResult<CreateCommandResponse<CreateBudgetDTO>>> Post([FromBody] CreateBudgetDTO budget)
    {
        var response = await _mediator.Send(new CreateBudgetCommand()
        {
            Budget = budget
        });

        return response;
    }

    [HttpPut]
    public async Task<ActionResult<UpdateCommandResponse<BudgetBaseDTO>>> Put([FromBody] UpdateBudgetDTO budget)
    {
        var response = await _mediator.Send(new UpdateBudgetCommand()
        {
            Budget = budget
        });

        return response;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteCommandResponse<BudgetBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteBudgetCommand()
        {
            Id = id
        });

        return response;
    }
}