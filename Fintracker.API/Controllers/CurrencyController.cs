using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Features.Currency.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/currency")]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CurrencyDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetCurrencyByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CurrencyDTO>>> Get([FromQuery] string sortBy, [FromQuery] bool isDescending)
    {
        var sortRequest = new GetCurrenciesSortedRequest()
        {
            IsDescending = isDescending,
            SortBy = sortBy
        };

        var simpleRequest = new GetCurrenciesRequest();

        IReadOnlyList<CurrencyDTO> response;

        if (!string.IsNullOrEmpty(sortBy))
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }
}