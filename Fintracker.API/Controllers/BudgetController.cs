using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Features.Budget.Requests.Queries;
using Fintracker.Application.Models;
using Fintracker.Application.Responses.API_Responses;
using Fintracker.Application.Responses.Commands_Responses;
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
    
    [NonAction]
    private Guid GetCurrentUserId()
    {
        var uid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypeConstants.Uid)?.Value;
        if (Guid.TryParse(uid, out var currentUserId))
            return currentUserId;
        return Guid.Empty;
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

    [HttpGet("wallet/{walletId:guid}")]
    [ProducesResponseType(typeof(List<BudgetBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<BudgetBaseDTO>>> GetBudgetsByWalletId(Guid walletId, [FromQuery] BudgetQueryParams? query)
    {
        

        var simpleRequest = new GetBudgetsByWalletIdRequest()
        {
            WalletId = walletId,
            IsPublic = query?.IsPublic,
            UserId = GetCurrentUserId()
        };

        var sortedRequest = new GetBudgetsByWalletIdSortedRequest()
        {
            WalletId = walletId,
            Params = query!,
            UserId = GetCurrentUserId()
        };
        
        IReadOnlyList<BudgetBaseDTO> budgets;
        if (query is null)
            budgets = await _mediator.Send(simpleRequest);
        else
            budgets = await _mediator.Send(sortedRequest);

        return Ok(budgets);
    }

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(List<BudgetBaseDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<BudgetBaseDTO>>> GetBudgetsByUserId(Guid userId, [FromQuery] BudgetQueryParams? query)
    {

        var simpleRequest = new GetBudgetsByUserIdRequest()
        {
            UserId = userId,
            IsPublic = query?.IsPublic
        };

        var sortedRequest = new GetBudgetsByUserIdSortedRequest()
        {
            UserId = userId,
            Params = query!
        };
        
        IReadOnlyList<BudgetBaseDTO> budgets;
        if (query is null)
            budgets = await _mediator.Send(simpleRequest);
        else
            budgets = await _mediator.Send(sortedRequest);

        return Ok(budgets);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateCommandResponse<CreateBudgetDTO>),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCommandResponse<BudgetBaseDTO>>> Post([FromBody] CreateBudgetDTO budget)
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
            Id = id,
            UserId = GetCurrentUserId()
        });

        return Ok(response);
    }
}