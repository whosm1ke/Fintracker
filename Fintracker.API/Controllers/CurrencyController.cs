using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Features.Currency.Requests.Queries;
using Fintracker.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/currency")]
[AllowAnonymous]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CurrencyDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CurrencyDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetCurrencyByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CurrencyDTO>),StatusCodes.Status200OK)]
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