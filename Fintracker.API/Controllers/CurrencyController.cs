using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Features.Currency.Requests.Queries;
using Fintracker.Application.Models;
using Fintracker.Application.Responses.API_Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;


[Route("api/currency")]
[AllowAnonymous]
public class CurrencyController : BaseController
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CurrencyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CurrencyDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetCurrencyByIdRequest
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CurrencyDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CurrencyDTO>>> Get([FromQuery] QueryParams query)
    {
        var sortRequest = new GetCurrenciesSortedRequest
        {
            Params = query
        };

        var simpleRequest = new GetCurrenciesRequest();

        IReadOnlyList<CurrencyDTO> response;

        if (!string.IsNullOrEmpty(query.SortBy))
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }
}