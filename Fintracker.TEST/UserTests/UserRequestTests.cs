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

    [Fact]
    public async Task GetUserWithBudget_Should_Return_B10C_Test()
    {
        var mockUserRepo = MockUserRepository.GetUserRepository().Object;
        var handler = new GetUserWithBudgetsByIdRequestHandler(_mapper, mockUserRepo);
        var expectedResult = new UserWithBudgetsDTO
        {
            Id = new Guid("E126CEFE-57A3-4E2A-93A6-5EE7F819B10C"),
            Budgets = new List<BudgetBaseDTO>
            {
                new()
                {
                    Id = new Guid("31F50DEB-14C4-4D36-A0B9-1CF1C316CE43"),
                    Name = "Budget 1",
                    Categories = new List<CategoryDTO>()
                },
                new()
                {
                    Id = new Guid("BE9D9B0D-B483-44D7-BE3B-59D9197AD5C6"),
                    Name = "Budget 2",
                    Categories = new List<CategoryDTO>()
                }
            }
        };

        var actualResult = await handler.Handle(new GetUserWithBudgetsByIdRequest
        {
            Id = new Guid("E126CEFE-57A3-4E2A-93A6-5EE7F819B10C"),
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Budgets.Count.Should().Be(expectedResult.Budgets.Count);
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact]
    public async Task GetUserWithMemberWallets_Should_Return_735C_Test()
    {
        var mockUserRepo = MockUserRepository.GetUserRepository().Object;
        var handler = new GetUserWithMemberWalletsByIdRequestHandler(_mapper, mockUserRepo);
        var expectedResult = new UserWithMemberWalletsDTO
        {
            Id = new Guid("2F566F81-4723-4D28-AB7C-A3004F98735C"),
            Email = "accessToWalletUser1",
            MemberWallets = new List<WalletBaseDTO>
            {
                new()
                {
                    Id = new Guid("BC3CCC22-F825-4522-8FF8-18DE43D198A9"),
                    Name = "Wallet 1",
                },
                new()
                {
                    Id = new Guid("934B7E74-F0F2-47F1-B1AB-87B7E88F0778"),
                    Name = "Wallet 2"
                }
            }
        };

        var actualResult = await handler.Handle(new GetUserWithMemberWalletsByIdRequest
        {
            Id = new Guid("2F566F81-4723-4D28-AB7C-A3004F98735C"),
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.MemberWallets.Count.Should().Be(expectedResult.MemberWallets.Count);
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact]
    public async Task GetUserWithOwnedWallets_Should_Return_0EC5_Test()
    {
        var mockUserRepo = MockUserRepository.GetUserRepository().Object;
        var handler = new GetUserWithOwnedWalletsByIdRequestHandler(_mapper, mockUserRepo);
        var expectedResult = new UserWithOwnedWalletsDTO
        {
            Id = new Guid("5718AD4F-3065-4E46-85A4-785E64F60EC5"),
            OwnedWallets = new List<WalletBaseDTO>
            {
                new ()
                {
                    Id = new Guid("7183A82C-CBDB-43B1-8FE9-A0525109731A"),
                    Name= "Owned wallet 1"
                },
                new ()
                {
                    Id = new Guid("87E1885E-11D6-4029-B3D6-C3FA849628BB"),
                    Name= "Owned wallet 2"
                }
            }
        };

        var actualResult = await handler.Handle(new GetUserWithOwnedWalletsByIdRequest
        {
            Id = new Guid("5718AD4F-3065-4E46-85A4-785E64F60EC5"),
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.OwnedWallets.Count.Should().Be(expectedResult.OwnedWallets.Count);
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}