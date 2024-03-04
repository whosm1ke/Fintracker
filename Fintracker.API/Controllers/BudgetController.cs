using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.Features.Budget.Requests.Queries;
using Fintracker.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetBaseDTO>> Get(Guid id)
    {
        var budget = await _mediator.Send(new GetBudgetByIdRequest()
        {
            Id = id
        });

        return Ok(budget);
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
}