using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Models.Identity;
using Fintracker.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[Route("api/account")]
[ApiController]

public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
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
}