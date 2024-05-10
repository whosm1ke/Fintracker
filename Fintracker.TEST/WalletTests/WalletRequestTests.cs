using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.Transaction;
using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.Wallet.Handlers.Queries;
using Fintracker.Application.Features.Wallet.Requests.Queries;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;

namespace Fintracker.TEST.WalletTests;

public class WalletRequestTests
{
    private readonly IMapper _mapper;

    public WalletRequestTests()
    {
        new Fixture();
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<BudgetProfile>();
            c.AddProfile<CurrencyProfile>();
            c.AddProfile<WalletProfile>();
            c.AddProfile<UserProfile>();
            c.AddProfile<CategoryProfile>();
            c.AddProfile<TransactionProfile>();
        });

        _mapper = mapperCfg.CreateMapper();
    }
    
}