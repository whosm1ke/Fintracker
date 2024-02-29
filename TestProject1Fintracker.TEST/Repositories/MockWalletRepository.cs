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
                Id = new Guid("BA5D310A-4CE3-41EA-AC27-C212AB5652A0")
            },
            new()
            {
                Id = new Guid("16E4E7F0-48EE-46E6-9543-ABA7975FBE71")
            }
        };
        var mock = new Mock<IWalletRepository>();

        mock.Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .Returns(async (Guid id) => { return wallets.Find(c => c.Id == id) != null; });

        return mock;
    }
}