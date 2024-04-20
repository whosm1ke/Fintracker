﻿using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Features.Transaction.Handlers.Commands;
using Fintracker.Application.Features.Transaction.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using Xunit.Abstractions;

namespace Fintracker.TEST.TransactionTests;

public class TransactionsCommandTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;

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
    }
    
      [Fact]
    public async Task AddAsync_No_Optional_Params_Should_Return_True()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var mockUserRepository = MockUserRepository.GetUserRepository().Object;
        var handler = new CreateTransactionCommandHandler(_mapper, mockUnitOfWork, mockUserRepository);
        var transactionToAdd = _fixture.Build<CreateTransactionDTO>()
            .With(x => x.Amount, 500)
            .With(x => x.CategoryId, new Guid("D670263B-92CF-48C8-923A-EB09188F6077"))
            .With(x => x.UserId,new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .With(x => x.WalletId,new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"))
            .With(x => x.CurrencyId,new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"))
            .Without(x => x.Label)
            .Without(x => x.Note)
            .Create();

        var result = await handler.Handle(new CreateTransactionCommand
        {
            Transaction = transactionToAdd
        }, default);
        
        var transactionsCount = (await mockUnitOfWork.TransactionRepository.GetAllAsync()).Count;
        var walletBalance = (await mockUnitOfWork.WalletRepository.GetAsync(transactionToAdd.WalletId))!.Balance;

        walletBalance.Should().Be(500);
        result.Success.Should().BeTrue();
        result.CreatedObject.Should().NotBeNull();
        result.CreatedObject.Should().BeOfType<TransactionBaseDTO>();
        transactionsCount.Should().Be(10);
    }
    
    [Fact]
    public async Task AddAsync_Should_Return_True()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var mockUserRepository = MockUserRepository.GetUserRepository().Object;
        var handler = new CreateTransactionCommandHandler(_mapper, mockUnitOfWork, mockUserRepository);
        var transactionToAdd = _fixture.Build<CreateTransactionDTO>()
            .With(x => x.Amount, 1000)
            .With(x => x.CategoryId, new Guid("D670263B-92CF-48C8-923A-EB09188F6077"))
            .With(x => x.UserId,new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .With(x => x.WalletId,new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"))
            .With(x => x.CurrencyId,new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"))
            .With(x => x.Label, "Label 1")
            .With(x => x.Note, " Note 1")
            .Create();

        var result = await handler.Handle(new CreateTransactionCommand
        {
            Transaction = transactionToAdd
        }, default);
        
        var budgetsCount = (await mockUnitOfWork.TransactionRepository.GetAllAsync()).Count;
        result.Success.Should().BeTrue();
        result.CreatedObject.Should().NotBeNull();
        result.CreatedObject.Should().BeOfType<TransactionBaseDTO>();
        budgetsCount.Should().Be(10);
    }
    
    [Fact]
    public async Task UpdateAsync_Should_Return_True_And_Old_With_New_Objects()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new UpdateTransactionCommandHandler(_mapper, mockUnitOfWork);
        var transactionToUpdate = _fixture.Build<UpdateTransactionDTO>()
            .With(x => x.Amount, 1)
            .With(x => x.Label, "new label")
            .With(x => x.Note, "new note")
            .With(x => x.CategoryId, new Guid("D670263B-92CF-48C8-923A-EB09188F6077"))
            .With(x => x.Id,new Guid("87D1A139-D842-4DC1-AAE3-07FE5A586527"))
            .With(x => x.CurrencyId,new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"))
            .Create();
        
        var result = await handler.Handle(new UpdateTransactionCommand
        {
            Transaction = transactionToUpdate
        }, default);
        
        result.Success.Should().BeTrue();
        result.Old.Should().NotBeNull();
        result.New.Should().NotBeNull();
        result.Old.Should().BeOfType<TransactionBaseDTO>();
        result.New.Should().BeOfType<TransactionBaseDTO>();
    }
    
    [Fact]
    public async Task DeleteAsync_Should_Return_True_And_Deleted_Object()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new DeleteTransactionCommandHandler(_mapper, mockUnitOfWork);
        
        var result = await handler.Handle(new DeleteTransactionCommand
        {
            Id = new Guid("87D1A139-D842-4DC1-AAE3-07FE5A586527")
        }, default);

        var budgetCountAfterDelete = (await mockUnitOfWork.TransactionRepository.GetAllAsync()).Count;
        
        result.Success.Should().BeTrue();
        result.DeletedObj.Should().NotBeNull();
        result.DeletedObj.Should().BeOfType<TransactionBaseDTO>();
        budgetCountAfterDelete.Should().Be(8);
    }
}