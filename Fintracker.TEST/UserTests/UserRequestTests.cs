using AutoMapper;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.User.Handlers.Queries;
using Fintracker.Application.Features.User.Requests.Queries;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;

namespace Fintracker.TEST.UserTests;

public class UserRequestTests
{
    private readonly IMapper _mapper;

    public UserRequestTests()
    {
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<WalletProfile>();
            c.AddProfile<UserProfile>();
            c.AddProfile<CategoryProfile>();
            c.AddProfile<BudgetProfile>();
            c.AddProfile<CurrencyProfile>();
        });

        _mapper = mapperCfg.CreateMapper();
    }
   
}