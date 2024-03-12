using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Category;
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
    public async Task GetByType_Type_Income_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoriesByTypeRequestHandler(mockUnitOfWork, _mapper);
        var expectedResult = new List<CategoryDTO>
        {
            new()
            {
                Id = new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6"),
                Type = CategoryTypeEnum.INCOME,
                Name = "Category 1",
                Image = "Glory",
                IconColour = "pink"
            },
            new()
            {
                Id = new Guid("D8B7FB81-F6D9-49F0-A1C8-3B43B7D39F7C"),
                Type = CategoryTypeEnum.INCOME,
                Name = "Category 3",
                Image = "Image 1",
                IconColour = "yellow"
            }
        };

        var actualResult = await handler.Handle(new GetCategoriesByTypeRequest
        {
            Type = CategoryType.INCOME
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetAllSorted_Sort_By_Name_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoriesSortedRequestHandler(mockUnitOfWork, _mapper);
        var expectedResult = new List<CategoryDTO>
        {
            new()
            {
                Id = new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6"),
                Type = CategoryTypeEnum.INCOME,
                Name = "Category 1",
                Image = "Glory",
                IconColour = "pink"
            },
            new()
            {
                Id = new Guid("D670263B-92CF-48C8-923A-EB09188F6077"),
                Type = CategoryTypeEnum.EXPENSE,
                Name = "Category 2",
                Image = "frog",
                IconColour = "green"
            },
            new()
            {
                Id = new Guid("D8B7FB81-F6D9-49F0-A1C8-3B43B7D39F7C"),
                Type = CategoryTypeEnum.INCOME,
                Name = "Category 3",
                Image = "Image 1",
                IconColour = "yellow"
            },
            new()
            {
                Id = new Guid("F0872017-AE98-427E-B976-B46AC2004D15"),
                Type = CategoryTypeEnum.EXPENSE,
                Name = "Category 4",
                Image = "log",
                IconColour = "cyan"
            }
        };

        var actualResult = await handler.Handle(new GetCategoriesSortedRequest
        {
            SortBy = "Name"
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(expectedResult.Count);
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetCategoryById_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoryByIdRequestHandler(mockUnitOfWork, _mapper);
        var expectedResult = new CategoryDTO
        {
            Id = new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6"),
            Type = CategoryTypeEnum.INCOME,
            Name = "Category 1",
            Image = "Glory",
            IconColour = "pink"
        };

        var actualResult = await handler.Handle(new GetCategoryByIdRequest
        {
            Id = new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetAllCategories_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCategoriesRequestHandler(mockUnitOfWork, _mapper);
        var expectedResult = new List<CategoryDTO>
        {
            new()
            {
                Id = new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6"),
                Type = CategoryTypeEnum.INCOME,
                Name = "Category 1",
                Image = "Glory",
                IconColour = "pink"
            },
            new()
            {
                Id = new Guid("D670263B-92CF-48C8-923A-EB09188F6077"),
                Type = CategoryTypeEnum.EXPENSE,
                Name = "Category 2",
                Image = "frog",
                IconColour = "green"
            },
            new()
            {
                Id = new Guid("D8B7FB81-F6D9-49F0-A1C8-3B43B7D39F7C"),
                Type = CategoryTypeEnum.INCOME,
                Name = "Category 3",
                Image = "Image 1",
                IconColour = "yellow"
            },
            new()
            {
                Id = new Guid("F0872017-AE98-427E-B976-B46AC2004D15"),
                Type = CategoryTypeEnum.EXPENSE,
                Name = "Category 4",
                Image = "log",
                IconColour = "cyan"
            }
        };

        var actualResult = await handler.Handle(new GetCategoriesRequest(), default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(expectedResult.Count);
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}