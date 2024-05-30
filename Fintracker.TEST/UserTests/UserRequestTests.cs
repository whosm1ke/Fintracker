using AutoMapper;
using Fintracker.Application.Exceptions;
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
    public async Task Test_GetUserById_With_Valid_Id_Should_Return_User1()
    {
        var mockUserRepo = MockUserRepository.GetUserRepository().Object;
        var handler = new GetUserByIdRequestHandler(_mapper, mockUserRepo);

        var result = await handler.Handle(new GetUserByIdRequest()
        {
            Id = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
        }, default);

        result.Email.Should().Be("user1@gmail.com");
        result.UserName.Should().Be("username1");
    }
    
    [Fact]
    public async Task Test_GetUserById_With_Invalid_Id_Should_Return_User1()
    {
        var mockUserRepo = MockUserRepository.GetUserRepository().Object;
        var handler = new GetUserByIdRequestHandler(_mapper, mockUserRepo);

        var invalidId = new Guid("A7A4047F-EE9C-4AFD-B96F-746A63B7665E"); // Ensure this ID doesn't exist

        // Assert
        Func<Task> act = async () => await handler.Handle(new GetUserByIdRequest()
        {
            Id = invalidId
        }, default);

        await act.Should().ThrowAsync<NotFoundException>(); // FluentA
    }
}