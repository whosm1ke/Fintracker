using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Wallet;
using Fintracker.Application.DTO.Wallet.Validators;
using Fintracker.Application.Exceptions;
using Fintracker.Application.Features.Wallet.Handlers.Commands;
using Fintracker.Application.Features.Wallet.Requests.Commands;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit.Abstractions;

namespace Fintracker.TEST.WalletTests;

public class WalletCommandTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly CreateWalletDtoValidator _createWalletDtoValidator;
    private readonly UpdateWalletDtoValidator _updateWalletDtoValidator;

    public WalletCommandTests(ITestOutputHelper output)
    {
        _output = output;
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c =>
        {
            c.AddProfile<WalletProfile>();
            c.AddProfile<UserProfile>();
            c.AddProfile<CurrencyProfile>();
        });

        _createWalletDtoValidator = new CreateWalletDtoValidator(MockUnitOfWorkRepository.GetUniOfWork().Object,
            MockUserRepository.GetUserRepository().Object);

        _updateWalletDtoValidator = new UpdateWalletDtoValidator(MockUnitOfWorkRepository.GetUniOfWork().Object,
            MockUserRepository.GetUserRepository().Object);

        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    public async Task AddWallet_With_Valid_Params_Should_Return_True()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new CreateWalletCommandHandler(mockUnitOfWork, _mapper);
        var walletToAdd = _fixture.Build<CreateWalletDTO>()
            .With(x => x.OwnerId, new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"))
            .With(x => x.Name, "New wallet")
            .With(x => x.StartBalance, 1000)
            .With(x => x.CurrencyId, new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"))
            .Create();

        var command = new CreateWalletCommand()
        {
            Wallet = walletToAdd
        };

        var result = await handler.Handle(command, default);

        var validationResult =
            await _createWalletDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldNotHaveAnyValidationErrors();


        int walletCount = (await mockUnitOfWork.WalletRepository.GetAllAsync()).Count;
        result.Success.Should().BeTrue();
        walletCount.Should().Be(4);
    }

    [Fact]
    public async Task AddWallet_With_InValid_Params_Should_Not_Add()
    {
        var walletToAdd = _fixture.Build<CreateWalletDTO>()
            .With(x => x.OwnerId, new Guid("5F4C09E0-6AAF-4855-9FFA-426B024FE376"))
            .With(x => x.Name, "New wallet")
            .With(x => x.StartBalance, 1000)
            .With(x => x.CurrencyId, new Guid("A729E29A-D34D-4DFA-9941-4518B589FAE7"))
            .Create();

        var command = new CreateWalletCommand()
        {
            Wallet = walletToAdd
        };


        var validationResult =
            await _createWalletDtoValidator.TestValidateAsync(command, strategy => strategy.IncludeAllRuleSets());

        validationResult.ShouldHaveAnyValidationError();
    }


    [Fact]
    public async Task UpdateWallet_With_Valid_Params_Should_Return_True()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new UpdateWalletCommandHandler(mockUnitOfWork, _mapper,
            MockCurrencyConverter.GetCurrencyConverter().Object);
        var walletToUpdate = _fixture.Build<UpdateWalletDTO>()
            .With(x => x.Id, new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"))
            .With(x => x.Name, "UpdatedWallet wallet")
            .With(x => x.StartBalance, 2000)
            .With(x => x.CurrencyId, new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"))
            .With(x => x.UserIds,
                new List<Guid>()
                {
                    new Guid("6090BFC8-4D8D-4AF5-9D37-060581F051A7"),
                })
            .Create();

        var command = new UpdateWalletCommand()
        {
            Wallet = walletToUpdate
        };


        var validationResult =
            await _updateWalletDtoValidator.TestValidateAsync(command);

        validationResult.ShouldNotHaveAnyValidationErrors();
        var result = await handler.Handle(command, default);


        var expectedWallet =
            await mockUnitOfWork.WalletRepository.GetWalletById(new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"));

        expectedWallet.Name.Should().Be(result.New.Name);
        expectedWallet.StartBalance.Should().Be(result.New.StartBalance);
    }

    [Fact]
    public async Task UpdateWallet_With_InValid_Params_Should_Not_Be_Updated()
    {
        var walletToUpdate = _fixture.Build<UpdateWalletDTO>()
            .With(x => x.Id, new Guid("385DFD83-D9D9-4711-9458-8AC3ABD3B09D"))
            .With(x => x.Name, "UpdatedWallet wallet")
            .With(x => x.StartBalance, -2000)
            .With(x => x.CurrencyId, new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"))
            .With(x => x.UserIds,
                new List<Guid>()
                {
                    new Guid("0EF23E22-930B-40EF-AC31-3C93D7F04DE3"),
                })
            .Create();

        var command = new UpdateWalletCommand()
        {
            Wallet = walletToUpdate
        };


        var validationResult =
            await _updateWalletDtoValidator.TestValidateAsync(command);

        validationResult.ShouldHaveAnyValidationError();
       
    }
    
    [Fact]
    public async Task DeleteWallet_With_Valid_Params_Should_be_Deleted()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new DeleteWalletCommandHandler(mockUnitOfWork, _mapper);

        var command = new DeleteWalletCommand()
        {
            Id = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
            UserId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9")
        };


        var result = await handler.Handle(command, default);

        result.Should().NotBeNull();
        result.DeletedObj.Should().NotBeNull();
        result.Success.Should().BeTrue();
        
        int walletCount = (await mockUnitOfWork.WalletRepository.GetAllAsync()).Count;
        walletCount.Should().Be(2);
    }
    
    [Fact]
    public async Task DeleteWallet_With_InValid_Params_Should_Not_Be_Deleted()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new DeleteWalletCommandHandler(mockUnitOfWork, _mapper);

        var command = new DeleteWalletCommand()
        {
            Id = new Guid("B34063CA-A5D8-47C5-8633-AFC870F8846F"),
            UserId = new Guid("B35CF5BD-A778-470F-872B-80D2A689FA85")
        };



        Func<Task> act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<NotFoundException>(); // FluentAssertions
    }
}