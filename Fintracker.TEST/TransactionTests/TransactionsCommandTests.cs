using AutoFixture;
using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.DTO.Transaction.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Handlers.Commands;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit.Abstractions;

namespace Fintracker.TEST.TransactionTests;

public class TransactionsCommandTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly CreateTransactionDtoValidator _createTransactionDtoValidator;
    private readonly UpdateTransactionDtoValidator _updateTransactionDtoValidator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public TransactionsCommandTests(ITestOutputHelper output)
    {
        _output = output;
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<TransactionProfile>();
            c.AddProfile<WalletProfile>();
            c.AddProfile<CurrencyProfile>();
            c.AddProfile<UserProfile>();
            c.AddProfile<CategoryProfile>();
        });

        _mapper = mapperCfg.CreateMapper();

        _unitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        _userRepository = MockUserRepository.GetUserRepository().Object;

        _createTransactionDtoValidator = new CreateTransactionDtoValidator(_unitOfWork, _userRepository);
        _updateTransactionDtoValidator = new UpdateTransactionDtoValidator(_unitOfWork);
    }

    [Fact]
    public async Task AddTransaction_With_Valid_Params_Should_Return_True()
    {
        var handler = new CreateTransactionCommandHandler(_mapper, _unitOfWork,
            MockCurrencyConverter.GetCurrencyConverter().Object);
        var wallet = await _unitOfWork.WalletRepository.GetWalletById(new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"));
        var initialBalance = wallet.Balance;
        var initialTotalSpent = wallet.TotalSpent;

        var budget = await _unitOfWork.BudgetRepository.GetBudgetByIdAsync(new Guid("9C7EC483-ED14-4390-BBBA-A0753E55307F"));
        var initialBudgetBalance = budget.Balance;
        var initialBudgetTotalSpent = budget.TotalSpent;
        
        var transactionToAdd = _fixture.Build<CreateTransactionDTO>()
            .With(x => x.UserId, new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .With(x => x.Label, "Label")
            .With(x => x.Note, "Note")
            .With(x => x.Amount, 100)
            .With(x => x.WalletId, new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"))
            .With(x => x.CurrencyId, new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"))
            .With(x => x.CategoryId, new Guid("F0872017-AE98-427E-B976-B46AC2004D15"))
            .With(x => x.Date, new DateTime(2024,3,3))
            .Create();

        var command = new CreateTransactionCommand()
        {
            Transaction = transactionToAdd
        };

        var validationResult =
            await _createTransactionDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldNotHaveAnyValidationErrors();
        var result = await handler.Handle(command, default);

        wallet = await _unitOfWork.WalletRepository.GetWalletById(new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"));
        var afterBalance = wallet.Balance;
        var afterTotalSpent = wallet.TotalSpent;
        
        budget = await _unitOfWork.BudgetRepository.GetBudgetByIdAsync(new Guid("9C7EC483-ED14-4390-BBBA-A0753E55307F"));
        var afterBudgetBalance = budget.Balance;
        var afterBudgetTotalSpent = budget.TotalSpent;
        
        result.Success.Should().BeTrue();
        afterBalance.Should().Be(initialBalance - 100);
        afterTotalSpent.Should().Be(initialTotalSpent + 100);
        
        afterBudgetBalance.Should().Be(initialBudgetBalance - 100);
        afterBudgetTotalSpent.Should().Be(initialBudgetTotalSpent + 100);

        var transactionsCount = await _unitOfWork.TransactionRepository.GetAllAsync();
        transactionsCount.Count.Should().Be(7);
    }
    
    [Fact]
    public async Task AddTransaction_With_Invalid_Params_Should_Not_Pass_Validation()
    {
        
        var transactionToAdd = _fixture.Build<CreateTransactionDTO>()
            .With(x => x.UserId, new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .With(x => x.Label, "Label")
            .With(x => x.Note, "Note")
            .With(x => x.Amount, 100)
            // .With(x => x.WalletId, new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"))
            // .With(x => x.CurrencyId, new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"))
            .With(x => x.CategoryId, new Guid("F0872017-AE98-427E-B976-B46AC2004D15"))
            .With(x => x.Date, new DateTime(2024,3,3))
            .Create();

        var command = new CreateTransactionCommand()
        {
            Transaction = transactionToAdd
        };

        var validationResult =
            await _createTransactionDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldHaveAnyValidationError();
      
    }
    
    [Fact]
    public async Task UpdateTransaction_With_Valid_Params_Should_Return_True()
    {
        var handler = new UpdateTransactionCommandHandler(_mapper, _unitOfWork,
            MockCurrencyConverter.GetCurrencyConverter().Object);
        

        
        var transactionToUpdate = _fixture.Build<UpdateTransactionDTO>()
            .With(x => x.Id, new Guid("3C48E189-C154-4E74-B8C3-7F0CACD52FDB"))
            .With(x => x.Amount, 200)
            .With(x => x.CurrencyId, new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"))
            .With(x => x.CategoryId, new Guid("F0872017-AE98-427E-B976-B46AC2004D15"))
            .With(x => x.Date, new DateTime(2024,3,3))
            .Without(x => x.Label)
            .Without(x => x.Note)
            .Create();

        var command = new UpdateTransactionCommand()
        {
            Transaction = transactionToUpdate
        };

        var validationResult =
            await _updateTransactionDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldNotHaveAnyValidationErrors();
        var result = await handler.Handle(command, default);
        
        result.Success.Should().BeTrue();
        result.New.Amount.Should().Be(200);
    }
    
    [Fact]
    public async Task UpdateTransaction_With_Invalid_Params_Should_Not_Pass_Validation()
    {
        
        var transactionToUpdate = _fixture.Build<UpdateTransactionDTO>()
            .With(x => x.Id, new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .With(x => x.Label, "Label")
            .With(x => x.Note, "Note")
            .With(x => x.Amount, 100)
            .With(x => x.CategoryId, new Guid("F0872017-AE98-427E-B976-B46AC2004D15"))
            .With(x => x.Date, new DateTime(2024,3,3))
            .Create();

        var command = new UpdateTransactionCommand()
        {
            Transaction = transactionToUpdate
        };

        var validationResult =
            await _updateTransactionDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldHaveAnyValidationError();
      
    }
    
    
     [Fact]
    public async Task DeleteTransaction_With_Valid_Params_Should_Return_True()
    {
        var handler = new DeleteTransactionCommandHandler(_mapper, _unitOfWork,
            MockCurrencyConverter.GetCurrencyConverter().Object);
        

        var command = new DeleteTransactionCommand()
        {
            Id = new Guid("3C48E189-C154-4E74-B8C3-7F0CACD52FDB"),
            UserId =  new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        };


        var result = await handler.Handle(command, default);
        
        result.Success.Should().BeTrue();
        result.DeletedObj.Should().NotBeNull();
        
        var transactionsCount = await _unitOfWork.TransactionRepository.GetAllAsync();
        transactionsCount.Count.Should().Be(5);
    }
    
    [Fact]
    public async Task DeleteTransaction_With_Invalid_Params_Should_Throw_NotFound()
    {
        var handler = new DeleteTransactionCommandHandler(_mapper, _unitOfWork, MockCurrencyConverter.GetCurrencyConverter().Object);

        var command = new DeleteTransactionCommand()
        {
            Id = new Guid("B34063CA-A5D8-47C5-8633-AFC870F8846F"),
            UserId = new Guid("B35CF5BD-A778-470F-872B-80D2A689FA85")
        };

        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<NotFoundException>(); // FluentAssertions
      
    }
}