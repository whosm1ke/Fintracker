using AutoMapper;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Features.User.Handlers.Commands;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Fintracker.TEST.UserTests;

public class UserCommandTests
{
    private readonly IMapper _mapper;

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

        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    public async Task DeleteUserCommand_Should_Be_5_Test()
    {
        var userMockRepo = MockUserRepository.GetUserRepository().Object;
        var handler = new DeleteUserCommandHandler(_mapper, userMockRepo);
        var expectedResult = 5;
        
        var handlerResult = await handler.Handle(new DeleteUserCommand
        {
            Id = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        }, default);

        var actualResult = (await userMockRepo.GetAllAsync()).Count;

        handlerResult.DeletedObj.Should().NotBeNull();
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task UpdateUserCommand_ShouldBeNewUserDetails_Test()
    {
        var userMockRepo = MockUserRepository.GetUserRepository().Object;
        var handler = new UpdateUserCommandHandler(_mapper, userMockRepo);
        var expectedResult = new UserBaseDTO
        {
            Id = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Email = "user1gmail.com",
            UserDetails = new()
            {
                Avatar = "avatar", 
                Language = LanguageTypeEnum.Ukrainian,
                Sex = "Male",
                FName = "Misha"
            }
        };
        var formFileMock = new Mock<IFormFile>();
        formFileMock.SetupGet(x => x.FileName).Returns("avatar");
        var actualResult = await handler.Handle(new UpdateUserCommand
        {
            User = new()
            {
                Id = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
                Email = "user1gmail.com",
                UserDetails = new()
                {
                    Avatar = "avatar",
                    Language = LanguageTypeEnum.Ukrainian,
                    Sex = "Male",
                    FName = "Misha"
                },
                Avatar = formFileMock.Object
                
            },
            WWWRoot = "fake root"
        }, default);

        actualResult.Success.Should().BeTrue();
        actualResult.Old.Should().NotBeNull();
        actualResult.New.Should().NotBeNull();
        actualResult.New.Should().BeEquivalentTo(expectedResult);
    }
}