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

        var actualResult = await handler.Handle(new GetWalletsByOwnerIdRequest
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
            Params = new()
            {
                SortBy = "balance"
            }
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(2);
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    

    [Fact]
    public async Task GetWalletWithOwnerById_Should_Return_66B5_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletByIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new WalletBaseDTO
        {
            Id = new Guid("95E0ECF9-0647-450B-9495-B2A709D166B5"),
            Balance = 500,
            Name = "With Owner",
            Owner = new UserBaseDTO { Id = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9"), Email = "owner@gmail.com" }
        };

        var actualResult = await handler.Handle(new GetWalletByIdRequest
        {
            Id = new Guid("95E0ECF9-0647-450B-9495-B2A709D166B5")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
    
}