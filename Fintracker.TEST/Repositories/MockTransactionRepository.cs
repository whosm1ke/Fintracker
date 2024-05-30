using System.Linq.Dynamic.Core;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Fintracker.Domain.Enums;
using Moq;

namespace Fintracker.TEST.Repositories;

public class MockTransactionRepository
{
    public static Mock<ITransactionRepository> GetTransactionRepository()
    {
        var transactions = new List<Transaction>
        {
            new()
            {
                Id = new Guid("B77ADE7A-5861-4899-AA20-FB97786E8767"),
                Amount = 100,
                Label = "Label 1",
                Note = "Note 1",
                Category = new() { Id = new Guid("2CA5CC74-6D96-4878-8625-BC8E78DD295E") },
                CategoryId = new Guid("2CA5CC74-6D96-4878-8625-BC8E78DD295E"),
                UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
            },
            new()
            {
                Id = new Guid("3213D366-0D94-4709-8B5E-4D803E8A6932"),
                Amount = 120,
                Label = "Label 1.1",
                Note = "Note 1.1",
                Category = new() { Id = new Guid("2CA5CC74-6D96-4878-8625-BC8E78DD295E") },
                CategoryId = new Guid("2CA5CC74-6D96-4878-8625-BC8E78DD295E"),
                UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
            },
            new()
            {
                Id = new Guid("7318F6A6-A7CC-4FC0-9B2E-0F28FD68525D"),
                Amount = 200,
                Label = "Label 2",
                Note = "Note 2",
                Category = new() { Id = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19") },
                CategoryId = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19"),
                UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
            },
            new()
            {
                Id = new Guid("87D1A139-D842-4DC1-AAE3-07FE5A586527"),
                Amount = 300,
                Label = "Label 3",
                Note = "Note 3",
                WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
                Category = new() { Id = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19") },
                CategoryId = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19"),
                Wallet = new Wallet { Balance = 100 },
                UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
            },
            new()
            {
                Id = new Guid("DE97F5DD-8EAA-454A-BC9F-B34BD50D00B3"),
                Amount = 320,
                Label = "Label 3.1",
                Note = "Note 3.1",
                WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
                Category = new() { Id = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19") },
                CategoryId = new Guid("FA79B6AB-7E69-46CC-9522-D0F68DF0FE19"),
                Wallet = new Wallet { Balance = 200 },
                UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7")
            },
            new()
            {
                Id = new Guid("3C48E189-C154-4E74-B8C3-7F0CACD52FDB"),
                Amount = 300,
                Label = "Label 4",
                Note = "Note 4",
                WalletId = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
                Wallet = new()
                {
                    Id = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
                    Balance = 3000,
                    Name = "Wallet 3",
                    BankAccountId = "12345",
                    Currency = new Currency()
                    {
                        Symbol = "DLR"
                    }
                },
                CategoryId = new Guid("F0872017-AE98-427E-B976-B46AC2004D15"),
                UserId = new Guid("93F849FB-110A-44A4-8138-1404FF6556C7"),
                Currency = new()
                {
                    Id = new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"),
                    Name = "American Dollar",
                    Symbol = "DLR"
                },
                CurrencyId = new Guid("E01111DE-C2AF-4C40-B1F7-078875B7CC24"),
                Category = new()
                {
                    Id = new Guid("F0872017-AE98-427E-B976-B46AC2004D15"),
                    Type = CategoryType.EXPENSE,
                    Name = "Category 4",
                    Image = "log",
                    IconColour = "cyan",
                    UserId = new Guid("EDE38841-5183-4BDD-A148-D1923F170B1A")
                }
            }
        };

        var mock = new Mock<ITransactionRepository>();

        //GenericRepository

        mock.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
            .Returns((Transaction b) =>
            {
                transactions.Add(b);
                return Task.FromResult(b);
            });

        mock.Setup(x => x.Update(It.IsAny<Transaction>()))
            .Callback((Transaction b) =>
            {
                if (transactions.Find(x => x.Id == b.Id) != null)
                {
                    int index = transactions.FindIndex(x => x.Id == b.Id);
                    transactions[index] = b;
                }
            });

        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(transactions.Find(x => x.Id == id)); });

        mock.Setup(x => x.GetAsyncNoTracking(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(transactions.Find(x => x.Id == id)); });

        mock.Setup(x => x.GetAllAsync())
            .Returns(() => { return Task.FromResult<IReadOnlyList<Transaction>>(transactions)!; });

        mock.Setup(x => x.GetAllAsyncNoTracking())
            .Returns(() => { return Task.FromResult<IReadOnlyList<Transaction>>(transactions)!; });

        mock.Setup(x => x.Delete(It.IsAny<Transaction>()))
            .Callback((Transaction b) =>
            {
                if (transactions.Find(x => x.Id == b.Id) != null)
                {
                    int index = transactions.FindIndex(x => x.Id == b.Id);
                    transactions.RemoveAt(index);
                }
            });

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(transactions.Find(x => x.Id == id) != null); });

        //TransactionRepository

        mock.Setup(x => x.GetByCategoryIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns((Guid id, Guid userId) =>
                Task.FromResult((IReadOnlyList<Transaction>)transactions
                    .Where(x => x.CategoryId == id && x.UserId == userId).ToList()));

        mock.Setup(x =>
                x.GetByCategoryIdSortedAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<TransactionQueryParams>()))
            .Returns((Guid id, Guid userId, TransactionQueryParams query) => Task.FromResult(
                (IReadOnlyList<Transaction>)transactions.Where(x => x.CategoryId == id && x.UserId == userId)
                    .AsQueryable()
                    .OrderBy(query.SortBy)
                    .ToList()
            ));

        mock.Setup(x => x.GetByUserIdAsync(It.IsAny<Guid>()))
            .Returns((Guid id) =>
                Task.FromResult((IReadOnlyList<Transaction>)transactions.Where(x => x.UserId == id).ToList()));

        mock.Setup(x => x.GetByUserIdSortedAsync(It.IsAny<Guid>(), It.IsAny<TransactionQueryParams>()))
            .Returns((Guid id, TransactionQueryParams query) => Task.FromResult(
                (IReadOnlyList<Transaction>)transactions.Where(x => x.UserId == id)
                    .AsQueryable()
                    .OrderBy(query.SortBy)
                    .ToList()
            ));

        mock.Setup(x => x.GetByWalletIdAsync(It.IsAny<Guid>()))
            .Returns((Guid id) =>
                Task.FromResult((IReadOnlyList<Transaction>)transactions.Where(x => x.WalletId == id).ToList()));

        mock.Setup(x => x.GetByWalletIdInRangeAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns((Guid id, DateTime start, DateTime end) =>
                Task.FromResult((IReadOnlyList<Transaction>)transactions
                    .Where(x => x.WalletId == id && x.Date.Date >= start.Date && x.Date.Date <= end.Date).ToList()));

        mock.Setup(x => x.GetByWalletIdSortedAsync(It.IsAny<Guid>(), It.IsAny<TransactionQueryParams>()))
            .Returns((Guid id, TransactionQueryParams query) => Task.FromResult(
                (IReadOnlyList<Transaction>)transactions.Where(x => x.WalletId == id)
                    .AsQueryable()
                    .OrderBy(query.SortBy)
                    .ToList()
            ));

        mock.Setup(x => x.GetTransactionWithWalletAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) =>
            {
                var res = await Task.FromResult(transactions.FirstOrDefault(x => x.Id == id));
                return res;
            });


        mock.Setup(x => x.GetTransactionAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(transactions.Find(x => x.Id == id)); });

        return mock;
    }
}