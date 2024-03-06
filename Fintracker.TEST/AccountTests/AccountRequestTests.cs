using Fintracker.Application.DTO.Account;
using Fintracker.TEST.Repositories;
using FluentAssertions;

namespace Fintracker.TEST.AccountTests;

public class AccountRequestTests
{
    [Fact]
    public async Task Register_Test()
    {
        var mockAccount = MockAccountService.GetAccountService().Object;
        var registerRequest = new RegisterRequest()
        {
            UserName = "username",
            Email = "test@mail.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };
        var expectedUserId = new Guid("A6F29D61-2014-43D1-9EAE-0042781DD703");
        var regResponse = await mockAccount.Register(registerRequest);

        regResponse.UserId.Should().Be(expectedUserId);

    }
    
    [Fact]
    public async Task Login_Test()
    {
        var mockAccount = MockAccountService.GetAccountService().Object;
        var loginRequest = new LoginRequest()
        {
            Email = "test@mail.com",
            Password = "Password123!",
        };
        var expectedLoginResponse = new LoginResponse()
        {
            UserId = new Guid("A6F29D61-2014-43D1-9EAE-0042781DD703"),
            UserEmail = "test@email.com",
            Token = "Some token here as well"
        };
        var actualLoginResponse = await mockAccount.Login(loginRequest);

        actualLoginResponse.Should().Be(actualLoginResponse);

    }
}