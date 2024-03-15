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
        var expectedBudgets = new List<BudgetBaseDTO>
        {
            _mapper.Map<BudgetBaseDTO>(new Budget
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
            }),
            _mapper.Map<BudgetBaseDTO>(new Budget
            {
                Balance = 2000,
                CreatedAt = new DateTime(2024, 12, 12),
                ModifiedAt = new DateTime(2024, 12, 12),
                CreatedBy = "ME",
                ModifiedBy = "ME",
                Currency = new Currency(),
                EndDate = new DateTime(2024, 12, 12),
                StartDate = new DateTime(2024, 12, 14),
                Name = "A",
                Id = new Guid("29DECA11-E633-47E4-A0B2-569791B7D8C7"),
                Wallet = new Wallet(),
                User = new User(),
                Categories = new List<Category>(),
                UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
                CurrencyId = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"),
                WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0")
            })
        };

        var actualBudgets = (await handler.Handle(new GetBudgetsByUserIdRequest
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        }, default)).ToList();

        actualBudgets.Should().NotBeNull();
        actualBudgets.Should().BeEquivalentTo(expectedBudgets);
    }

    [Fact]
    public async Task GetBudgetsByUserIdSortedRequest_Sort_By_Name_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetBudgetsByUserIdSortedRequestHandler(mockUnitOfWork, _mapper);
        string firstItemName = "A";
        string secondItemName = "B";

        var actualSortedBudgets = await handler.Handle(new GetBudgetsByUserIdSortedRequest
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            SortBy = "name"
        }, default);

        actualSortedBudgets.Should().NotBeNull();
        actualSortedBudgets[0].Name.Should().Be(firstItemName);
        actualSortedBudgets[1].Name.Should().Be(secondItemName);
    }

    [Fact]
    public async Task GetBudgetsByWalletIdRequest_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetBudgetsByWalletIdRequestHandler(mockUnitOfWork, _mapper);
        int expectedBudgetsCount = 4;
        var actualBudgets = (await handler.Handle(new GetBudgetsByWalletIdRequest
        {
            WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0")
        }, default)).ToList();

        actualBudgets.Should().NotBeNull();
        actualBudgets.Count.Should().Be(expectedBudgetsCount);
    }

    [Fact]
    public async Task GetBudgetsByWalletIdSortedRequest_Sort_By_Name_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetBudgetsByWalletIdSortedRequestHandler(mockUnitOfWork, _mapper);
        string firstItemName = "A";
        string secondItemName = "B";
        string thirdItemName = "Budget with user";

        var actualSortedBudgets = await handler.Handle(new GetBudgetsByWalletIdSortedRequest
        {
            WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
            SortBy = "name"
        }, default);

        actualSortedBudgets.Should().NotBeNull();
        actualSortedBudgets[0].Name.Should().Be(firstItemName);
        actualSortedBudgets[1].Name.Should().Be(secondItemName);
        actualSortedBudgets[2].Name.Should().Be(thirdItemName);
    }

    [Fact]
    public async Task GetBudgetWithWalletByIdRequest_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetBudgetWithWalletByIdRequestHandler(mockUnitOfWork, _mapper);
        var expectedBudget = _mapper.Map<BudgetWithWalletDTO>(new Budget
        {
            Balance = 3000,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
            CreatedBy = "ME",
            ModifiedBy = "ME",
            Currency = new Currency(),
            EndDate = new DateTime(2024, 10, 12),
            StartDate = new DateTime(2024, 10, 14),
            Name = "Budget3",
            Id = new Guid("3DA8AFC6-1671-4E6C-A4AC-6A048C388764"),
            Wallet = new()
            {
                Id = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
                Balance = 1000,
                Name = "Wallet 1",
            },
            User = new User
            {
                Id = new Guid("83F849FB-110A-44A4-8138-1404FF6556C7")
            },
            Categories = new List<Category>(),
            WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
            UserId = new Guid("83F849FB-110A-44A4-8138-1404FF6556C7"),
            CurrencyId = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61")
        });

        var actualBudget = await handler.Handle(new GetBudgetWithWalletByIdRequest
        {
            Id = new Guid("3DA8AFC6-1671-4E6C-A4AC-6A048C388764")
        }, default);


        actualBudget.Should().BeOfType<BudgetWithWalletDTO>();
        actualBudget.Should().BeEquivalentTo(expectedBudget);
    }
    
    [Fact]
    public async Task GetBudgetWithYserByIdRequest_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetBudgetWithUserByIdRequestHandler(mockUnitOfWork, _mapper);
        var expectedBudget = _mapper.Map<BudgetWithUserDTO>(new Budget
        {
            Balance = 3000,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
            CreatedBy = "ME",
            ModifiedBy = "ME",
            Currency = new Currency(),
            EndDate = new DateTime(2024, 10, 12),
            StartDate = new DateTime(2024, 10, 14),
            Name = "Budget with user",
            Id = new Guid("9055E428-38C3-4616-A389-0102B766FD98"),
            Wallet =  new(),
            User = new User
            {
                Id = new Guid("83F849FB-110A-44A4-8138-1404FF6556C7"),
                Email = "user@mail.com"
            },
            Categories = new List<Category>(),
            WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
            UserId = new Guid("83F849FB-110A-44A4-8138-1404FF6556C7"),
            CurrencyId = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61")
        });

        var actualBudget = await handler.Handle(new GetBudgetWithUserByIdRequest
        {
            Id = new Guid("9055E428-38C3-4616-A389-0102B766FD98")
        }, default);


        actualBudget.Should().BeOfType<BudgetWithUserDTO>();
        actualBudget.Should().BeEquivalentTo(expectedBudget);
        actualBudget.User.Email.Should().Be(expectedBudget.User.Email);
    }
}