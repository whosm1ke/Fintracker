using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.Features.Transaction.Handlers.Queries;
using Fintracker.Application.Features.Transaction.Requests.Queries;
using Fintracker.Application.MapProfiles;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using Fintracker.TEST.Repositories;

namespace Fintracker.TEST.TransactionTests;

public class TransactionRequestTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;

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

        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    public async Task GetTransactionById_Should_Return_With_Label1_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetTransactionByIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new TransactionBaseDTO()
        {
            Id = new Guid("B77ADE7A-5861-4899-AA20-FB97786E8767"),
            Amount = 100,
            Label = "Label 1",
            Note = "Note 1",
            Category = new() { Id = new Guid("2CA5CC74-6D96-4878-8625-BC8E78DD295E") }
        };

        var actualResult = await handler.Handle(new GetTransactionByIdRequest()
        {
            Id = new Guid("B77ADE7A-5861-4899-AA20-FB97786E8767")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetTransactionsByCategoryId_Should_Return_Label2_Label3_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetTransactionsByCategoryIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new List<TransactionBaseDTO>
        {
            new()
            {
                Id = new Guid("7318F6A6-A7CC-4FC0-9B2E-0F28FD68525D"),
                Amount = 200,
                Label = "Label 2",
                Note = "Note 2",
                Category = new() { Id = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19") },
            },
            new()
            {
                Id = new Guid("87D1A139-D842-4DC1-AAE3-07FE5A586527"),
                Amount = 300,
                Label = "Label 3",
                Note = "Note 3",
                Category = new() { Id = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19") },
            }
        };

        var actualResult = await handler.Handle(new GetTransactionsByCategoryIdRequest()
        {
            CategoryId = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetTransactionsByCategoryIdSorted_SortBy_Amount_Should_Return_Label2_Label3_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetTransactionsByCategoryIdSortedRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new List<TransactionBaseDTO>
        {
            new()
            {
                Id = new Guid("7318F6A6-A7CC-4FC0-9B2E-0F28FD68525D"),
                Amount = 200,
                Label = "Label 2",
                Note = "Note 2",
                Category = new() { Id = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19") },
            },
            new()
            {
                Id = new Guid("87D1A139-D842-4DC1-AAE3-07FE5A586527"),
                Amount = 300,
                Label = "Label 3",
                Note = "Note 3",
                Category = new() { Id = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19") },
            }
        };

        var actualResult = await handler.Handle(new GetTransactionsByCategoryIdSortedRequest()
        {
            CategoryId = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19"),
            SortBy = "Amount"
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetTransactionsByUserId_Should_Return_WithUser1_WithUser2_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetTransactionsByUserIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new List<TransactionBaseDTO>
        {
            new()
            {
                Id = new Guid("C333575E-1AF5-4C32-A540-1EE29EDD4ECB"),
                Amount = 120,
                Label = "With User 1",
                Note = "With User 1",
                UserId = new Guid("2A9E1D20-7464-4C82-BB23-001CC7F1783A")
            },
            new()
            {
                Id = new Guid("5F740828-527F-49F8-9422-4000805C81B5"),
                Amount = 120,
                Label = "With User 2",
                Note = "With User 2",
                UserId = new Guid("2A9E1D20-7464-4C82-BB23-001CC7F1783A")
            }
        };

        var actualResult = await handler.Handle(new GetTransactionsByUserIdRequest()
        {
            UserId = new Guid("2A9E1D20-7464-4C82-BB23-001CC7F1783A")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetTransactionsByUserIdSorted_SortBy_Amount_Should_Return_WithUser1_WithUser2_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetTransactionsByUserIdSortedRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new List<TransactionBaseDTO>
        {
            new()
            {
                Id = new Guid("C333575E-1AF5-4C32-A540-1EE29EDD4ECB"),
                Amount = 120,
                Label = "With User 1",
                Note = "With User 1",
                UserId = new Guid("2A9E1D20-7464-4C82-BB23-001CC7F1783A")
            },
            new()
            {
                Id = new Guid("5F740828-527F-49F8-9422-4000805C81B5"),
                Amount = 120,
                Label = "With User 2",
                Note = "With User 2",
                UserId = new Guid("2A9E1D20-7464-4C82-BB23-001CC7F1783A")
            }
        };

        var actualResult = await handler.Handle(new GetTransactionsByUserIdSortedRequest()
        {
            UserId = new Guid("2A9E1D20-7464-4C82-BB23-001CC7F1783A"),
            SortBy = "Amount"
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetTransactionsByWalletId_Should_Return_Wallet1_Wallet2_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetTransactionsByWalletIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new List<TransactionBaseDTO>
        {
            new()
            {
                Id = new Guid("C2D360B7-AB60-4C45-AA9B-867E9EF71921"),
                Amount = 320,
                Label = "With Wallet 1",
                Note = "With Wallet 1",
                WalletId = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506")
            },
            new()
            {
                Id = new Guid("3A84D4C2-E423-4697-9A06-DF0E1250E9B7"),
                Amount = 220,
                Label = "With Wallet 2",
                Note = "With Wallet 2",
                WalletId = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506")
            }
        };

        var actualResult = await handler.Handle(new GetTransactionsByWalletIdRequest()
        {
            WalletId = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetTransactionsByWalletIdSorted_SortBy_Amount_Should_Return_WithUser2_WithUser1_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetTransactionsByWalletIdSortedRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new List<TransactionBaseDTO>
        {
            new()
            {
                Id = new Guid("3A84D4C2-E423-4697-9A06-DF0E1250E9B7"),
                Amount = 220,
                Label = "With Wallet 2",
                Note = "With Wallet 2",
                WalletId = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506")
            },
            new()
            {
                Id = new Guid("C2D360B7-AB60-4C45-AA9B-867E9EF71921"),
                Amount = 320,
                Label = "With Wallet 1",
                Note = "With Wallet 1",
                WalletId = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506")
            }
        };

        var actualResult = await handler.Handle(new GetTransactionsByWalletIdSortedRequest()
        {
            WalletId = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506"),
            SortBy = "Amount"
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetTransactionWithWalletById_Should_Return_TransWithWallet_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetTransactionWithWalletByIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new TransactionWithWalletDTO()
        {
            Id = new Guid("D2E41134-B415-4450-B47A-D48A96EA9226"),
            Amount = 220,
            Label = "Trans Wallet",
            Note = "Trans Wallet",
            Wallet = new() { Id = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506"), Name = "Wallet 1" },
        };

        var actualResult = await handler.Handle(new GetTransactionWithWalletByIdRequest()
        {
            Id = new Guid("D2E41134-B415-4450-B47A-D48A96EA9226")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetTransactionWithUserById_Should_Return_TransWithUser_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetTransactionWithUserByIdRequestHandler(_mapper, mockUnitOfWork);
        var expectedResult = new TransactionWithUserDTO()
        {
            Id = new Guid("89748830-B290-4ED2-AB51-B2853D91B785"),
            Amount = 220,
            Label = "Trans User",
            Note = "Trans User",
            User = new() { Id = new Guid("3DCF7BFC-C7A1-48F2-A56D-B33740E4B3FF"), Email = "transUser@mail.com" },
        };

        var actualResult = await handler.Handle(new GetTransactionWithUserByIdRequest()
        {
            Id = new Guid("89748830-B290-4ED2-AB51-B2853D91B785")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetAllTransactions_Should_Return_9_Transa_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetTransactionsRequestHandler(_mapper, mockUnitOfWork);
        int expectedResultCount = 9;

        var actualResultCount = (await handler.Handle(new GetTransactionsRequest()
            ,default)).Count;

        actualResultCount.Should().Be(expectedResultCount);
    }
}