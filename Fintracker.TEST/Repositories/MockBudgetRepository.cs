using System.Linq.Dynamic.Core;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Moq;

namespace Fintracker.TEST.Repositories;

public class MockBudgetRepository
{
    public static Mock<IBudgetRepository> GetBudgetRepository()
    {
        var budgets = new List<Budget>
        {
            new()
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
                Owner = new User(),
                Categories = new List<Category>(),
                OwnerId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
                CurrencyId = new Guid("E014D577-D121-4399-B3BE-36D6E80C9F61"),
                WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C")
            },
            new Budget()
            {
                Id = new Guid("9C7EC483-ED14-4390-BBBA-A0753E55307F"),
                StartBalance = 1000,
                Balance = 1000,
                OwnerId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
                Categories = new List<Category>()
                {
                    new Category()
                    {
                        Id = new Guid("F0872017-AE98-427E-B976-B46AC2004D15"),
                        Name = "TEST CATEGORY"
                    }
                },
                Currency = new()
                {
                    Id = new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"),
                    Name = "American Dollar",
                    Symbol = "DLR"
                },
                IsPublic = true,
                WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
                StartDate = new DateTime(2024, 1, 12),
                EndDate = new DateTime(2024, 12, 12)
            }
        };

        var mock = new Mock<IBudgetRepository>();

        //GenericRepository

        //AddAsync
        mock.Setup(x => x.AddAsync(It.IsAny<Budget>()))
            .Returns((Budget b) =>
            {
                budgets.Add(b);
                return Task.FromResult(b);
            });
        //UpdateAsync
        mock.Setup(x => x.Update(It.IsAny<Budget>()))
            .Callback((Budget b) =>
            {
                if (budgets.Find(x => x.Id == b.Id) != null)
                {
                    int index = budgets.FindIndex(x => x.Id == b.Id);
                    budgets[index] = b;
                }
            });
        //GetAsync
        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(budgets.Find(x => x.Id == id)); });
        //GetAsyncNoTracking
        mock.Setup(x => x.GetAsyncNoTracking(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(budgets.Find(x => x.Id == id)); });
        //GetAllAsyncNoTracking
        mock.Setup(x => x.GetAllAsyncNoTracking())
            .Returns(() => Task.FromResult<IReadOnlyList<Budget?>>(budgets));
        //GetAllAsync
        mock.Setup(x => x.GetAllAsync())
            .Returns(() => Task.FromResult<IReadOnlyList<Budget?>>(budgets));
        //DeleteAsync
        mock.Setup(x => x.Delete(It.IsAny<Budget>()))
            .Callback((Budget b) =>
            {
                if (budgets.Find(x => x.Id == b.Id) != null)
                {
                    int index = budgets.FindIndex(x => x.Id == b.Id);
                    budgets.RemoveAt(index);
                }
            });
        //ExistsAsync
        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(budgets.FirstOrDefault(x => x.Id == id) != null); });

        //BudgetRepository
        mock.Setup(x => x.GetBudgetByIdAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(budgets.FirstOrDefault(x => x.Id == id)); });

        mock.Setup(x => x.GetBudgetsByUserIdAsync(It.IsAny<Guid>(), It.IsAny<bool?>()))
            .Returns((Guid id, bool? isPublic) =>
                Task.FromResult((IReadOnlyList<Budget>)budgets.Where(x => x.OwnerId == id).ToList()));


        mock.Setup(x => x.GetByWalletIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool?>()))
            .Returns((Guid id, Guid userId, bool? isPublic) =>
                Task.FromResult((IReadOnlyList<Budget>)budgets
                    .Where(x => x.WalletId == id && (x.OwnerId == userId || x.Members.Any(m => m.Id == userId)))
                    .ToList()));

        mock.Setup(x => x.GetByWalletIdSortedAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<BudgetQueryParams>()))
            .Returns((Guid id, Guid userId, BudgetQueryParams query) => Task.FromResult((IReadOnlyList<Budget>)budgets
                .Where(x => x.WalletId == id)
                .AsQueryable()
                .OrderBy(query.SortBy)
                .ToList()));
        
        mock.Setup(x => x.GetByUserIdSortedAsync(It.IsAny<Guid>(), It.IsAny<BudgetQueryParams>()))
            .Returns((Guid userId, BudgetQueryParams query) => Task.FromResult((IReadOnlyList<Budget>)budgets
                .Where(x => x.OwnerId == userId || x.Members.Any(m => m.Id == userId))
                .AsQueryable()
                .OrderBy(query.SortBy)
                .ToList()));

        mock.Setup(x => x.GetBudgetsByCategoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns((Guid catId, Guid userId) =>
            {
                IReadOnlyList<Budget> budgetsToReturn =
                    budgets.Where(b =>
                        b.Categories != null && b.Categories.Any(c => c.Id == catId) && b.OwnerId == userId ||
                        b.Members != null && b.Members.Any(m => m.Id == userId)).ToList();

                return Task.FromResult(budgetsToReturn);
            });
        return mock;
    }
}