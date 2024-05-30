using AutoFixture;
using AutoMapper;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Handlers.Queries;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using Fintracker.Application.MapProfiles;
using Fintracker.Application.Models;
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
    public async Task Test_GetWalletsByUserId_With_Valid_UserIds()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletsByUserIdRequestHandler(_mapper, mockUnitOfWork);

        var actualResult = await handler.Handle(new GetWalletsByUserIdRequest()
        {
            UserId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult[0].Id.Should().Be(new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"));
        actualResult[1].Id.Should().Be(new Guid("16E4E7F0-48EE-46E6-9543-ABA7975FBE71"));
    }
    
    [Fact]
    public async Task Test_GetWalletsByUserId_With_InValid_UserIds_Should_Be_Count_0()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletsByUserIdRequestHandler(_mapper, mockUnitOfWork);

        var actualResult = await handler.Handle(new GetWalletsByUserIdRequest()
        {
            UserId = new Guid("BFA913C1-2570-49D5-9F3F-F55C5B0C8692")
        }, default);

        actualResult.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task Test_GetWalletsByUserIdSorted_With_Valid_UserIds()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletsByUserIdSortedRequestHandler(_mapper, mockUnitOfWork);

        var actualResult = await handler.Handle(new GetWalletsByUserIdSortedRequest()
        {
            UserId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9"),
            Params = new QueryParams()
            {
                SortBy = "balance",
            }
        }, default);

        actualResult.Should().NotBeNull();
        actualResult[0].Id.Should().Be(new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"));
        actualResult[1].Id.Should().Be(new Guid("16E4E7F0-48EE-46E6-9543-ABA7975FBE71"));
        actualResult.Should().BeInAscendingOrder(x => x.Balance);
    }
    
    [Fact]
    public async Task Test_GetWalletsByUserIdSorted_With_InValid_UserIds_Should_Be_Count_0()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletsByUserIdSortedRequestHandler(_mapper, mockUnitOfWork);

        var actualResult = await handler.Handle(new GetWalletsByUserIdSortedRequest()
        {
            UserId = new Guid("E84D3DEB-7FAB-43B5-A860-C7D8F522007F"),
            Params = new QueryParams()
            {
                SortBy = "balance",
            }
        }, default);

        actualResult.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task Test_GetWalletById_With_Valid_Id_Should_Return_Wallet1()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletByIdRequestHandler(_mapper, mockUnitOfWork);

        var actualResult = await handler.Handle(new GetWalletByIdRequest()
        {
            Id = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Id.Should().Be(new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"));
        actualResult.Name.Should().Be("Wallet 1");
    }
    
    [Fact]
    public async Task Test_GetWalletById_With_Invalid_Id_Should_Throw_NotFound()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetWalletByIdRequestHandler(_mapper, mockUnitOfWork);

        var invalidId = new Guid("A7A4047F-EE9C-4AFD-B96F-746A63B7665E"); // Ensure this ID doesn't exist

        // Assert
        Func<Task> act = async () => await handler.Handle(new GetWalletByIdRequest()
        {
            Id = invalidId
        }, default);

        await act.Should().ThrowAsync<NotFoundException>(); // FluentAssertions
    }

    
}