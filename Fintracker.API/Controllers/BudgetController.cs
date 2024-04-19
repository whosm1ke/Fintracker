﻿using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Features.Budget.Requests.Queries;
using Fintracker.Application.Models;
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BudgetBaseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BudgetBaseDTO>> GetById(Guid id)
    {
        var budget = await _mediator.Send(new GetBudgetByIdRequest
        {
            Id = id
        });

        return Ok(budget);
    }

    [HttpGet("user/{id:guid}")]
    [ProducesResponseType(typeof(List<BudgetBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<BudgetBaseDTO>>> GetBudgetsByUserId(Guid id, [FromQuery] bool? isPublic)
    {
        IReadOnlyList<BudgetBaseDTO> budgets = await _mediator.Send(new GetBudgetsByUserIdRequest
        {
            UserId = id,
            IsPublic = isPublic
        });
        return Ok(budgets);
    }

    [HttpGet("wallet/{id:guid}")]
    [ProducesResponseType(typeof(List<BudgetBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<BudgetBaseDTO>>> GetBudgetsByWalletId(Guid id, [FromQuery] bool? isPublic)
    {
        IReadOnlyList<BudgetBaseDTO> budgets = await _mediator.Send(new GetBudgetsByWalletIdRequest
        {
            WalletId = id,
            IsPublic = isPublic
        });
        return Ok(budgets);
    }

    [HttpGet("user/{userId:guid}/sorted")]
    [ProducesResponseType(typeof(List<BudgetBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<BudgetBaseDTO>>> GetBudgetsByUserIdSorted(Guid userId,
        [FromQuery] BudgetQueryParams query)
    {
        IReadOnlyList<BudgetBaseDTO> budgets = await _mediator.Send(new GetBudgetsByUserIdSortedRequest
        {
            UserId = userId,
            Params = query,
            IsPublic = query.IsPublic
        });

        return Ok(budgets);
    }

    [HttpGet("wallet/{walletId:guid}/sorted")]
    [ProducesResponseType(typeof(List<BudgetBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<BudgetBaseDTO>>> GetBudgetsByWalletIdSorted(Guid walletId,
        [FromQuery] BudgetQueryParams query)
    {
        IReadOnlyList<BudgetBaseDTO> budgets = await _mediator.Send(new GetBudgetsByWalletIdSortedRequest
        {
            WalletId = walletId,
            Params = query,
            IsPublic = query.IsPublic
        });

        return Ok(budgets);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateCommandResponse<CreateBudgetDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCommandResponse<CreateBudgetDTO>>> Post([FromBody] CreateBudgetDTO budget)
    {
        var response = await _mediator.Send(new CreateBudgetCommand
        {
            Budget = budget
        });

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UpdateCommandResponse<BudgetBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateCommandResponse<BudgetBaseDTO>>> Put([FromBody] UpdateBudgetDTO budget)
    {
        var response = await _mediator.Send(new UpdateBudgetCommand
        {
            Budget = budget
        });

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteCommandResponse<BudgetBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteCommandResponse<BudgetBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteBudgetCommand
        {
            Id = id
        });

        return Ok(response);
    }
}