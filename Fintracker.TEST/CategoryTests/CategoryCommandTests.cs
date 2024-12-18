using AutoFixture;
using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Category.Validators;
using Fintracker.Application.Features.Category.Handlers.Commands;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using FluentValidation.TestHelper;
using MediatR;
using Xunit.Abstractions;

namespace Fintracker.TEST.CategoryTests;

public class CategoryCommandTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly CreateCategoryDtoValidator _createCategoryDtoValidator;
    private readonly UpdateCategoryDtoValidator _updateCategoryDtoValidator;
    private readonly DeleteCategoryDtoValidator _deleteCategoryDtoValidator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CategoryCommandTests(ITestOutputHelper output)
    {
        _output = output;
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c => { c.AddProfile<CategoryProfile>(); });

        _mapper = mapperCfg.CreateMapper();

        _unitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        _userRepository = MockUserRepository.GetUserRepository().Object;

        _updateCategoryDtoValidator = new UpdateCategoryDtoValidator(_unitOfWork, _userRepository);
        _createCategoryDtoValidator = new CreateCategoryDtoValidator(_userRepository);
        _deleteCategoryDtoValidator = new DeleteCategoryDtoValidator(_unitOfWork, _userRepository);
    }

    [Fact]
    public async Task Test_AddCategory_With_Valid_Params_Should_Return_True()
    {
        var handler = new CreateCategoryCommandHandler(_unitOfWork, _mapper);
        var categoryToAdd = _fixture.Build<CreateCategoryDTO>()
            .With(x => x.Name, "New category")
            .With(x => x.Image, "FaIcon")
            .Without(x => x.Type)
            .With(x => x.IconColour, "red")
            .Create();


        var command = new CreateCategoryCommand
        {
            Category = categoryToAdd,
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        };

        var validationResult =
            await _createCategoryDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldNotHaveAnyValidationErrors();

        var result = await handler.Handle(command, default);

        int categoriesCount = (await _unitOfWork.CategoryRepository.GetAllAsync()).Count;
        result.Success.Should().BeTrue();
        categoriesCount.Should().Be(5);
    }

    [Fact]
    public async Task Test_AddCategory_With_Invalid_Params_Should_Throw_Validation_Error()
    {
        var categoryToAdd = _fixture.Build<CreateCategoryDTO>()
            .With(x => x.Name, "Extremely super long name for simple category without any sense")
            .With(x => x.Image, "FaIcon")
            .Without(x => x.Type)
            .With(x => x.IconColour, "red")
            .Create();


        var command = new CreateCategoryCommand
        {
            Category = categoryToAdd,
            UserId = new Guid("B0BE44D3-3D1F-4079-BE2B-E55030396FA2")
        };

        var validationResult =
            await _createCategoryDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldHaveAnyValidationError();
    }


    [Fact]
    public async Task Test_UpdateCategory_With_Valid_Params_Should_Return_True()
    {
        var handler = new UpdateCategoryCommandHandler(_unitOfWork, _mapper);
        var categoryToUpdate = _fixture.Build<UpdateCategoryDTO>()
            .With(x => x.Name, "This is updated category")
            .With(x => x.Image, "New Icon")
            .With(x => x.Id, new Guid("D8B7FB81-F6D9-49F0-A1C8-3B43B7D39F7C"))
            .With(x => x.IconColour, "red")
            .Create();


        var command = new UpdateCategoryCommand
        {
            Category = categoryToUpdate,
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        };

        var validationResult =
            await _updateCategoryDtoValidator.TestValidateAsync(command, s => s.IncludeAllRuleSets());
        validationResult.ShouldNotHaveAnyValidationErrors();


        var result = await handler.Handle(command, default);


        result.Success.Should().BeTrue();
        result.New.Name.Should().Be(categoryToUpdate.Name);
    }
    
    [Fact]
    public async Task Test_UpdateCategory_With_Invalid_Params_Should_Throw_Validation_Errors()
    {
        var categoryToUpdate = _fixture.Build<UpdateCategoryDTO>()
            .With(x => x.Name, "This is updated category")
            .With(x => x.Image, "New Icon")
            .With(x => x.Id, new Guid("FDCD3EDF-9C7C-4B45-A4FF-955263742ABF"))
            .With(x => x.IconColour, "redredredredredredredredredredredredredredredredred")
            .Create();


        var command = new UpdateCategoryCommand
        {
            Category = categoryToUpdate,
            UserId = new Guid("9F8E612F-42EA-4D1D-B2D8-A63BA5D657FD")
        };

        var validationResult =
            await _updateCategoryDtoValidator.TestValidateAsync(command, s => s.IncludeAllRuleSets());
        validationResult.ShouldHaveAnyValidationError();
    }
    
    [Fact]
    public async Task Test_DeleteCategory_With_Valid_Params_Should_Return_True()
    {
        var handler = new DeleteCategoryCommandHandler(_unitOfWork, _mapper, MockCurrencyConverter.GetCurrencyConverter().Object);


        var command = new DeleteCategoryCommand
        {
            Id = new Guid("D8B7FB81-F6D9-49F0-A1C8-3B43B7D39F7C"),
            CategoryToReplaceId = null,
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        };

        var validationResult =
            await _deleteCategoryDtoValidator.TestValidateAsync(command, s => s.IncludeAllRuleSets());
        validationResult.ShouldNotHaveAnyValidationErrors();


        var result = await handler.Handle(command, default);


        result.Success.Should().BeTrue();
        
        int categoriesCount = (await _unitOfWork.CategoryRepository.GetAllAsync()).Count;
        result.Success.Should().BeTrue();
        categoriesCount.Should().Be(3);
    }
    
    [Fact]
    public async Task Test_DeleteCategory_With_Invalid_Params_Should_Throw_Validation_Errors()
    {

        var command = new DeleteCategoryCommand
        {
            CategoryToReplaceId = null,
            Id = new Guid("AC757A03-19D0-4884-91AC-1C8E62A85C3E"),
            UserId = new Guid("9F8E612F-42EA-4D1D-B2D8-A63BA5D657FD")
        };

        var validationResult =
            await _deleteCategoryDtoValidator.TestValidateAsync(command, s => s.IncludeAllRuleSets());
        validationResult.ShouldHaveAnyValidationError();
    }

    [Fact]
    private async Task Test_PopulateUserWithCategories_With_Valid_Params_Should_Return_True()
    {
        var handler = new PopulateUserWithCategoriesCommandHandler(_unitOfWork, _mapper);
        var command = new PopulateUserWithCategoriesCommand()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            PathToFile = "C:\\Users\\cherm\\OneDrive\\Рабочий стол\\Fintracker_Project\\Fintracker\\Fintracker.API\\wwwroot\\data\\categories.json"
        };


        var result = await handler.Handle(command, default);

        result.Should().BeEquivalentTo(Unit.Value);
        var allCategories =
            await _unitOfWork.CategoryRepository.GetAllAsync(new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"));

        allCategories.Count.Should().Be(28);
    }
}