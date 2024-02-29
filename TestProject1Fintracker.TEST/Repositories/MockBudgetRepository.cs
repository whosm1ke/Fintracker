using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Moq;

namespace TestProject1Fintracker.TEST.Repositories;

public class MockBudgetRepository
{
    public static Mock<IBudgetRepository> GetBudgetRepository()
    {
        var budgets = new List<Budget>()
        {
            new Budget()
            {
                Balance = 1000,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = "ME",
                ModifiedBy = "ME",
                Currency = new Currency(),
                EndDate = new DateTime(2024, 12, 12),
                StartDate = new DateTime(2024, 12, 14),
                Name = "Budget1",
                Id = new Guid("29DECA11-E633-47E4-A0B2-569791B7D8C7"),
                Wallet = new Wallet(),
                User = new User(),
                Categories = new List<Category>()
            },
            new Budget()
            {
                Balance = 2000,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = "ME",
                ModifiedBy = "ME",
                Currency = new Currency(),
                EndDate = new DateTime(2024, 11, 12),
                StartDate = new DateTime(2024, 11, 14),
                Name = "Budget2",
                Id = new Guid("5F5F42ED-345C-4B13-AA35-76005A9607FF"),
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
                Id = new Guid("8FE42E15-4484-4DF7-BC1A-6C4047DD5C2C"),
                Wallet = new Wallet(),
                User = new User(),
                Categories = new List<Category>()
            }
        };

        var mock = new Mock<IBudgetRepository>();

        mock.Setup(x => x.AddAsync(It.IsAny<Budget>()))
            .Returns(async (Budget b) =>
            {
                budgets.Add(b);
                return b;
            });

        mock.Setup(x => x.UpdateAsync(It.IsAny<Budget>()))
            .Returns(async (Budget b) =>
            {
                if (budgets.Find(x => x.Id == b.Id) != null)
                {
                    int index = budgets.FindIndex(x => x.Id == b.Id);
                    budgets[index] = b;
                }
            });

        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return budgets.Find(x => x.Id == id); });

        mock.Setup(x => x.GetAllAsync())
            .Returns(async () =>
            {
                return budgets;
            });

        mock.Setup(x => x.DeleteAsync(It.IsAny<Budget>()))
            .Returns(async (Budget b) =>
            {
                if (budgets.Find(x => x.Id == b.Id) != null)
                {
                    int index = budgets.FindIndex(x => x.Id == b.Id);
                    budgets.RemoveAt(index);
                }
            });

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return budgets.Find(x => x.Id == id) != null; });
        return mock;
    }
}