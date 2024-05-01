using System.Security.Claims;
using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.DTO.Invite;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Models.Identity;
using Fintracker.Application.Responses.API_Responses;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[Route("api/account")]
public class AccountController : BaseController
{
    private readonly IAccountService _accountService;
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;

    public AccountController(IAccountService accountService, IMediator mediator, IWebHostEnvironment environment)
    {
        _accountService = accountService;
        _mediator = mediator;
        _environment = environment;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse),
        StatusCodes.Status400BadRequest | StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest register)
    {
        var response = await _accountService.Register(register);

        await _mediator.Send(new PopulateUserWithCategoriesCommand()
        {
            UserId = response.UserId,
            PathToFile = Path.Combine(_environment.WebRootPath, "data", "categories.json")
        });

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
        var inviteCommand = new InviteUserCommand
        {
            UserEmail = invite.Email,
            WalletId = invite.WalletId,
            WhoInvited = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            UrlCallback = invite.UrlCallback,
            PathToCategories = Path.Combine(_environment.WebRootPath, "data", "categories.json")
        };
        await _mediator.Send(inviteCommand);

        return Ok();
    }

    [HttpPost("invite/accept")]
    [ProducesResponseType(typeof(BaseCommandResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseCommandResponse>> AddUserToWallet([FromBody] AcceptInviteDto accept)
    {
        var response = await _mediator.Send(new AddUserToWalletCommand
        {
            WalletId = accept.WalletId,
            UserId = accept.UserId,
            PathToCategories = Path.Combine(_environment.WebRootPath, "data", "categories.json")
        });
        return Ok(response);
    }


    [HttpGet("reset-password")]
    public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetRequestBase reset)
    {
        await _mediator.Send(new SentResetPasswordCommand
        {
            Email = HttpContext.User.FindFirst(ClaimTypeConstants.Email)?.Value ?? "no",
            UrlCallback = reset.UrlCallback
        });

        return Ok();
    }

    [HttpGet("reset-email")]
    public async Task<IActionResult> ResetEmailRequest([FromBody] ResetEmailRequest reset)
    {
        await _mediator.Send(new SentResetEmailCommand
        {
            Email = HttpContext.User.FindFirst(ClaimTypeConstants.Email)?.Value ?? "no",
            UrlCallback = reset.UrlCallback,
            NewEmail = reset.NewEmail
        });

        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        var res = await _accountService.ResetPassword(model);
        if (!res)
            return BadRequest("Something gone wrong. Check your email or password again");
        return Ok();
    }

    [HttpPost("reset-email")]
    public async Task<IActionResult> ResetEmail([FromBody] ResetEmailModel model)
    {
        var res = await _accountService.ResetEmail(model);
        if (!res)
            return BadRequest("Something gone wrong. Check your email");
        return Ok();
    }
}