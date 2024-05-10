using AutoFixture;
using AutoMapper;
using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.DTO.Budget;
using Fintracker.Application.Features.Budget.Handlers.Commands;
using Fintracker.Application.Features.Budget.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;

namespace Fintracker.TEST.BudgetTests;

public class BudgetCommandTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly IUserRepository _userRepository;

    public BudgetCommandTests(IUserRepository userRepository)
    {
        _userRepository = MockUserRepository.GetUserRepository().Object;
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
    }
}