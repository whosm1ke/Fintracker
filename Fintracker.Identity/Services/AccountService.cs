﻿using System.Reflection;
using Fintracker.Application;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Models.Identity;
using Fintracker.Domain.Entities;
using Fintracker.Domain.Enums;
using Fintracker.Identity.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Fintracker.Identity.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly AppSettings _appSettings;


    public AccountService(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager,
        IOptions<AppSettings> appSettings)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _appSettings = appSettings.Value;
    }

    public async Task<RegisterResponse> Register(RegisterRequest register)
    {
        var validator = new RegisterRequestValidator(_userManager);
        var validationResult = await validator.ValidateAsync(register);
        var response = new RegisterResponse();
        if (validationResult.IsValid)
        {
            var appUser = new User
            {
                UserName = register.UserName,
                Email = register.Email,
                UserDetails = new UserDetails
                {
                    Avatar = $"{_appSettings.BaseUrl}/api/user/avatar/logo.png",
                    Sex = "Other",
                    DateOfBirth = DateTime.Now,
                    Language = Language.English
                },
                CurrencyId = new Guid("c6746fe4-eb4c-1746-0c5e-88d8748deebc")
            };

            var createdUser = await _userManager.CreateAsync(appUser, register.Password);
            if (createdUser.Succeeded)
            {
                response.UserId = appUser.Id;
                response.Email = register.Email;

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    return response;
                }

                await _userManager.DeleteAsync(appUser);
                throw new RegisterAccountException(roleResult.Errors.Select(x => new ExceptionDetails
                {
                    ErrorMessage = x.Description,
                    PropertyName = null
                }).ToList());
            }

            throw new RegisterAccountException(createdUser.Errors.Select(x => new ExceptionDetails
            {
                ErrorMessage = x.Description,
                PropertyName = ExtractPropertyNameFromCode(x, typeof(RegisterRequest).GetProperties())
            }).ToList());
        }


        throw new BadRequestException(validationResult.Errors.Select(x => new ExceptionDetails
            { ErrorMessage = x.ErrorMessage, PropertyName = x.PropertyName }).ToList());
    }

    private string ExtractPropertyNameFromCode(IdentityError error, PropertyInfo[] props)
    {
        var prop = props.FirstOrDefault(p => error.Code.Contains(p.Name));
        if (prop != null)
            return prop.Name;
        return null;
    }

    public async Task<LoginResponse> Login(LoginRequest login)
    {
        var validator = new LoginRequestValidator(_userManager);
        var validationResult = await validator.ValidateAsync(login);
        var response = new LoginResponse();
        if (validationResult.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
                throw new LoginException(validationResult.Errors
                    .Select(x => new ExceptionDetails
                    {
                        PropertyName = x.PropertyName,
                        ErrorMessage = x.ErrorMessage
                    }).ToList());

            var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!checkPasswordResult.Succeeded)
                throw new LoginException(new ExceptionDetails
                {
                    ErrorMessage = "Invalid credentials",
                    PropertyName = nameof(User)
                });

            response.UserId = user.Id;
            response.Token = await _tokenService.CreateToken(user);
            response.UserEmail = user.Email!;
            await _signInManager.SignInAsync(user, true);
            return response;
        }

        throw new BadRequestException(validationResult.Errors.Select(x => new ExceptionDetails
            { ErrorMessage = x.ErrorMessage, PropertyName = x.PropertyName }).ToList());
    }

    public async Task<string> UpdateUserUsername(string newUsername, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = "Can not find user with id " + userId,
                PropertyName = nameof(userId)
            }, nameof(User));
        var changeResult = await _userManager.SetUserNameAsync(user, newUsername);

        if (!changeResult.Succeeded)
            throw new RegisterAccountException(changeResult.Errors.Select(x => new ExceptionDetails
            {
                ErrorMessage = x.Description,
                PropertyName = "userName"
            }).ToList());

        return newUsername;
    }

    public async Task<bool> ResetPassword(ResetPasswordModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId.ToString());

        if (user is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"User with id was not founded [{model.UserId}]",
                PropertyName = nameof(model.UserId)
            }, nameof(User));


        var restPasswordResult = await _userManager.ResetPasswordAsync(user!, model.Token, model.Password);
        
        if(!restPasswordResult.Succeeded)
            throw new BadRequestException(restPasswordResult.Errors.Select(x => new ExceptionDetails
            {
                ErrorMessage = x.Description,
                PropertyName = "password"
            }).ToList());

        return restPasswordResult.Succeeded;
    }

    public async Task<bool> ResetEmail(ResetEmailModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId.ToString());

        if (user is null)
            throw new NotFoundException(new ExceptionDetails
            {
                ErrorMessage = $"User with id was not founded [{model.UserId}]",
                PropertyName = nameof(model.UserId)
            }, nameof(User));

        var resetEmailResult = await _userManager.ChangeEmailAsync(user!, model.NewEmail, model.Token);
        
        if(!resetEmailResult.Succeeded)
            throw new BadRequestException(resetEmailResult.Errors.Select(x => new ExceptionDetails
            {
                ErrorMessage = x.Description,
                PropertyName = "email"
            }).ToList());

        return resetEmailResult.Succeeded;
    }

    public async Task<string> GenerateResetPasswordToken(User user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = System.Net.WebUtility.UrlEncode(token);
        return encodedToken;
    }

    public async Task<string> GenerateResetEmailToken(User user, string newEmail)
    {
        var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        var encodedToken = System.Net.WebUtility.UrlEncode(token);
        return encodedToken;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
}