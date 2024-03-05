using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Moq;
using System.Linq.Dynamic.Core;

namespace Fintracker.TEST.Repositories;

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
                Category = new() { Id = new Guid("2CA5CC74-6D96-4878-8625-BC8E78DD295E") },
                CategoryId = new Guid("2CA5CC74-6D96-4878-8625-BC8E78DD295E")
            },
            new()
            {
                Id = new Guid("7318F6A6-A7CC-4FC0-9B2E-0F28FD68525D"),
                Amount = 200,
                Label = "Label 2",
                Note = "Note 2",
                Category = new() { Id = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19") },
                CategoryId = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19")
            },
            new()
            {
                Id = new Guid("87D1A139-D842-4DC1-AAE3-07FE5A586527"),
                Amount = 300,
                Label = "Label 3",
                Note = "Note 3",
                Category = new() { Id = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19") },
                CategoryId = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19")
            },
            new()
            {
                Id = new Guid("C333575E-1AF5-4C32-A540-1EE29EDD4ECB"),
                Amount = 120,
                Label = "With User 1",
                Note = "With User 1",
                User = new() { Id = new Guid("2A9E1D20-7464-4C82-BB23-001CC7F1783A") },
                UserId = new Guid("2A9E1D20-7464-4C82-BB23-001CC7F1783A")
            },
            new()
            {
                Id = new Guid("5F740828-527F-49F8-9422-4000805C81B5"),
                Amount = 120,
                Label = "With User 2",
                Note = "With User 2",
                User = new() { Id = new Guid("2A9E1D20-7464-4C82-BB23-001CC7F1783A") },
                UserId = new Guid("2A9E1D20-7464-4C82-BB23-001CC7F1783A")
            },

            new()
            {
                Id = new Guid("C2D360B7-AB60-4C45-AA9B-867E9EF71921"),
                Amount = 320,
                Label = "With Wallet 1",
                Note = "With Wallet 1",
                Wallet = new() { Id = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506") },
                WalletId = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506")
            },
            new()
            {
                Id = new Guid("3A84D4C2-E423-4697-9A06-DF0E1250E9B7"),
                Amount = 220,
                Label = "With Wallet 2",
                Note = "With Wallet 2",
                Wallet = new() { Id = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506") },
                WalletId = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506")
            },
            new()
            {
                Id = new Guid("D2E41134-B415-4450-B47A-D48A96EA9226"),
                Amount = 220,
                Label = "Trans Wallet",
                Note = "Trans Wallet",
                Wallet = new() { Id = new Guid("9B2EAEC4-DD3B-4572-9A69-48FBC50C8506"), Name = "Wallet 1" },
            },
            new()
            {
                Id = new Guid("89748830-B290-4ED2-AB51-B2853D91B785"),
                Amount = 220,
                Label = "Trans User",
                Note = "Trans User",
                User = new() { Id = new Guid("3DCF7BFC-C7A1-48F2-A56D-B33740E4B3FF"), Email = "transUser@mail.com" },
            }
        };

        var mock = new Mock<ITransactionRepository>();

        //GenericRepository

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

        mock.Setup(x => x.GetAsyncNoTracking(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return transactions.Find(x => x.Id == id); });

        mock.Setup(x => x.GetAllAsync())
            .Returns(async () => { return transactions; });

        mock.Setup(x => x.GetAllAsyncNoTracking())
            .Returns(async () => { return transactions; });

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

        //TransactionRepository

        mock.Setup(x => x.GetByCategoryIdAsync(It.IsAny<Guid>()))
            .Returns((Guid id) =>
                Task.FromResult((IReadOnlyList<Transaction>)transactions.Where(x => x.CategoryId == id).ToList()));

        mock.Setup(x => x.GetByCategoryIdSortedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns((Guid id, string sortBy, bool isDescending) => Task.FromResult(
                (IReadOnlyList<Transaction>)transactions.Where(x => x.CategoryId == id)
                    .AsQueryable()
                    .OrderBy(sortBy)
                    .ToList()
            ));

        mock.Setup(x => x.GetByUserIdAsync(It.IsAny<Guid>()))
            .Returns((Guid id) =>
                Task.FromResult((IReadOnlyList<Transaction>)transactions.Where(x => x.UserId == id).ToList()));

        mock.Setup(x => x.GetByUserIdSortedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns((Guid id, string sortBy, bool isDescending) => Task.FromResult(
                (IReadOnlyList<Transaction>)transactions.Where(x => x.UserId == id)
                    .AsQueryable()
                    .OrderBy(sortBy)
                    .ToList()
            ));

        mock.Setup(x => x.GetByWalletIdAsync(It.IsAny<Guid>()))
            .Returns((Guid id) =>
                Task.FromResult((IReadOnlyList<Transaction>)transactions.Where(x => x.WalletId == id).ToList()));

        mock.Setup(x => x.GetByWalletIdSortedAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Returns((Guid id, string sortBy, bool isDescending) => Task.FromResult(
                (IReadOnlyList<Transaction>)transactions.Where(x => x.WalletId == id)
                    .AsQueryable()
                    .OrderBy(sortBy)
                    .ToList()
            ));

        mock.Setup(x => x.GetTransactionWithWalletAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => Task.FromResult(transactions.FirstOrDefault(x => x.Id == id)));
        
        mock.Setup(x => x.GetTransactionWithUserAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => Task.FromResult(transactions.FirstOrDefault(x => x.Id == id)));
        
        mock.Setup(x => x.GetTransactionAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return transactions.Find(x => x.Id == id); });
        
        return mock;
    }
}