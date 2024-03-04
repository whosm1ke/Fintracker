using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Moq;
using System.Linq.Dynamic.Core;

namespace Fintracker.TEST.Repositories;

public class MockBudgetRepository
{
    public static Mock<IBudgetRepository> GetBudgetRepository()
    {
        var budgets = new List<Budget>
        {
            new Budget()
            {
                Balance = 2000,
                CreatedAt = new DateTime(2024, 12, 12),
                ModifiedAt = new DateTime(2024, 12, 12),
                CreatedBy = "ME2000",
                ModifiedBy = "ME2000",
                Currency = new Currency(),
                EndDate = new DateTime(2024, 12, 12),
                StartDate = new DateTime(2024, 12, 14),
                Name = "A",
                Id = new Guid("29DECA11-E633-47E4-A0B2-569791B7D8C7"),
                Wallet = new Wallet(),
                User = new User(),
                Categories = new List<Category>(),
                UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
                CurrencyId = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"),
                WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0")
            },
            new Budget()
            {
                Balance = 1000,
                CreatedAt = new DateTime(2024, 12, 12),
                ModifiedAt = new DateTime(2024, 12, 12),
                CreatedBy = "ME1000",
                ModifiedBy = "ME1000",
                Currency = new Currency(),
                EndDate = new DateTime(2024, 11, 12),
                StartDate = new DateTime(2024, 11, 14),
                Name = "B",
                Id = new Guid("5F5F42ED-345C-4B13-AA35-76005A9607FF"),
                Wallet = new Wallet(),
                User = new User(),
                Categories = new List<Category>(),
                WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
                UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
                CurrencyId = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61")
            },
            new Budget()
            {
                Balance = 3000,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = "ME",
                ModifiedBy = "ME",
                Currency = new Currency(),
                EndDate = new DateTime(2024, 10, 12),
                StartDate = new DateTime(2024, 10, 14),
                Name = "Budget3",
                Id = new Guid("8FE42E15-4484-4DF7-BC1A-6C4047DD5C2C"),
                Wallet = new Wallet(),
                User = new User(),
                Categories = new List<Category>()
            },
            new Budget()
            {
                Balance = 3000,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = "ME",
                ModifiedBy = "ME",
                Currency = new Currency(),
                EndDate = new DateTime(2024, 10, 12),
                StartDate = new DateTime(2024, 10, 14),
                Name = "Budget3",
                Id = new Guid("3DA8AFC6-1671-4E6C-A4AC-6A048C388764"),
                Wallet =  new()
                {
                    Id = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
                    Balance = 1000,
                    Name = "Wallet 1",
                },
                User = new User()
                {
                    Id = new Guid("83F849FB-110A-44A4-8138-1404FF6556C7")
                },
                Categories = new List<Category>(),
                WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
                UserId = new Guid("83F849FB-110A-44A4-8138-1404FF6556C7"),
                CurrencyId = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61")
            },
            new Budget()
            {
                Balance = 3000,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = "ME",
                ModifiedBy = "ME",
                Currency = new Currency(),
                EndDate = new DateTime(2024, 10, 12),
                StartDate = new DateTime(2024, 10, 14),
                Name = "Budget with user",
                Id = new Guid("9055E428-38C3-4616-A389-0102B766FD98"),
                Wallet =  new(),
                User = new User()
                {
                    Id = new Guid("83F849FB-110A-44A4-8138-1404FF6556C7"),
                    Email = "user@mail.com"
                },
                Categories = new List<Category>(),
                WalletId = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
                UserId = new Guid("83F849FB-110A-44A4-8138-1404FF6556C7"),
                CurrencyId = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61")
            }
        };

        var mock = new Mock<IBudgetRepository>();

        //GenericRepository

    //AddAsync
        mock.Setup(x => x.AddAsync(It.IsAny<Budget>()))
            .Returns(async (Budget b) =>
            {
                budgets.Add(b);
                return b;
            });
    //UpdateAsync
        mock.Setup(x => x.UpdateAsync(It.IsAny<Budget>()))
            .Returns(async (Budget b) =>
            {
                if (budgets.Find(x => x.Id == b.Id) != null)
                {
                    int index = budgets.FindIndex(x => x.Id == b.Id);
                    budgets[index] = b;
                }
            });
    //GetAsync
        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return budgets.Find(x => x.Id == id); });
    //GetAsyncNoTracking
        mock.Setup(x => x.GetAsyncNoTracking(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return budgets.Find(x => x.Id == id); });
    //GetAllAsyncNoTracking
        mock.Setup(x => x.GetAllAsyncNoTracking())
            .ReturnsAsync(budgets);
    //GetAllAsync
        mock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(budgets);
    //DeleteAsync
        mock.Setup(x => x.DeleteAsync(It.IsAny<Budget>()))
            .Returns(async (Budget b) =>
            {
                if (budgets.Find(x => x.Id == b.Id) != null)
                {
                    int index = budgets.FindIndex(x => x.Id == b.Id);
                    budgets.RemoveAt(index);
                }
            });
    //ExistsAsync
        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return budgets.FirstOrDefault(x => x.Id == id) != null; });

    //BudgetRepository
        mock.Setup(x => x.GetBudgetWithCategoriesAsync(It.IsAny<Guid>()))
            .Returns( async (Guid id) => { return budgets.FirstOrDefault(x => x.Id == id); });

        
        mock.Setup(x => x.GetBudgetWithWalletAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) =>
            {
                return budgets.FirstOrDefault(x => x.Id == id);
            });

        mock.Setup(x => x.GetBudgetWithUserAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return budgets.FirstOrDefault(x => x.Id == id); });

        mock.Setup(x => x.GetByUserIdAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => Task.FromResult((IReadOnlyList<Budget>)budgets.Where(x => x.UserId == id).ToList()));

        mock.Setup(x => x.GetByUserIdSortedAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns((Guid id, string sortBy) => Task.FromResult((IReadOnlyList<Budget>)budgets.Where(x => x.UserId == id)
                .AsQueryable()
                .OrderBy(sortBy)
                .ToList()));

        mock.Setup(x => x.GetByWalletIdAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => Task.FromResult((IReadOnlyList<Budget>)budgets.Where(x => x.WalletId == id).ToList()));

        mock.Setup(x => x.GetByWalletIdSortedAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns((Guid id, string sortBy) => Task.FromResult((IReadOnlyList<Budget>)budgets.Where(x => x.WalletId == id)
                .AsQueryable()
                .OrderBy(sortBy)
                .ToList()));


        return mock;
    }
}