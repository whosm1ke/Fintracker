using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Handlers.Commands;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;

namespace Fintracker.TEST.BudgetTests;

public class BudgetCommandTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    public BudgetCommandTests()
    {
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<BudgetProfile>();
            c.AddProfile<CurrencyProfile>();
        });

        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    public async Task AddAsync_Should_Return_True()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var mockUserRepository = MockUserRepository.GetUserRepository().Object;
        var handler = new CreateBudgetCommandHandler(mockUnitOfWork, _mapper, mockUserRepository);
        var budgetToAdd = _fixture.Build<CreateBudgetDTO>()
            .With(c => c.Name, "New Budget")
            .With(c => c.Balance, 100)
            .With(c => c.EndDate, new DateTime(2024, 12, 12))
            .With(c => c.StartDate, new DateTime(2024, 12, 10))
            .With(x => x.CategoryIds, new List<Guid> {new("77326B96-DF2B-4CC8-93A3-D11A276433D6")})
            .With(x => x.UserId,new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .With(x => x.WalletId,new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"))
            .With(x => x.CurrencyId,new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"))
            .Create();

        var result = await handler.Handle(new CreateBudgetCommand
        {
            Budget = budgetToAdd
        }, default);
        
        var budgetsCount = (await mockUnitOfWork.BudgetRepository.GetAllAsync()).Count;
        result.Success.Should().BeTrue();
        result.CreatedObject.Should().NotBeNull();
        result.CreatedObject.Should().BeOfType<CreateBudgetDTO>();
        budgetsCount.Should().Be(6);
    }
    
    [Fact]
    public async Task UpdateAsync_Should_Return_True_And_Old_With_New_Objects()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new UpdateBudgetCommandHandler(mockUnitOfWork, _mapper);
        var budgetToUpdate = _fixture.Build<UpdateBudgetDTO>()
            .With(c => c.Name, "This is a new budget")
            .With(c => c.Balance, 100)
            .With(c => c.EndDate, new DateTime(2024, 10, 12))
            .With(c => c.StartDate, new DateTime(2024, 9, 10))
            .With(x => x.CategoryIds, new List<Guid> {new("77326B96-DF2B-4CC8-93A3-D11A276433D6")})
            .With(x => x.Id,new Guid("29DECA11-E633-47E4-A0B2-569791B7D8C7"))
            .With(x => x.WalletId,new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"))
            .With(x => x.CurrencyId,new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"))
            .Create();
        
        var result = await handler.Handle(new UpdateBudgetCommand
        {
            Budget = budgetToUpdate
        }, default);
        
        result.Success.Should().BeTrue();
        result.Old.Should().NotBeNull();
        result.New.Should().NotBeNull();
        result.Old.Should().BeOfType<BudgetBaseDTO>();
        result.New.Should().BeOfType<BudgetBaseDTO>();
    }
    
    [Fact]
    public async Task DeleteAsync_Should_Return_True_And_Deleted_Object()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new DeleteBudgetCommandHandler(mockUnitOfWork, _mapper);
        
        var result = await handler.Handle(new DeleteBudgetCommand
        {
            Id = new Guid("29DECA11-E633-47E4-A0B2-569791B7D8C7")
        }, default);

        var budgetCountAfterDelete = (await mockUnitOfWork.BudgetRepository.GetAllAsync()).Count;
        
        result.Success.Should().BeTrue();
        result.DeletedObj.Should().NotBeNull();
        result.DeletedObj.Should().BeOfType<BudgetBaseDTO>();
        budgetCountAfterDelete.Should().Be(4);
    }
}