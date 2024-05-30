using AutoFixture;
using AutoMapper;
using Fintracker.Application.DTO.Currency;
using Fintracker.Application.Features.Currency.Handlers.Queries;
using Fintracker.Application.Features.Currency.Requests.Queries;
using Fintracker.Application.MapProfiles;
using Fintracker.TEST.Repositories;
using FluentAssertions;

namespace Fintracker.TEST.CurrencyTests;

public class CurrencyRequestTests
{
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;

    public CurrencyRequestTests()
    {
        _fixture = new Fixture();
        var mapperCfg = new MapperConfiguration(c => { c.AddProfile<CurrencyProfile>(); });

        _mapper = mapperCfg.CreateMapper();
    }

    [Fact]
    public async Task GetCurrencies_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCurrenciesRequestHandler(mockUnitOfWork, _mapper);
        var expectedResult = new List<CurrencyDTO>
        {
            new()
            {
                Id = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"),
                Name = "Ukrainian hrivna",
                Symbol = "UAH"
            },
            new()
            {
                Id = new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"),
                Name = "American Dollar",
                Symbol = "DLR"
            }
        };

        var actualResult = await handler.Handle(new GetCurrenciesRequest(), default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetCurrenciesSorted_Sory_By_Symbol_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCurrenciesSortedRequestHandler(mockUnitOfWork, _mapper);

        var actualResult = await handler.Handle(new GetCurrenciesSortedRequest
        {
            Params = new()
            {
                SortBy = "symbol"
            }
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeInAscendingOrder(c => c.Symbol);
    }

    [Fact]
    public async Task GetCurrencyById_Should_Return_UAH_Test()
    {
        var mockUnitOfWork = MockUnitOfWorkRepository.GetUniOfWork().Object;
        var handler = new GetCurrencyByIdRequestHandler(mockUnitOfWork, _mapper);
        var expectedResult = new CurrencyDTO
        {
            Id = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"),
            Name = "Ukrainian hrivna",
            Symbol = "UAH"
        };

        var actualResult = await handler.Handle(new GetCurrencyByIdRequest
        {
            Id = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61")
        }, default);

        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}