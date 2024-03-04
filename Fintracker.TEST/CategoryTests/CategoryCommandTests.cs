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


        var result = await handler.Handle(new CreateCategoryCommand()
        {
            Category = categoryToAdd
        }, default);

        int categoriesCount = (await mockUnitOfWork.CategoryRepository.GetAllAsync()).Count;
        result.Success.Should().BeTrue();
        categoriesCount.Should().Be(5);
    }


    [Fact]
    public async Task UpdateCategory_Should_Return_True_And_Old_And_New_Objects()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new UpdateCategoryCommandHandler(mockUnitOfWork, _mapper);
        var categoryToUpdate = _fixture.Build<UpdateCategoryDTO>()
            .With(x => x.Name, "This is new category")
            .With(x => x.Image, "New Icon")
            .With(x => x.Id, new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6"))
            .With(x => x.IconColour, "red")
            .Create();


        var result = await handler.Handle(new UpdateCategoryCommand()
        {
            Category = categoryToUpdate
        }, default);

        var updatedCategory =
            await mockUnitOfWork.CategoryRepository.GetAsync(new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6"));
        
        result.Success.Should().BeTrue();
        updatedCategory!.Name.Should().Be(categoryToUpdate.Name);
        updatedCategory.Image.Should().Be(categoryToUpdate.Image);

        result.Old.Should().NotBeNull();
        result.New.Should().NotBeNull();
        result.Old.Image.Should().Be("Glory");
        result.New!.Image.Should().Be("New Icon");
    }
    
    [Fact]
    public async Task DeleteAsync_Should_Return_True_And_Deleted_Object()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new DeleteCategoryCommandHandler(mockUnitOfWork, _mapper);
        
        var result = await handler.Handle(new DeleteCategoryCommand()
        {
            Id = new Guid("77326B96-DF2B-4CC8-93A3-D11A276433D6")
        }, default);

        var categoriesCountAfterDelete = (await mockUnitOfWork.CategoryRepository.GetAllAsync()).Count;
        
        result.Success.Should().BeTrue();
        result.DeletedObj.Should().NotBeNull();
        categoriesCountAfterDelete.Should().Be(3);
    }
}

