using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Transaction;
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
}