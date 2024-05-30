using AutoFixture;
using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Transaction.Handlers.Queries;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using Xunit.Abstractions;

namespace Fintracker.TEST.TransactionTests;

public class TransactionRequestTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionRequestTests(ITestOutputHelper output)
    {
        _output = output;
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<TransactionProfile>();
            c.AddProfile<CategoryProfile>();
            c.AddProfile<CurrencyProfile>();
            c.AddProfile<WalletProfile>();
            c.AddProfile<UserProfile>();
        });
        _unitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    public async Task Test_GetTransactionById_With_Valid_Id()
    {
        var handler = new GetTransactionByIdRequestHandler(_mapper,_unitOfWork);

        var actualResult = await handler.Handle(new GetTransactionByIdRequest()
        {
            Id = new Guid("B77ADE7A-5861-4899-AA20-FB97786E8767")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Id.Should().Be(new Guid("B77ADE7A-5861-4899-AA20-FB97786E8767"));
    }
    
    [Fact]
    public async Task Test_GetTransactionById_With_Invalid_Should_Throw_NotFound()
    {
        var handler = new GetTransactionByIdRequestHandler(_mapper,_unitOfWork);
        

        Func<Task> act = async () => await handler.Handle(new GetTransactionByIdRequest()
        {
            Id = new Guid("F2733815-E508-429B-8222-B666F9462117")
        }, default);

        await act.Should().ThrowAsync<NotFoundException>(); 
    }
    
    [Fact]
    public async Task Test_GetTransactionsByUserId_With_Valid_UserId()
    {
        var handler = new GetTransactionsByUserIdRequestHandler(_mapper,_unitOfWork);

        var actualResult = await handler.Handle(new GetTransactionsByUserIdRequest()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(6);
    }
    
    [Fact]
    public async Task Test_GetTransactionsByUserId_With_Invalid_UserId_Should_Return_Count_0()
    {
        var handler = new GetTransactionsByUserIdRequestHandler(_mapper,_unitOfWork);
        
        
        
        var actualResult = await handler.Handle(new GetTransactionsByUserIdRequest()
        {
            UserId = new Guid("D605C511-5425-449D-9CD7-D7BDBE0FDC08")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(0);
    }
    
     
    [Fact]
    public async Task Test_GetTransactionsByWalletId_With_Valid_WalletId()
    {
        var handler = new GetTransactionsByWalletIdRequestHandler(_mapper,_unitOfWork);

        var actualResult = await handler.Handle(new GetTransactionsByWalletIdRequest()
        {
            WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(3);
    }
    
    [Fact]
    public async Task Test_GetTransactionsByWalletId_With_Invalid_WalletId_Should_Return_Count_0()
    {
        var handler = new GetTransactionsByWalletIdRequestHandler(_mapper,_unitOfWork);
        

        var actualResult = await handler.Handle(new GetTransactionsByWalletIdRequest()
        {
            WalletId = new Guid("C3305451-C7DB-48A3-AF78-AC04F9B6B467")

        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(0);
        
    }
    
    [Fact]
    public async Task Test_GetTransactionsByCategoryId_With_Valid_CategoryId()
    {
        var handler = new GetTransactionsByCategoryIdRequestHandler(_mapper,_unitOfWork);

        var actualResult = await handler.Handle(new GetTransactionsByCategoryIdRequest()
        {
            CategoryId = new Guid("2CA5CC74-6D96-4878-8625-BC8E78DD295E"),
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(2);
    }
    
    [Fact]
    public async Task Test_GetTransactionsByCategoryId_With_Invalid_CategoryId_Should_Return_Count_0()
    {
        var handler = new GetTransactionsByCategoryIdRequestHandler(_mapper,_unitOfWork);
        

        var actualResult = await handler.Handle(new GetTransactionsByCategoryIdRequest()
        {
            CategoryId = new Guid("C3305451-C7DB-48A3-AF78-AC04F9B6B467"),
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")

        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(0);
        
    }
    
    [Fact]
    public async Task Test_GetTransactionsByCategoryIdSorted_With_Valid_CategoryId()
    {
        var handler = new GetTransactionsByCategoryIdSortedRequestHandler(_mapper,_unitOfWork);

        var actualResult = await handler.Handle(new GetTransactionsByCategoryIdSortedRequest()
        {
            CategoryId = new Guid("2CA5CC74-6D96-4878-8625-BC8E78DD295E"),
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Params = new(){SortBy = "amount"}
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(2);
        actualResult.Should().BeInAscendingOrder(x => x.Amount);
    }
    
    [Fact]
    public async Task Test_GetTransactionsByCategoryIdSorted_With_Invalid_SortBy_Should_Throw_BadRequest()
    {
        var handler = new GetTransactionsByCategoryIdSortedRequestHandler(_mapper,_unitOfWork);


        Func<Task> act = async () => await handler.Handle(new GetTransactionsByCategoryIdSortedRequest()
        {
            CategoryId = new Guid("2CA5CC74-6D96-4878-8625-BC8E78DD295E"),
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Params = new(){SortBy = "non-existing-param"}
        }, default);

        await act.Should().ThrowAsync<BadRequestException>();

    }
    
    [Fact]
    public async Task Test_GetTransactionsByUserIdSorted_With_Valid_UserId()
    {
        var handler = new GetTransactionsByUserIdSortedRequestHandler(_mapper,_unitOfWork);

        var actualResult = await handler.Handle(new GetTransactionsByUserIdSortedRequest()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Params = new(){SortBy = "amount"}
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(6);
        actualResult.Should().BeInAscendingOrder(x => x.Amount);
    }
    
    [Fact]
    public async Task Test_GetTransactionsByUserIdSorted_With_Invalid_SortBy_Should_Throw_BadRequest()
    {
        var handler = new GetTransactionsByUserIdSortedRequestHandler(_mapper,_unitOfWork);


        Func<Task> act = async () => await handler.Handle(new GetTransactionsByUserIdSortedRequest()
        {
            UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Params = new(){SortBy = "non-existing-param"}
        }, default);

        await act.Should().ThrowAsync<BadRequestException>();

    }
    
     
    [Fact]
    public async Task Test_GetTransactionsByWalletIdSorted_With_Valid_UserId()
    {
        var handler = new GetTransactionsByWalletIdSortedRequestHandler(_mapper,_unitOfWork);

        var actualResult = await handler.Handle(new GetTransactionsByWalletIdSortedRequest()
        {
            WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
            Params = new(){SortBy = "amount"}
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Count.Should().Be(3);
        actualResult.Should().BeInAscendingOrder(x => x.Amount);
    }
    
    [Fact]
    public async Task Test_GetTransactionsByWalletIdSorted_With_Invalid_SortBy_Should_Throw_BadRequest()
    {
        var handler = new GetTransactionsByWalletIdSortedRequestHandler(_mapper,_unitOfWork);


        Func<Task> act = async () => await handler.Handle(new GetTransactionsByWalletIdSortedRequest()
        {
            WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
            Params = new(){SortBy = "bruh"}
        }, default);

        await act.Should().ThrowAsync<BadRequestException>();

    }
}