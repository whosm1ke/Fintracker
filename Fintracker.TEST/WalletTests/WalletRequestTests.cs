using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Handlers.Queries;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;

namespace Fintracker.TEST.WalletTests;

public class WalletRequestTests
{
    private readonly IMapper _mapper;

    public WalletRequestTests()
    {
        new Fixture();
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<BudgetProfile>();
            c.AddProfile<CurrencyProfile>();
            c.AddProfile<WalletProfile>();
            c.AddProfile<UserProfile>();
            c.AddProfile<CategoryProfile>();
            c.AddProfile<TransactionProfile>();
        });

        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    public async Task GetWalletsByOwnerId_Should_Return_52A0_BE71_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletsByOwnerIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new List<WalletBaseDTO>()
        {
            new()
            {
                Id = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
                Balance = 1000,
                Name = "Wallet 1",
                OwnerId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9")
            },
            new()
            {
                Id = new Guid("16E4E7F0-48EE-46E6-9543-ABA7975FBE71"),
                Balance = 2000,
                Name = "Wallet 2",
                OwnerId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9")
            }
        };

        var actualResult = await handler.Handle(new GetWalletsByOwnerIdRequest()
        {
            OwnerId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(2);
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetWalletsByOwnerIdSorted_SortBy_Balance_Should_Return_52A0_BE71_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletsByOwnerIdSortedRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new List<WalletBaseDTO>
        {
            new()
            {
                Id = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
                Balance = 1000,
                Name = "Wallet 1",
                OwnerId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9")
            },
            new()
            {
                Id = new Guid("16E4E7F0-48EE-46E6-9543-ABA7975FBE71"),
                Balance = 2000,
                Name = "Wallet 2",
                OwnerId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9")
            }
        };

        var actualResult = await handler.Handle(new GetWalletsByOwnerIdSortedRequest
        {
            OwnerId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9"),
            SortBy = "Balance"
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(2);
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetWalletWithBudgetsById_Should_Return_8D6E_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletWithBudgetsByIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new WalletWithBudgetsDTO()
        {
            Id = new Guid("32A22A34-F772-4F65-B806-51B2E8528D6E"),
            Name = "With Budgets",
            Budgets = new List<BudgetBaseDTO>()
            {
                new()
                {
                    Id = new Guid("438A3485-E4F0-4C79-971C-DC07FB92BAD8"),
                    Name = "Budget 1",
                    Categories = new List<CategoryDTO>()
                },
                new()
                {
                    Id = new Guid("B036C34F-FD3F-484C-9CA2-7E603E5E076A"),
                    Name = "Budget 2",
                    Categories = new List<CategoryDTO>()
                }
            }
        };

        var actualResult = await handler.Handle(new GetWalletWithBudgetsByIdRequest
        {
            Id = new Guid("32A22A34-F772-4F65-B806-51B2E8528D6E"),
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetWalletWithMembersById_Should_Return_D528_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletWithMembersByIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new WalletWithMembersDTO()
        {
            Id = new Guid("83D7946B-3CCD-401E-8EF4-62BCA04FD528"),
            Balance = 2000,
            Name = "With Members",
            Users = new List<UserBaseDTO>()
            {
                new()
                {
                    Id = new Guid("2045DCCE-ED9E-4880-ABF8-1710C678BA3F"),
                    Email = "member1@gmail.com"
                },
                new()
                {
                    Id = new Guid("99E84605-35FB-491A-ADC5-523516612B41"),
                    Email = "member2@gmail.com"
                }
            }
        };

        var actualResult = await handler.Handle(new GetWalletWithMembersByIdRequest
        {
            Id = new Guid("83D7946B-3CCD-401E-8EF4-62BCA04FD528"),
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetWalletWithOwnerById_Should_Return_66B5_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletByIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new WalletBaseDTO()
        {
            Id = new Guid("95E0ECF9-0647-450B-9495-B2A709D166B5"),
            Balance = 500,
            Name = "With Owner",
            Owner = new UserBaseDTO()
                { Id = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9"), Email = "owner@gmail.com" }
        };

        var actualResult = await handler.Handle(new GetWalletByIdRequest
        {
            Id = new Guid("95E0ECF9-0647-450B-9495-B2A709D166B5")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetWalletWithTransactionsById_Should_Return_E6A7_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletWithTransactionsByIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new WalletWithTransactionsDTO()
        {
            Id = new Guid("8ED1883D-1833-47CB-8E12-27AC26F5E6A7"),
            Name = "With Transactions",
            Transactions = new List<TransactionBaseDTO>()
            {
                new()
                {
                    Id = new Guid("0F47F18E-5DE9-429B-9EF9-4CF74F338EE3"),
                    Note = "transaction 1"
                },
                new()
                {
                    Id = new Guid("DABB1876-F428-4C8C-B04A-52F94582DFCF"),
                    Note = "transaction 2"
                }
            }
        };

        var actualResult = await handler.Handle(new GetWalletWithTransactionsByIdRequest()
        {
            Id = new Guid("8ED1883D-1833-47CB-8E12-27AC26F5E6A7"),
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}