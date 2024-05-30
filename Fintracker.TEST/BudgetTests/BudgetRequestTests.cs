using AutoFixture;
using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Budget.Handlers.Queries;
using Fintracker.Application.Features.Budget.Requests.Queries;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using Xunit.Abstractions;

namespace Fintracker.TEST.BudgetTests;

public class BudgetRequestTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IUnitOfWork _unitOfWork;

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
        _unitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
    }

    [Fact]
    public async Task Test_GetBudgetById_With_Valid_Id()
    {
        var handler = new GetBudgetByIdRequestHandler(_unitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetBudgetByIdRequest()
        {
            Id = new Guid("9C7EC483-ED14-4390-BBBA-A0753E55307F")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Id.Should().Be(new Guid("9C7EC483-ED14-4390-BBBA-A0753E55307F"));
    }

    [Fact]
    public async Task Test_GetBudgetById_With_Invalid_Id_Should_Throw_NotFound()
    {
        var handler = new GetBudgetByIdRequestHandler(_unitOfWork, _mapper);


        // Assert
        Func<Task> act = async () => await handler.Handle(new GetBudgetByIdRequest()
        {
            Id = new Guid("588AF370-1599-434A-A13C-ED2AEEBEEC9C")
        }, default);

        await act.Should().ThrowAsync<NotFoundException>(); // FluentAssertions
    }

    [Fact]
    public async Task Test_GetBudgetsByUserId_With_Valid_UserId_Should_Return_Count_2()
    {
        var handler = new GetBudgetsByUserIdRequestHandler(_unitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetBudgetsByUserIdRequest()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(2);
    }

    [Fact]
    public async Task Test_GetBudgetsByUserId_With_Invalid_UserId_Should_Return_Count_0()
    {
        var handler = new GetBudgetsByUserIdRequestHandler(_unitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetBudgetsByUserIdRequest()
        {
            UserId = new Guid("3101CB56-A349-4A56-966D-8D295A078215")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(0);
    }

    [Fact]
    public async Task Test_GetBudgetsByUserIdSorted_With_Valid_UserId_Should_Return_Count_2()
    {
        var handler = new GetBudgetsByUserIdSortedRequestHandler(_unitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetBudgetsByUserIdSortedRequest()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Params = new()
            {
                SortBy = "name"
            }
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(2);
        actualResult.Should().BeInAscendingOrder(x => x.Name);
    }

    [Fact]
    public async Task Test_GetBudgetsByUserSortedId_With_Invalid_Sort_Param_Should_Throw_BadRequest()
    {
        var handler = new GetBudgetsByUserIdSortedRequestHandler(_unitOfWork, _mapper);


        Func<Task> act = async () => await handler.Handle(new GetBudgetsByUserIdSortedRequest()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Params = new()
            {
                SortBy = "non-existing-sort-param"
            }
        }, default);

        await act.Should().ThrowAsync<BadRequestException>(); // FluentAssertions
    }
    
    [Fact]
    public async Task Test_GetBudgetsByWalletId_With_Valid_WalletId_Should_Return_Count_2()
    {
        var handler = new GetBudgetsByWalletIdRequestHandler(_unitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetBudgetsByWalletIdRequest()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(2);
    }

    [Fact]
    public async Task Test_GetBudgetsByWalletId_With_Invalid_WalletId_Should_Return_Count_0()
    {
        var handler = new GetBudgetsByWalletIdRequestHandler(_unitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetBudgetsByWalletIdRequest()
        {
            UserId = new Guid("3101CB56-A349-4A56-966D-8D295A078215"),
            WalletId = new Guid("0E6ABEFD-B5BD-4C69-BCEE-98524E593709")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task Test_GetBudgetsByWalletIdSorted_With_Valid_WalletId_Should_Return_Count_2()
    {
        var handler = new GetBudgetsByWalletIdSortedRequestHandler(_unitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetBudgetsByWalletIdSortedRequest()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
            Params = new()
            {
                SortBy = "name"
            }
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(2);
        actualResult.Should().BeInAscendingOrder(x => x.Name);
    }

    [Fact]
    public async Task Test_GetBudgetsByWalletSortedId_With_Invalid_Sort_Param_Should_Throw_BadRequest()
    {
        var handler = new GetBudgetsByWalletIdSortedRequestHandler(_unitOfWork, _mapper);


        Func<Task> act = async () => await handler.Handle(new GetBudgetsByWalletIdSortedRequest()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
            Params = new()
            {
                SortBy = "non-existing-sort-param"
            }
        }, default);

        await act.Should().ThrowAsync<BadRequestException>(); // FluentAssertions
    }
}