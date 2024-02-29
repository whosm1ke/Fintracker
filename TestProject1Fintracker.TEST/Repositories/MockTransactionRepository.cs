using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Moq;

namespace TestProject1Fintracker.TEST.Repositories;

public class MockTransactionRepository
{
    public static Mock<ITransactionRepository> GetTransactionRepository()
    {
        var transactions = new List<Transaction>()
        {
            new()
            {
                Id = new Guid("B77ADE7A-5861-4899-AA20-FB97786E8767"),
                Amount = 100,
                Label = "Label 1",
                Note = "Note 1",
            },
            new()
            {
                Id = new Guid("7318F6A6-A7CC-4FC0-9B2E-0F28FD68525D"),
                Amount = 200,
                Label = "Label 2",
                Note = "Note 2",
            },
            new()
            {
                Id = new Guid("87D1A139-D842-4DC1-AAE3-07FE5A586527"),
                Amount = 300,
                Label = "Label 3",
                Note = "Note 3",
            }
        };

        var mock = new Mock<ITransactionRepository>();

        mock.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
            .Returns(async (Transaction b) =>
            {
                transactions.Add(b);
                return b;
            });

        mock.Setup(x => x.UpdateAsync(It.IsAny<Transaction>()))
            .Returns(async (Transaction b) =>
            {
                if (transactions.Find(x => x.Id == b.Id) != null)
                {
                    int index = transactions.FindIndex(x => x.Id == b.Id);
                    transactions[index] = b;
                }
            });

        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return transactions.Find(x => x.Id == id); });

        mock.Setup(x => x.GetAllAsync())
            .Returns(async () =>
            {
                return transactions;
            });

        mock.Setup(x => x.DeleteAsync(It.IsAny<Transaction>()))
            .Returns(async (Transaction b) =>
            {
                if (transactions.Find(x => x.Id == b.Id) != null)
                {
                    int index = transactions.FindIndex(x => x.Id == b.Id);
                    transactions.RemoveAt(index);
                }
            });

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return transactions.Find(x => x.Id == id) != null; });

        return mock;
    }
}