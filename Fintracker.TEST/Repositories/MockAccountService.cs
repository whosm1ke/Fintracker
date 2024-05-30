using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Models.Identity;
using Fintracker.Domain.Entities;
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
                    new RegisterResponse
                    {
                        UserId = new Guid("A6F29D61-2014-43D1-9EAE-0042781DD703")
                    }
                ));

        mock.Setup(x => x.Login(It.IsAny<LoginRequest>()))
            .Returns((LoginRequest lq) => Task.FromResult(new LoginResponse
            {
                UserId = new Guid("A6F29D61-2014-43D1-9EAE-0042781DD703"),
                UserEmail = "test@email.com",
                Token = "Some token here as well"
            }));

        mock.Setup(x => x.GenerateResetEmailToken(It.IsAny<User>(), It.IsAny<string>()))
            .Returns((User user, string email) => Task.FromResult("string"));
        
        mock.Setup(x => x.GenerateResetPasswordToken(It.IsAny<User>()))
            .Returns((User user) => Task.FromResult("string"));

        mock.Setup(x => x.UpdateUserUsername(It.IsAny<string>(), It.IsAny<Guid>()))
            .Returns((string userName, Guid userId) =>
            {
                var user = MockUserRepository.Users.Find(x => x.Id == userId);
                
                if (user is null)
                    throw new NotFoundException(new ExceptionDetails
                    {
                        ErrorMessage = "Can not find user with id " + userId,
                        PropertyName = nameof(userId)
                    }, nameof(User));


                var changeResult = MockUserRepository.Users.Any(u => u.UserName == userName);

                if (changeResult)
                    throw new RegisterAccountException(new ExceptionDetails()
                    {
                        ErrorMessage = "This username is already in use",
                        PropertyName = "userName"
                    });

                user.UserName = userName;

                return Task.FromResult(userName);
            });

        return mock;
    }
}