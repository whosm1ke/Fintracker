using AutoFixture;
using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Budget.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Budget.Handlers.Commands;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Fintracker.TEST.BudgetTests;

public class BudgetCommandTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateBudgetDtoValidator _createBudgetDtoValidator;
    private readonly UpdateBudgetDtoValidator _updateBudgetDtoValidator;

    public BudgetCommandTests()
    {
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<BudgetProfile>();
            c.AddProfile<CurrencyProfile>();
            c.AddProfile<CategoryProfile>();
            c.AddProfile<UserProfile>();
            c.AddProfile<WalletProfile>();
        });

        _mapper = mapperCfg.CreateMapper();
        _unitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        _userRepository = MockUserRepository.GetUserRepository().Object;

        _createBudgetDtoValidator = new CreateBudgetDtoValidator(_unitOfWork, _userRepository);
        _updateBudgetDtoValidator = new UpdateBudgetDtoValidator(_unitOfWork);
    }

    [Fact]
    private async Task Test_AddBudget_With_Valid_Params_Should_Return_True()
    {
        var handler = new CreateBudgetCommandHandler(_unitOfWork, _mapper, MockCurrencyConverter.GetCurrencyConverter().Object);
        var budgetToAdd = _fixture.Build<CreateBudgetDTO>()
            .With(x => x.Name, "New Budget")
            .With(x => x.StartBalance, 1000)
            .With(x => x.CurrencyId, new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"))
            .With(x => x.WalletId, new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"))
            .With(x => x.CategoryIds, new List<Guid>()
            {
                new Guid("D670263B-92CF-48C8-923A-EB09188F6077"),
                new Guid("F0872017-AE98-427E-B976-B46AC2004D15")
            })
            .With(x => x.StartDate, new DateTime(2024,1,1))
            .With(x => x.EndDate, new DateTime(2024,1,12))
            .With(x => x.IsPublic, true)
            .With(x => x.OwnerId, new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .Create();

        var command = new CreateBudgetCommand()
        {
            Budget = budgetToAdd
        };


        var validationResult =
            await _createBudgetDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldNotHaveAnyValidationErrors();

        var result = await handler.Handle(command, default);

        int budgetsCount = (await _unitOfWork.BudgetRepository.GetAllAsync()).Count;
        result.Success.Should().BeTrue();
        budgetsCount.Should().Be(3);
    }
    
    [Fact]
    private async Task Test_AddBudget_With_Invalid_Params_Should_Throw_Validation_Errors()
    {
        var budgetToAdd = _fixture.Build<CreateBudgetDTO>()
            .With(x => x.Name, "Extremely loooooooooooooooooooooooooooooooooooooong name for budget")
            .With(x => x.StartBalance, 100_000_000_000_000_0)
            .With(x => x.CurrencyId, new Guid("C25D02C2-DB44-42B6-8961-58170D4F920F"))
            .With(x => x.WalletId, new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"))
            .With(x => x.CategoryIds, new List<Guid>()
            {
                new Guid("D670263B-92CF-48C8-923A-EB09188F6077"),
                new Guid("F0872017-AE98-427E-B976-B46AC2004D15")
            })
            .With(x => x.StartDate, new DateTime(2024,1,1))
            .With(x => x.EndDate, new DateTime(2024,1,12))
            .With(x => x.IsPublic, true)
            .With(x => x.OwnerId, new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .Create();

        var command = new CreateBudgetCommand()
        {
            Budget = budgetToAdd
        };
        
        var validationResult =
            await _createBudgetDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldHaveAnyValidationError();

    }
    
    [Fact]
    private async Task Test_UpdateBudget_With_Valid_Params_Should_Return_True()
    {
        var handler = new UpdateBudgetCommandHandler(_unitOfWork, _mapper, MockCurrencyConverter.GetCurrencyConverter().Object);
        var budgetToUpdate = _fixture.Build<UpdateBudgetDTO>()
            .With(x => x.Id, new Guid("29DECA11-E633-47E4-A0B2-569791B7D8C7"))
            .With(x => x.Name, "Updated Budget")
            .With(x => x.StartBalance, 2000)
            .With(x => x.CurrencyId, new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"))
            .With(x => x.WalletId, new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"))
            .With(x => x.CategoryIds, new List<Guid>()
            {
                new Guid("D670263B-92CF-48C8-923A-EB09188F6077"),
                new Guid("F0872017-AE98-427E-B976-B46AC2004D15")
            })
            .With(x => x.StartDate, new DateTime(2024,1,1))
            .With(x => x.EndDate, new DateTime(2025,1,12))
            .With(x => x.IsPublic, false)
            .Create();

        var command = new UpdateBudgetCommand()
        {
            Budget = budgetToUpdate
        };


        var validationResult =
            await _updateBudgetDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldNotHaveAnyValidationErrors();

        var result = await handler.Handle(command, default);

        result.Success.Should().BeTrue();
        result.New.Name.Should().Be(budgetToUpdate.Name);
        result.New.StartBalance.Should().Be(budgetToUpdate.StartBalance);
        result.New.EndDate.Should().Be(budgetToUpdate.EndDate);
        result.New.IsPublic.Should().Be(budgetToUpdate.IsPublic);
    }
    
    [Fact]
    private async Task Test_UpdateBudget_With_Invalid_Params_Should_Throw_Validation_Errors()
    {
        var budgetToUpdate = _fixture.Build<UpdateBudgetDTO>()
            .With(x => x.Id, new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .With(x => x.Name, "Extremely loooooooooooooooooooooooooooooooooooooong name for budget")
            .With(x => x.StartBalance, 100_000_000_000_000_0)
            .With(x => x.CurrencyId, new Guid("C25D02C2-DB44-42B6-8961-58170D4F920F"))
            .With(x => x.WalletId, new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"))
            .With(x => x.CategoryIds, new List<Guid>()
            {
                new Guid("D670263B-92CF-48C8-923A-EB09188F6077"),
                new Guid("F0872017-AE98-427E-B976-B46AC2004D15")
            })
            .With(x => x.StartDate, new DateTime(2024,1,1))
            .With(x => x.EndDate, new DateTime(2024,1,12))
            .With(x => x.IsPublic, true)
            .Create();

        var command = new UpdateBudgetCommand()
        {
            Budget = budgetToUpdate
        };
        
        var validationResult =
            await _updateBudgetDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldHaveAnyValidationError();

    }
    
    [Fact]
    private async Task Test_DeleteBudget_With_Valid_Params_Should_Return_True()
    {
        var handler = new DeleteBudgetCommandHandler(_unitOfWork, _mapper);

        var command = new DeleteBudgetCommand()
        {
            Id = new Guid("9C7EC483-ED14-4390-BBBA-A0753E55307F"),
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        };



        var result = await handler.Handle(command, default);

        result.Success.Should().BeTrue();
    }
    
    [Fact]
    private async Task Test_DeleteBudget_With_Invalid_Params_Should_Throw_Validation_Errors()
    {

        var handler = new DeleteBudgetCommandHandler(_unitOfWork, _mapper);

        var command = new DeleteBudgetCommand()
        {
            Id = new Guid("1FF9CF5C-9D48-4122-B206-4E8B7ECC547E"),
            UserId = new Guid("D1170B32-22B8-4BDC-ADAB-1552D118CE84")
        };

        // Assert
        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<NotFoundException>(); // FluentAssertions

    }
}