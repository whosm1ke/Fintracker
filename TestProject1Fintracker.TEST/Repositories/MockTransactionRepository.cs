using Fintracker.Application.Contracts.Persistence;
using Moq;

namespace TestProject1Fintracker.TEST.Repositories;

public class MockTransactionRepository
{
    public static Mock<ITransactionRepository> GetTransactionRepository()
    {
        var mock = new Mock<ITransactionRepository>();

        
        return mock;
    }
}