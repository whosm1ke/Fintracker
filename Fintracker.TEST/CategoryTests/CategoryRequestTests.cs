using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Category.Handlers.Queries;
using Fintracker.Application.Features.Category.Requests.Queries;
using Fintracker.Application.MapProfiles;
using Fintracker.Domain.Enums;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using Xunit.Abstractions;

namespace Fintracker.TEST.CategoryTests;

public class CategoryRequestTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;

    public CategoryRequestTests(ITestOutputHelper output)
    {
        _output = output;
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c => { c.AddProfile<CategoryProfile>(); });

        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    public async Task Test_GetCategoriesByType_With_Invalid_Params_Should_Return_Count_0()
    {
       
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoriesByTypeRequestHandler(mockUnitOfWork, _mapper);
       

        var actualResult = await handler.Handle(new GetCategoriesByTypeRequest
        {
            Type = CategoryType.INCOME,
            UserId = new Guid("6F9661D1-B736-49FF-B753-71295BCD9352")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task Test_GetCategoriesByType_With_Valid_Params_Should_Return_Count_2()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoriesByTypeRequestHandler(mockUnitOfWork, _mapper);
       

        var actualResult = await handler.Handle(new GetCategoriesByTypeRequest
        {
            Type = CategoryType.INCOME,
            UserId = new Guid("EDE38841-5183-4BDD-A148-D1923F170B1A")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(1);
    }

    [Fact]
    public async Task Test_GetCategoriesSorted_With_Valid_Params_Should_Return_Count_4()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoriesSortedRequestHandler(mockUnitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetCategoriesSortedRequest
        {
            Params = new()
            {
                SortBy = "name"
            },
            UserId = new Guid("EDE38841-5183-4BDD-A148-D1923F170B1A")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(4);
        actualResult.Should().BeInAscendingOrder(x => x.Name);
    }
    
    [Fact]
    public async Task Test_GetCategoriesSorted_With_Invalid_Params_Should_Throw_BadRequest()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoriesSortedRequestHandler(mockUnitOfWork, _mapper);


        Func<Task> act = async () => await handler.Handle(new GetCategoriesSortedRequest
        {
            Params = new()
            {
                SortBy = "non-existing-name"
            },
            UserId = new Guid("EDE38841-5183-4BDD-A148-D1923F170B1A")
        }, default);

        await act.Should().ThrowAsync<BadRequestException>(); // FluentAssertions
    }

    [Fact]
    public async Task Test_GetCategoryById_With_Valid_Params_Should_Return_Category_1()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoryByIdRequestHandler(mockUnitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetCategoryByIdRequest
        {
            Id = new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6"),
            UserId = new Guid("EDE38841-5183-4BDD-A148-D1923F170B1A")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Id.Should().Be( new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6"));
        actualResult.Name.Should().Be("Category 1");
        actualResult.Type.Should().Be(CategoryTypeEnum.INCOME);
    }
    
    [Fact]
    public async Task Test_GetCategoryById_With_Invalid_Params_Should_Throw_NotFound()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoryByIdRequestHandler(mockUnitOfWork, _mapper);
        
        Func<Task> act = async () => await handler.Handle(new GetCategoryByIdRequest
        {
            Id = new Guid("28CCBE49-A9AD-4306-B82F-241C8C464C43"),
            UserId = new Guid("D9E84A33-9047-4F85-9D5B-CA4EC37C837B")
        }, default);

        await act.Should().ThrowAsync<NotFoundException>(); // FluentAssertions
    }

    [Fact]
    public async Task Test_GetCategories_With_Valid_User_Id_Should_Return_Count_4()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoriesRequestHandler(mockUnitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetCategoriesRequest
        {
            UserId = new Guid("EDE38841-5183-4BDD-A148-D1923F170B1A")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(3);
    }
    
    [Fact]
    public async Task Test_GetCategories_With_Invalid_User_Id_Should_Return_Count_0()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoriesRequestHandler(mockUnitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetCategoriesRequest
        {
            UserId = new Guid("998F5947-C1DA-4B60-AF19-FA8B8FF85013")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(0);
    }
}