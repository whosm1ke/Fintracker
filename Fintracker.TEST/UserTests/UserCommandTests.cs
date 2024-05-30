using AutoFixture;
using AutoMapper;
using Fintracker.Application;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Invite.Validators;
using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.User.Validators;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Features.User.Handlers.Commands;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.Application.Models.Mail;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace Fintracker.TEST.UserTests;

public class UserCommandTests
{
    private readonly IMapper _mapper;
    private readonly AddUserToWalletValidator _addUserToWalletValidator;
    private readonly InviteUserValidator _inviteUserValidator;
    private readonly UpdateUserDtoValidator _updateUserValidator;
    private readonly Mock<IOptions<AppSettings>> _mockAppSettings;
    private readonly IUserRepository _mockUserRepo;
    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly IFixture _fixture;


    public UserCommandTests()
    {
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<WalletProfile>();
            c.AddProfile<UserProfile>();
            c.AddProfile<CategoryProfile>();
            c.AddProfile<BudgetProfile>();
            c.AddProfile<CurrencyProfile>();
        });

        _fixture = new Fixture();

        _mockAppSettings = new Mock<IOptions<AppSettings>>();
        _mockAppSettings.Setup(x => x.Value).Returns(new AppSettings()
        {
            BaseUrl = "https://test-base-url.com",
            AllowedExtensions = new[] { ".jpg", ".png" }
        });

        _mockUserRepo = MockUserRepository.GetUserRepository().Object;
        _mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;

        _addUserToWalletValidator = new AddUserToWalletValidator(_mockUserRepo, _mockUnitOfWork);
        _inviteUserValidator = new InviteUserValidator(_mockUserRepo, _mockUnitOfWork);
        _updateUserValidator = new UpdateUserDtoValidator(_mockUserRepo, _mockAppSettings.Object, _mockUnitOfWork);


        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    private async Task Test_AddUserToWallet_With_Valid_Params_Should_Be_Added()
    {
        var mediatorMock = new Mock<IMediator>();

        var handler =
            new AddUserToWalletCommandHandler(_mockUserRepo, _mockUnitOfWork, mediatorMock.Object,
                _mockAppSettings.Object);

        var command = new AddUserToWalletCommand()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
            PathToCategories = "Path"
        };


        var validationResult =
            await _addUserToWalletValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());
        validationResult.ShouldNotHaveAnyValidationErrors();

        var result = await handler.Handle(command, default);

        result.Success.Should().BeTrue();
        mediatorMock.Verify(m => m.Send(It.IsAny<PopulateUserWithCategoriesCommand>(), It.IsAny<CancellationToken>()),
            Times.Once); // Ensure command is sent
    }

    [Fact]
    private async Task Test_AddUserToWallet_With_Invalid_Params_Should_Not_Be_Added()
    {
        var mockAppSettings = new Mock<IOptions<AppSettings>>();
        mockAppSettings.Setup(x => x.Value).Returns(new AppSettings()
        {
            BaseUrl = "https://test-base-url.com"
        });
        var command = new AddUserToWalletCommand()
        {
            UserId = new Guid("FEC7B8A3-4821-4A9D-8886-7345A1DD448E"),
            WalletId = new Guid("1239BBB3-3A4A-4065-917F-D15D7621D283"),
            PathToCategories = "Path"
        };

        var validationResult =
            await _addUserToWalletValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());
        validationResult.ShouldHaveAnyValidationError();
    }

    [Fact]
    private async Task Test_DeleteUser_With_Valid_Params_Should_Be_Deleted()
    {
        var handler = new DeleteUserCommandHandler(_mapper, _mockUserRepo);

        var command = new DeleteUserCommand()
        {
            Id = new Guid("6090BFC8-4D8D-4AF5-9D37-060581F051A7"),
        };


        var result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.DeletedObj.Should().NotBeNull();
        result.Success.Should().BeTrue();

        int usersCount = (await _mockUserRepo.GetAllAsync()).Count;
        usersCount.Should().Be(3);
    }

    [Fact]
    private async Task Test_DeleteUser_With_Invalid_Params_Should_Throw_NotFound()
    {
        var handler = new DeleteUserCommandHandler(_mapper, _mockUserRepo);

        var command = new DeleteUserCommand()
        {
            Id = new Guid("B34063CA-A5D8-47C5-8633-AFC870F8846F"),
        };


        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<NotFoundException>(); // FluentAssertions
    }

    [Fact]
    private async Task Test_InviteUserCommand_ForExistingUser_Should_Return_Unit()
    {
        var emailSenderMock = new Mock<IEmailSender>();
        emailSenderMock.Setup(x => x.SendEmail(It.IsAny<EmailModel>(), It.IsAny<InviteEmailModel>()))
            .Returns((EmailModel model, InviteEmailModel e) => Task.FromResult(true));
        var handler = new InviteUserCommandHandler(emailSenderMock.Object, _mockUserRepo, _mockAppSettings.Object);

        var command = new InviteUserCommand()
        {
            WhoInvited = "myemail@gmail.com",
            WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
            UserEmail = "user2@gmail.com",
            UrlCallback = "callback"
        };

        var validationResult =
            await _inviteUserValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());
        validationResult.ShouldNotHaveAnyValidationErrors();

        var result = await handler.Handle(command, default);

        emailSenderMock.Verify(m => m.SendEmail(It.IsAny<EmailModel>(), It.IsAny<InviteEmailModel>()),
            Times.Once); // Ensure command is sent

        result.Should().BeEquivalentTo(Unit.Value);
    }

    [Fact]
    private async Task Test_InviteUserCommand_ForNon_ExistingUser_Should_Return_Unit()
    {
        var emailSenderMock = new Mock<IEmailSender>();
        emailSenderMock.Setup(x => x.SendEmail(It.IsAny<EmailModel>(), It.IsAny<InviteEmailModel>()))
            .Returns((EmailModel model, InviteEmailModel e) => Task.FromResult(true));
        var handler = new InviteUserCommandHandler(emailSenderMock.Object, _mockUserRepo, _mockAppSettings.Object);

        var command = new InviteUserCommand()
        {
            WhoInvited = "myemail@gmail.com",
            WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
            UserEmail = "nonexistingone@gmail.com",
            UrlCallback = "callback"
        };

        var validationResult =
            await _inviteUserValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());
        validationResult.ShouldNotHaveAnyValidationErrors();

        var result = await handler.Handle(command, default);

        emailSenderMock.Verify(m => m.SendEmail(It.IsAny<EmailModel>(), It.IsAny<InviteEmailModel>()),
            Times.Once); // Ensure command is sent

        result.Should().BeEquivalentTo(Unit.Value);
    }

    [Fact]
    private async Task Test_InviteUserCommand_When_Email_Was_Not_Sent_Should_Throw_BadRequest()
    {
        var emailSenderMock = new Mock<IEmailSender>();
        emailSenderMock.Setup(x => x.SendEmail(It.IsAny<EmailModel>(), It.IsAny<InviteEmailModel>()))
            .Returns((EmailModel model, InviteEmailModel e) => Task.FromResult(false));
        var handler = new InviteUserCommandHandler(emailSenderMock.Object, _mockUserRepo, _mockAppSettings.Object);

        var command = new InviteUserCommand()
        {
            WhoInvited = "myemail@gmail.com",
            WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
            UserEmail = "email@gmail.com",
            UrlCallback = "callback"
        };

        var validationResult =
            await _inviteUserValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());
        validationResult.ShouldNotHaveAnyValidationErrors();

        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact]
    private async Task Test_InviteUserCommand_With_Invalid_Params_Should_Have_Validation_Errors()
    {
        var command = new InviteUserCommand()
        {
            WhoInvited = "myemail@gmail.com",
            WalletId = new Guid("854FFDB4-58B7-416C-8F25-CC82405370E1"),
            UserEmail = "myemail@gmail.com",
            UrlCallback = "callback"
        };

        var validationResult =
            await _inviteUserValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());
        validationResult.ShouldHaveAnyValidationError();
    }

    [Fact]
    private async Task Test_SentResetEmail_For_ExistingUser_Should_Return_Unit()
    {
        var emailSenderMock = new Mock<IEmailSender>();
        emailSenderMock
            .Setup(x => x.SendEmail(It.IsAny<EmailModel>(),
                It.IsAny<object>())) // Match any object as the second parameter
            .ReturnsAsync(true); // Always return true to simulate successful email sending

        var handler = new SentResetEmailCommandHandler(MockAccountService.GetAccountService().Object,
            emailSenderMock.Object, _mockUserRepo, _mockAppSettings.Object);

        var command = new SentResetEmailCommand()
        {
            UserId = new Guid("6090BFC8-4D8D-4AF5-9D37-060581F051A7"),
            NewEmail = "newEmail@gmail.com",
            UrlCallback = "callback"
        };


        var result = await handler.Handle(command, default);

        emailSenderMock.Verify(m => m.SendEmail(It.IsAny<EmailModel>(), It.IsAny<object>()),
            Times.Once); // Ensure command is sent

        result.Should().BeEquivalentTo(Unit.Value);
    }

    [Fact]
    private async Task Test_SentResetEmail_For_Non_ExistingUser_Should_Throw_BadRequest()
    {
        var emailSenderMock = new Mock<IEmailSender>();
        emailSenderMock
            .Setup(x => x.SendEmail(It.IsAny<EmailModel>(),
                It.IsAny<object>())) // Match any object as the second parameter
            .ReturnsAsync(true); // Always return true to simulate successful email sending

        var handler = new SentResetEmailCommandHandler(MockAccountService.GetAccountService().Object,
            emailSenderMock.Object, _mockUserRepo, _mockAppSettings.Object);

        var command = new SentResetEmailCommand()
        {
            UserId = new Guid("39AAA154-253A-473D-8E8E-F578D43D6284"),
            NewEmail = "newEmail@gmail.com",
            UrlCallback = "callback"
        };

        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Invalid email. Check spelling.");
    }

    [Fact]
    private async Task Test_SentResetEmail_For_ExistingUser_But_Email_Was_Not_Sent_Should_Throw_BadRequest()
    {
        var emailSenderMock = new Mock<IEmailSender>();
        emailSenderMock
            .Setup(x => x.SendEmail(It.IsAny<EmailModel>(),
                It.IsAny<object>())) // Match any object as the second parameter
            .ReturnsAsync(false);

        var handler = new SentResetEmailCommandHandler(MockAccountService.GetAccountService().Object,
            emailSenderMock.Object, _mockUserRepo, _mockAppSettings.Object);

        var command = new SentResetEmailCommand()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            NewEmail = "newEmail@gmail.com",
            UrlCallback = "callback"
        };

        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage($"Was not sent to {command.NewEmail}. Check spelling");
    }


    [Fact]
    private async Task Test_SentPasswordEmail_For_ExistingUser_Should_Return_Unit()
    {
        var emailSenderMock = new Mock<IEmailSender>();
        emailSenderMock
            .Setup(x => x.SendEmail(It.IsAny<EmailModel>(),
                It.IsAny<object>())) // Match any object as the second parameter
            .ReturnsAsync(true); // Always return true to simulate successful email sending

        var handler = new SentResetPasswordCommandHandler(MockAccountService.GetAccountService().Object,
            _mockUserRepo, emailSenderMock.Object, _mockAppSettings.Object);

        var command = new SentResetPasswordCommand()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Email = "user2@gmail.com",
            UrlCallback = "callback"
        };


        var result = await handler.Handle(command, default);

        emailSenderMock.Verify(m => m.SendEmail(It.IsAny<EmailModel>(), It.IsAny<object>()),
            Times.Once); // Ensure command is sent

        result.Should().BeEquivalentTo(Unit.Value);
    }

    [Fact]
    private async Task Test_SentResetPassword_For_Non_ExistingUser_Should_Throw_NotFound()
    {
        var emailSenderMock = new Mock<IEmailSender>();
        emailSenderMock
            .Setup(x => x.SendEmail(It.IsAny<EmailModel>(),
                It.IsAny<object>())) // Match any object as the second parameter
            .ReturnsAsync(true); // Always return true to simulate successful email sending

        var handler = new SentResetPasswordCommandHandler(MockAccountService.GetAccountService().Object,
            _mockUserRepo, emailSenderMock.Object, _mockAppSettings.Object);

        var command = new SentResetPasswordCommand()
        {
            UserId = new Guid("39AAA154-253A-473D-8E8E-F578D43D6284"),
            Email = "newEmail@gmail.com",
            UrlCallback = "callback"
        };

        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"User with id was not founded [{command.UserId}]");
    }

    [Fact]
    private async Task Test_SentResetPassword_For_ExistingUser_But_Email_Was_Not_Sent_Should_Throw_BadRequest()
    {
        var emailSenderMock = new Mock<IEmailSender>();
        emailSenderMock
            .Setup(x => x.SendEmail(It.IsAny<EmailModel>(),
                It.IsAny<object>())) // Match any object as the second parameter
            .ReturnsAsync(false);

        var handler = new SentResetPasswordCommandHandler(MockAccountService.GetAccountService().Object,
            _mockUserRepo, emailSenderMock.Object, _mockAppSettings.Object);

        var command = new SentResetPasswordCommand()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Email = "newEmail@gmail.com",
            UrlCallback = "callback"
        };

        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage($"Was not sent to {command.Email}. Check spelling");
    }

    [Fact]
    public async Task UpdateUser_With_Valid_Params_Should_Return_True()
    {
        var userToUpdate = _fixture.Build<UpdateUserDTO>()
            .With(x => x.Id, new Guid("2F566F81-4723-4D28-AB7C-A3004F98735C"))
            .With(x => x.CurrencyId, new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"))
            .With(x => x.UserDetails, new UserDetailsDTO()
            {
                Avatar = "New Avatar URL.jpg",
                Language = LanguageTypeEnum.English,
                Sex = "Male",
                DateOfBirth = new DateTime(2024, 12, 12)
            })
            .Without(x => x.Avatar)
            .Create();

        var command = new UpdateUserCommand()
        {
            User = userToUpdate,
            WWWRoot = "Path"
        };

        var validationResult =
            await _updateUserValidator.TestValidateAsync(command);
        validationResult.ShouldNotHaveAnyValidationErrors();

        var handler = new UpdateUserCommandHandler(_mapper, _mockUserRepo);

        await handler.Handle(command, default);
        var expectedUser =
            await _mockUserRepo.GetAsync(new Guid("2F566F81-4723-4D28-AB7C-A3004F98735C"));

        expectedUser.UserDetails.Sex.Should().Be(userToUpdate.UserDetails.Sex);
        expectedUser.CurrencyId.Should().Be(userToUpdate.CurrencyId);
    }

    [Fact]
    public async Task UpdateUser_With_Invalid_Params_Should_Return_NotFound_And_Has_Validation_Errors()
    {
        var handler = new UpdateUserCommandHandler(_mapper, _mockUserRepo);
        var userToUpdate = _fixture.Build<UpdateUserDTO>()
            .With(x => x.Id, new Guid("5F3636E0-F240-45F4-B1FC-13A53229A976"))
            .With(x => x.CurrencyId, new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"))
            .With(x => x.UserDetails, new UserDetailsDTO()
            {
                Avatar = "New Avatar URL.not_allowed_extension",
                Language = LanguageTypeEnum.English,
                Sex = "Male",
                DateOfBirth = new DateTime(2024, 12, 12)
            })
            .Without(x => x.Avatar)
            .Create();

        var command = new UpdateUserCommand()
        {
            User = userToUpdate,
            WWWRoot = "Path"
        };

        var validationResult =
            await _updateUserValidator.TestValidateAsync(command);
        validationResult.ShouldHaveAnyValidationError();

        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<NotFoundException>(); // FluentAssertions
    }

    [Fact]
    public async Task UpdateUserName_With_Valid_Params_Should_Return_New_Username()
    {
        var command = new UpdateUserUsernameCommand()
        {
            UserId = new Guid("6090BFC8-4D8D-4AF5-9D37-060581F051A7"),
            NewUsername = "NewUserName"
        };

        var handler = new UpdateUserUsernameCommandHandler(MockAccountService.GetAccountService().Object);

        var actualResult = await handler.Handle(command, default);


        var expectedUser =
            await _mockUserRepo.GetAsync(new Guid("6090BFC8-4D8D-4AF5-9D37-060581F051A7"));

        actualResult.Success.Should().BeTrue();
        actualResult.Message.Should().Be(expectedUser.UserName);
    }

    [Fact]
    public async Task UpdateUserName_With_Invalid_Params_Should_Throw_NotFound()
    {
        var command = new UpdateUserUsernameCommand()
        {
            UserId = new Guid("3AB94A4F-DAAE-489B-99BE-402394DE6726"),
            NewUsername = "NewUserName"
        };

        var handler = new UpdateUserUsernameCommandHandler(MockAccountService.GetAccountService().Object);


        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Can not find user with id {command.UserId}");
    }

    [Fact]
    public async Task UpdateUserName_With_Existing_Username_Should_Throw_RegisterAccountException()
    {
        var command = new UpdateUserUsernameCommand()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            NewUsername = "username1"
        };

        var handler = new UpdateUserUsernameCommandHandler(MockAccountService.GetAccountService().Object);


        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<RegisterAccountException>();
    }
}