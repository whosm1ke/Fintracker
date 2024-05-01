using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.DTO.Monobank;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Monobank.Requests.Commands;
using Fintracker.Application.Features.Monobank.Requests.Queries;
using Fintracker.Application.Models.Monobank;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Fintracker.API.Controllers;


[Route("api/bank")]
[Authorize(Roles = "Admin, User")]
public class BankController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _cache;

    public BankController(IMediator mediator, IMemoryCache cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpGet("monobank")]
    public async Task<ActionResult<MonobankUserInfoDTO>> GetMonobankUserInfo([FromHeader] string xToken)
    {
        string email = HttpContext.User.FindFirst(x => x.Type == ClaimTypeConstants.Email)?.Value!;
        var response = await _mediator.Send(new GetMonobankUserInfoRequest
        {
            Token = xToken,
            Email = email
        });


        return Ok(response);
    }

    [HttpPost("monobank/add-initial-transactions")]
    public async Task<ActionResult<CreateCommandResponse<WalletBaseDTO>>> AddMonobankTransactions(
        [FromBody] MonobankConfiguration monoCfg)
    {
        string id = HttpContext.User.FindFirst(x => x.Type == ClaimTypeConstants.Uid)?.Value!;
        string email = HttpContext.User.FindFirst(x => x.Type == ClaimTypeConstants.Email)?.Value!;
        Guid userId = Guid.Parse(id);

        var transactions = await _mediator.Send(new GetMonobankUserTransactionsRequest
        {
            Configuration = monoCfg,
            Email = email
        });

        var addTransactionsResponse = await _mediator.Send(new AddInitialTransactionToNewBankWalletCommand
        {
            Payload = new()
            {
                OwnerId = userId,
                Transactions = transactions.ToList(),
                Email = email,
                AccountId = monoCfg.AccountId
            }
        });

        return Ok(addTransactionsResponse);
    }

    [HttpPost("monobank/add-new-transactions")]
    public async Task<IActionResult> AddNewMonoTransactions([FromBody] string accountId)
    {
        string id = HttpContext.User.FindFirst(x => x.Type == ClaimTypeConstants.Uid)?.Value!;
        string email = HttpContext.User.FindFirst(x => x.Type == ClaimTypeConstants.Email)?.Value!;
        Guid userId = Guid.Parse(id);

        var transactions = await _mediator.Send(new GetMonobankUserTransactionsRequest
        {
            Configuration = new MonobankConfiguration
            {
                AccountId = accountId,
                From = _cache.Get<long>("mono_from_value")
            },
            Email = email
        });

        var addTransactionsResponse = await _mediator.Send(new AddNewTransactionsToBankingWalletCommand
        {
            Payload = new()
            {
                OwnerId = userId,
                Transactions = transactions.ToList(),
                Email = email,
                AccountId = accountId
            }
        });

        return Ok(addTransactionsResponse);
    }
}