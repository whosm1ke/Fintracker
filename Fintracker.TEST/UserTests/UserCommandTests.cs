using AutoMapper;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.DTO.User;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.Features.User.Handlers.Commands;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Fintracker.TEST.UserTests;

public class UserCommandTests
{
    private readonly IMapper _mapper;

    public UserCommandTests()
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