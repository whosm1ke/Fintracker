using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Handlers.Commands;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using Xunit.Abstractions;

namespace Fintracker.TEST.WalletTests;

public class WalletCommandTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;

    public WalletCommandTests(ITestOutputHelper output)
    {
        _output = output;
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<WalletProfile>();
            c.AddProfile<UserProfile>();
        });

        _mapper = mapperCfg.CreateMapper();
    }
    
     [Fact]
    public async Task AddAsync_Should_Return_True()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new CreateWalletCommandHandler(mockUnitOfWork, _mapper);
        var walletToAdd = _fixture.Build<CreateWalletDTO>()
            .With(c => c.Name, "New Budget")
            .With(c => c.Balance, 100)
            .With(x => x.OwnerId, new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .With(x => x.CurrencyId,new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"))
            .Create();

        var result = await handler.Handle(new CreateWalletCommand
        {
            Wallet = walletToAdd
        }, default);
        
        var budgetsCount = (await mockUnitOfWork.WalletRepository.GetAllAsync()).Count;
        result.Success.Should().BeTrue();
        result.CreatedObject.Should().NotBeNull();
        result.CreatedObject.Should().BeOfType<WalletBaseDTO>();
        budgetsCount.Should().Be(8);
    }
    
    [Fact]
    public async Task UpdateAsync_Should_Return_True_And_Old_With_New_Objects()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new UpdateWalletCommandHandler(mockUnitOfWork, _mapper);
        var budgetToUpdate = _fixture.Build<UpdateWalletDTO>()
            .With(c => c.Name, "This is a new wallet")
            .With(c => c.Balance, 101230)
            .With(x => x.Id,new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"))
            .With(x => x.CurrencyId,new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"))
            .Create();
        
        var result = await handler.Handle(new UpdateWalletCommand
        {
            Wallet = budgetToUpdate
        }, default);
        
        result.Success.Should().BeTrue();
        result.Old.Should().NotBeNull();
        result.New.Should().NotBeNull();
        result.Old.Should().BeOfType<WalletBaseDTO>();
        result.New.Should().BeOfType<WalletBaseDTO>();
    }
    
    [Fact]
    public async Task DeleteAsync_Should_Return_True_And_Deleted_Object()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new DeleteWalletCommandHandler(mockUnitOfWork, _mapper);
        
        var result = await handler.Handle(new DeleteWalletCommand
        {
            Id = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0")
        }, default);

        var budgetCountAfterDelete = (await mockUnitOfWork.WalletRepository.GetAllAsync()).Count;
        
        result.Success.Should().BeTrue();
        result.DeletedObj.Should().NotBeNull();
        result.DeletedObj.Should().BeOfType<WalletBaseDTO>();
        budgetCountAfterDelete.Should().Be(6);
    }
}