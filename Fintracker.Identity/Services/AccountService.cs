using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Models.Identity;
using Fintracker.Domain.Entities;
using Fintracker.Identity.Validators;
using Microsoft.AspNetCore.Identity;

namespace Fintracker.Identity.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public AccountService(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    public async Task<RegisterResponse> Register(RegisterRequest register)
    {
        var validator = new RegisterRequestValidator(_userManager);
        var validationresult = await validator.ValidateAsync(register);
        var response = new RegisterResponse();
        if (validationresult.IsValid)
        {
            var appUser = new User()
            {
                UserName = register.UserName,
                Email = register.Email
            };

            var createdUser = await _userManager.CreateAsync(appUser, register.Password);
            if (createdUser.Succeeded)
            {
                response.UserId = appUser.Id;

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    return response;
                }

                await _userManager.DeleteAsync(appUser);
                throw new RegisterAccountException(roleResult.Errors.Select(x => x.Description).ToList());
            }

            throw new RegisterAccountException(createdUser.Errors.Select(x => x.Description).ToList());
        }

        throw new RegisterAccountException(validationresult.Errors
            .Select(x => x.ErrorMessage).ToList());
    }

    public async Task<LoginResponse> Login(LoginRequest login)
    {
        var validator = new LoginRequestValidator(_userManager);
        var validationresult = await validator.ValidateAsync(login);
        var response = new LoginResponse();
        if (validationresult.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
                throw new LoginException(validationresult.Errors
                    .Select(x => x.ErrorMessage).ToList());

            var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!checkPasswordResult.Succeeded) throw new LoginException("Invalid credentials");
            
            response.UserId = user.Id;
            response.Token = await _tokenService.CreateToken(user);
            response.UserEmail = user.Email!;
            await _signInManager.SignInAsync(user, true);
            return response;
        }

        throw new LoginException(validationresult.Errors
            .Select(x => x.ErrorMessage).ToList());
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
}