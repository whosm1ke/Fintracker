using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Moq;

namespace TestProject1Fintracker.TEST.Repositories;

public class MockWalletRepository
{
    public static Mock<IWalletRepository> GetWalletRepository()
    {
        var wallets = new List<Wallet>()
        {
            new()
            {
                Id = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0"),
                Balance = 1000,
                Name = "Wallet 1",
            },
            new()
            {
                Id = new Guid("16E4E7F0-48EE-46E6-9543-ABA7975FBE71"),
                Balance = 2000,
                Name = "Wallet 2",
            },
            new()
            {
                Id = new Guid("83E1C69F-8B85-46D4-8AB7-2DBE7D66038C"),
                Balance = 3000,
                Name = "Wallet 3",
            }
        };
        
        var mock = new Mock<IWalletRepository>();
        
        mock.Setup(x => x.AddAsync(It.IsAny<Wallet>()))
            .Returns(async (Wallet b) =>
            {
                wallets.Add(b);
                return b;
            });

        mock.Setup(x => x.UpdateAsync(It.IsAny<Wallet>()))
            .Returns(async (Wallet b) =>
            {
                if (wallets.Find(x => x.Id == b.Id) != null)
                {
                    int index = wallets.FindIndex(x => x.Id == b.Id);
                    wallets[index] = b;
                }
            });

        mock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return wallets.Find(x => x.Id == id); });

        mock.Setup(x => x.GetAllAsync())
            .Returns(async () =>
            {
                return wallets;
            });

        mock.Setup(x => x.DeleteAsync(It.IsAny<Wallet>()))
            .Returns(async (Wallet b) =>
            {
                if (wallets.Find(x => x.Id == b.Id) != null)
                {
                    int index = wallets.FindIndex(x => x.Id == b.Id);
                    wallets.RemoveAt(index);
                }
            });

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return wallets.Find(x => x.Id == id) != null; });

        return mock;
    }
}