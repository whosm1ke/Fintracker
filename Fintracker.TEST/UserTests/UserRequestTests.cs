using AutoMapper;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.User.Handlers.Queries;
using Fintracker.Application.Features.User.Requests.Queries;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;

namespace Fintracker.TEST.UserTests;

public class UserRequestTests
{
    private readonly IMapper _mapper;

    public UserRequestTests()
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
    public async Task GetUserById_Should_Return_56C7_Test()
    {
        var mockUserRepo = MockUserRepository.GetUserRepository().Object;
        var handler = new GetUserByIdRequestHandler(_mapper, mockUserRepo);
        var expectedResult = new UserBaseDTO
        {
            Id = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Email = "user1@gmail.com",
        };

        var actualResult = await handler.Handle(new GetUserByIdRequest
        {
            Id = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetUsersAccessedToWallet_98A9_Should_Return_735C_53BE_Test()
    {
        var mockUserRepo = MockUserRepository.GetUserRepository().Object;
        var handler = new GetUsersAccessedToWalletRequestHandler(_mapper, mockUserRepo);
        var expectedResult = new List<UserBaseDTO>
        {
            new()
            {
                Id = new Guid("2F566F81-4723-4D28-AB7C-A3004F98735C"),
                Email = "accessToWalletUser1",
            },
            new()
            {
                Id = new Guid("D4577085-22CE-4DE3-91E2-7C454C9653BE"),
                Email = "accessToWalletUser2",
            }
        };

        var actualResult = await handler.Handle(new GetUsersAccessedToWalletRequest
        {
            WalletId = new Guid("BC3CCC22-F825-4522-8FF8-18DE43D198A9")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(expectedResult.Count);
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

   
}