using System.Security.Claims;
using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.DTO.Invite;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Models.Identity;
using Fintracker.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMediator _mediator;

    public AccountController(IAccountService accountService, IMediator mediator)
    {
        _accountService = accountService;
        _mediator = mediator;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),
        StatusCodes.Status400BadRequest | StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest register)
    {
        var response = await _accountService.Register(register);

        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest login)
    {
        var response = await _accountService.Login(login);

        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Logout()
    {
        await _accountService.Logout();

        return Ok();
    }

    [HttpPost("invite")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> InviteUser([FromBody] InviteDTO invite)
    {
        var inviteCommand = new InviteUserCommand()
        {
            UserEmail = invite.Email,
            WalletId = invite.WalletId,
            WhoInvited = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            UrlCallback = invite.UrlCallback
        };
        await _mediator.Send(inviteCommand);

        return Ok();
    }

    [HttpPost("invite/accept")]
    [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseCommandResponse>> AddUserToWallet([FromQuery] Guid walletId,
        [FromQuery] string token)
    {
        var response = await _mediator.Send(new AddUserToWalletCommand()
        {
            WalletId = walletId,
            Token = token
        });


        return Ok(response);
    }


    [HttpPost("reset-pass-request")]
    public async Task<IActionResult> ResetPasswordRequest([FromBody] string urlCallback)
    {
        await _mediator.Send(new SentResetPasswordCommand()
        {
            Email = HttpContext.User.FindFirst(ClaimTypeConstants.Email)?.Value ?? "no",
            UrlCallback = urlCallback
        });

        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordModel model)
    {
        var res = await _accountService.ResetPassword(model);
        if (!res)
            return BadRequest("Something gone wrong. Check your email or password again");
        return Ok();
    }
}