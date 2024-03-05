using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.DTO.Account;
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
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest register)
    {
        var response = await _accountService.Register(register);

        return Ok(response);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest login)
    {
        var response = await _accountService.Login(login);

        return Ok(response);
    }
    
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await _accountService.Logout();

        return Ok();
    }
}