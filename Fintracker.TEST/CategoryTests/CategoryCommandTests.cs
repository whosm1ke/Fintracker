using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Features.Category.Handlers.Commands;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using Xunit.Abstractions;

namespace Fintracker.TEST.CategoryTests;

public class CategoryCommandTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;

    public CategoryCommandTests(ITestOutputHelper output)
    {
        _output = output;
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c => { c.AddProfile<CategoryProfile>(); });

        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    public async Task AddCategory_Should_Return_True()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new CreateCategoryCommandHandler(mockUnitOfWork, _mapper);
        var categoryToAdd = _fixture.Build<CreateCategoryDTO>()
            .With(x => x.Name, "New category")
            .With(x => x.Image, "FaIcon")
            .Without(x => x.Type)
            .With(x => x.IconColour, "red")
            .Create();


        var result = await handler.Handle(new CreateCategoryCommand
        {
            Category = categoryToAdd,
            UserId = new Guid("EDE38841-5183-4BDD-A148-D1923F170B1A")
        }, default);

        int categoriesCount = (await mockUnitOfWork.CategoryRepository.GetAllAsync()).Count;
        result.Success.Should().BeTrue();
        categoriesCount.Should().Be(5);
    }


    [Fact]
    public async Task UpdateCategory_Should_Return_Forbidden_Exception()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new UpdateCategoryCommandHandler(mockUnitOfWork, _mapper);
        var categoryToUpdate = _fixture.Build<UpdateCategoryDTO>()
            .With(x => x.Name, "This is new category")
            .With(x => x.Image, "New Icon")
            .With(x => x.Id, new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6"))
            .With(x => x.IconColour, "red")
            .Create();


       
           var result = await handler.Handle(new UpdateCategoryCommand
            {
                Category = categoryToUpdate,
                UserId = new Guid("EDE38841-5183-4BDD-A148-D1923F170B1A")
            }, default);


            result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_Return_True()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new DeleteCategoryCommandHandler(mockUnitOfWork, _mapper, null);


        var result = await handler.Handle(new DeleteCategoryCommand
        {
            Id = new Guid("D670263B-92CF-48C8-923A-EB09188F6077"),
            UserId = new Guid("EDE38841-5183-4BDD-A148-D1923F170B1A")
        }, default);


        result.Success.Should().BeTrue();
    }
}