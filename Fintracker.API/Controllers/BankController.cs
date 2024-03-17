using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.Features.Monobank.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/bank")]
[Authorize(Roles = "Admin, User")]
public class BankController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("monobank")]
    public async Task<ActionResult<MonobankUserInfoDTO>> GetMonobankUserInfo([FromHeader] string xToken)
    {
        var response = await _mediator.Send(new GetMonobankUserInfoRequest()
        {
            Token = xToken
        });

        return Ok(response);
    }
    
    [HttpGet("monobank/transactions")]
    public async Task<ActionResult<IReadOnlyList<MonoTransactionDTO>>> GetMonobankTransactions([FromHeader] string xToken,
        [FromQuery] string accountId, [FromQuery] long from, [FromQuery] long? to)
    {
        var response = await _mediator.Send(new GetMonobankUserTransactionsRequest()
        {
            Token = xToken,
            AccountId = accountId,
            From = from,
            To = to ?? (long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds
        });

        return Ok(response);
    }
}