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

    [Fact]
    public async Task GetUserById_Should_Return_56C7_Test()
    {
        var mockUserRepo = MockUserRepository.GetUserRepository().Object;
        var handler = new GetUserByIdRequestHandler(_mapper, mockUserRepo);
        var expectedResult = new UserBaseDTO
        {
            Id = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
            Email = "user1@gmail.com",
            OwnedWallets = new List<WalletPureDTO>(),
            OwnedBudgets = new List<BudgetPureDTO>(),
            MemberWallets = new List<WalletPureDTO>(),
            UserName = "username1"
        };

        var actualResult = await handler.Handle(new GetUserByIdRequest
        {
            Id = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }


   
}