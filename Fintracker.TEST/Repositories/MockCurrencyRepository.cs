using System.Linq.Dynamic.Core;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Moq;

namespace Fintracker.TEST.Repositories;

public class MockCurrencyRepository
{
    public static Mock<ICurrencyRepository> GetCurrencyRepository()
    {
        var currencies = new List<Currency>
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
        var mock = new Mock<ICurrencyRepository>();

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns((Guid id) =>
            {
                return Task.FromResult(currencies.Find(c => c.Id == id) != null);
            });

        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns((Guid id) =>
            {
                return Task.FromResult(currencies.FirstOrDefault(x => x.Id == id));
            });

        mock.Setup(x => x.GetAllAsync())
            .Returns(() => Task.FromResult<IReadOnlyList<Currency?>>(currencies));
        
        mock.Setup(x => x.GetCurrenciesSorted(It.IsAny<string>(), It.IsAny<bool>()))
            .Returns((string sortBy, bool isDescending) => Task.FromResult((IReadOnlyList<Currency>)currencies
            .AsQueryable()
            .OrderBy(sortBy)
            .ToList()));
        
        return mock;
    }
}