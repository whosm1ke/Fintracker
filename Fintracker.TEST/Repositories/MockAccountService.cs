using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Models.Identity;
using Moq;

namespace Fintracker.TEST.Repositories;

public class MockAccountService
{
    public static Mock<IAccountService> GetAccountService()
    {
        var mock = new Mock<IAccountService>();

        mock.Setup(x => x.Register(It.IsAny<RegisterRequest>()))
            .Returns((RegisterRequest rq) =>
                Task.FromResult(
                    new RegisterResponse()
                    {
                        UserId = new Guid("A6F29D61-2014-43D1-9EAE-0042781DD703")
                    }
                ));

        mock.Setup(x => x.Login(It.IsAny<LoginRequest>()))
            .Returns((LoginRequest lq) => Task.FromResult(new LoginResponse()
            {
                UserId = new Guid("A6F29D61-2014-43D1-9EAE-0042781DD703"),
                UserEmail = "test@email.com",
                Token = "Some token here as well"
            }));

        return mock;
    }
}