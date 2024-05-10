using AutoFixture;
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
}