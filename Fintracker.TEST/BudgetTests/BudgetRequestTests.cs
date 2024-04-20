using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Handlers.Queries;
using Fintracker.Application.Features.Budget.Requests.Queries;
using Fintracker.Application.MapProfiles;
using Fintracker.Domain.Entities;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using Xunit.Abstractions;

namespace Fintracker.TEST.BudgetTests;

public class BudgetRequestTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;

    public BudgetRequestTests(ITestOutputHelper output)
    {
        _output = output;
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<BudgetProfile>();
            c.AddProfile<CurrencyProfile>();
            c.AddProfile<WalletProfile>();
            c.AddProfile<UserProfile>();
            c.AddProfile<CategoryProfile>();
        });

        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    public async Task GetBudgetByIdRequest_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetBudgetByIdRequestHandler(mockUnitOfWork, _mapper);
        var expectedBudget = _mapper.Map<BudgetBaseDTO>(new Budget
        {
            Balance = 1000,
            CreatedAt = new DateTime(2024, 12, 12),
            ModifiedAt = new DateTime(2024, 12, 12),
            CreatedBy = "ME",
            ModifiedBy = "ME",
            Currency = new Currency(),
            EndDate = new DateTime(2024, 11, 12),
            StartDate = new DateTime(2024, 11, 14),
            Name = "B",
            Id = new Guid("5F5F42ED-345C-4B13-AA35-76005A9607FF"),
            Wallet = new Wallet(),
            User = new User(),
            Categories = new List<Category>(),
            WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            CurrencyId = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61")
        });

        var actualBudget = await handler.Handle(new GetBudgetByIdRequest
        {
            Id = new Guid("5F5F42ED-345C-4B13-AA35-76005A9607FF")
        }, default);

        actualBudget.Should().NotBeNull();
        actualBudget.Should().BeEquivalentTo(expectedBudget);
    }

    [Fact]
    public async Task GetBudgetsByUserIdRequest_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetBudgetsByUserIdRequestHandler(mockUnitOfWork, _mapper);
       
        

        var actualBudgets = (await handler.Handle(new GetBudgetsByUserIdRequest
        {
            UserId = new Guid("E126CEFE-57A3-4E2A-93A6-5EE7F819B10C")
        }, default)).ToList();

        actualBudgets.Should().NotBeNull();
        actualBudgets.Count.Should().Be(0);
    }

    [Fact]
    public async Task GetBudgetsByUserIdSortedRequest_Sort_By_Name_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetBudgetsByUserIdSortedRequestHandler(mockUnitOfWork, _mapper);

        var actualSortedBudgets = await handler.Handle(new GetBudgetsByUserIdSortedRequest
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Params = new()
            {
                SortBy = "name"
            }
        }, default);

        actualSortedBudgets.Should().NotBeNull();
        actualSortedBudgets.Count.Should().Be(0);
    }

    [Fact]
    public async Task GetBudgetsByWalletIdRequest_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetBudgetsByWalletIdRequestHandler(mockUnitOfWork, _mapper);
        var actualBudgets = (await handler.Handle(new GetBudgetsByWalletIdRequest
        {
            WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0")
        }, default)).ToList();

        actualBudgets.Should().NotBeNull();
        actualBudgets.Count.Should().Be(0);
    }

    [Fact]
    public async Task GetBudgetsByWalletIdSortedRequest_Sort_By_Name_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetBudgetsByWalletIdSortedRequestHandler(mockUnitOfWork, _mapper);

        var actualSortedBudgets = await handler.Handle(new GetBudgetsByWalletIdSortedRequest
        {
            WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
            Params = new()
            {
                SortBy = "name"
            }
        }, default);

        actualSortedBudgets.Should().NotBeNull();
        actualSortedBudgets.Count.Should().Be(0);
    }
    
}