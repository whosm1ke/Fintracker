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
    [ProducesResponseType(typeof(RegisterResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status400BadRequest | StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest register)
    {
        var response = await _accountService.Register(register);

        return Ok(response);
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest login)
    {
        var response = await _accountService.Login(login);

        return Ok(response);
    }
    
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(void),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Logout()
    {
        await _accountService.Logout();

        return Ok();
    }
    
    [HttpPost("invite")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> InviteUser([FromBody] InviteDTO invite)
    {
        await _mediator.Send(new InviteUserCommand()
        {
            UserEmail = invite.Email,
            WalletId = invite.WalletId,
        });

        return Ok();
    }
    [HttpPost("invite/accept")]
    [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseCommandResponse>> AddUserToWallet([FromQuery] Guid walletId, [FromQuery]string token)
    {
        var response = await _mediator.Send(new AddUserToWalletCommand()
        {
            WalletId = walletId,
            Token = token
        });

        return Ok(response);
    }
}