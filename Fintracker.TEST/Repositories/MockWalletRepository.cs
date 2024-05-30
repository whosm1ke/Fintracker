using System.Linq.Dynamic.Core;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Models;
using Fintracker.Domain.Entities;
using Moq;

namespace Fintracker.TEST.Repositories;

public class MockWalletRepository
{
    public static Mock<IWalletRepository> GetWalletRepository()
    {
        var wallets = new List<Wallet>
        {
            new()
            {
                Id = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
                Balance = 1000,
                Name = "Wallet 1",
                OwnerId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9")
            },
            new()
            {
                Id = new Guid("16E4E7F0-48EE-46E6-9543-ABA7975FBE71"),
                Balance = 2000,
                Name = "Wallet 2",
                OwnerId = new Guid("A98A21C7-E794-4A65-B618-FA6D8A5F63D9")
            },
            new()
            {
                Id = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
                Balance = 3000,
                Name = "Wallet 3",
                BankAccountId = "12345"
            }
        };

        var mock = new Mock<IWalletRepository>();

        //GenericRepository

        mock.Setup(x => x.AddAsync(It.IsAny<Wallet>()))
            .Returns((Wallet b) =>
            {
                wallets.Add(b);
                return Task.FromResult(b);
            });

        mock.Setup(x => x.Update(It.IsAny<Wallet>()))
            .Callback((Wallet b) =>
            {
                if (wallets.Find(x => x.Id == b.Id) != null)
                {
                    int index = wallets.FindIndex(x => x.Id == b.Id);
                    wallets[index] = b;
                }
            });

        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(wallets.Find(x => x.Id == id)); });

        mock.Setup(x => x.GetAsyncNoTracking(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(wallets.Find(x => x.Id == id)); });

        mock.Setup(x => x.GetAllAsync())
            .Returns(() => Task.FromResult<IReadOnlyList<Wallet>>(wallets)!);

        mock.Setup(x => x.GetAllAsyncNoTracking())
            .Returns(() => Task.FromResult<IReadOnlyList<Wallet>>(wallets)!);

        mock.Setup(x => x.Delete(It.IsAny<Wallet>()))
            .Callback((Wallet b) =>
            {
                if (wallets.Find(x => x.Id == b.Id) != null)
                {
                    int index = wallets.FindIndex(x => x.Id == b.Id);
                    wallets.RemoveAt(index);
                }
            });

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => { return Task.FromResult(wallets.Find(x => x.Id == id) != null); });

        //WalletRepository

        mock.Setup(x => x.GetWalletById(It.IsAny<Guid>()))
            .Returns((Guid id) => Task.FromResult(wallets.Find(x => x.Id == id)));


        mock.Setup(x => x.GetByUserIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => wallets.Where(x => x.OwnerId == id || x.Users.Any(x => x.Id == id)).ToList());
        
        mock.Setup(x => x.GetByUserIdSortedAsync(It.IsAny<Guid>(),It.IsAny<QueryParams>()))
            .Returns((Guid userId, QueryParams query) => Task.FromResult((IReadOnlyList<Wallet>)wallets
                .Where(x => x.OwnerId == userId || x.Users.Any(x => x.Id == userId))
                .AsQueryable()
                .OrderBy(query.SortBy)
                .ToList()));

        return mock;
    }
}