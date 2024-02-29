using Fintracker.Application.Contracts.Persistence;
using Moq;

namespace TestProject1Fintracker.TEST.Repositories;

public class MockUnitOfWorkRepository
{
    public static Mock<IUnitOfWork> GetUniOfWork()
    {
        var mockUoW = new Mock<IUnitOfWork>();

        mockUoW.Setup(x => x.BudgetRepository)
            .Returns(MockBudgetRepository.GetBudgetRepository().Object);
        
        mockUoW.Setup(x => x.CategoryRepository)
            .Returns(MockCategoryRepository.GetCategoryRepository().Object);
        
        mockUoW.Setup(x => x.CurrencyRepository)
            .Returns(MockCurrencyRepository.GetCurrencyRepository().Object);
        
        mockUoW.Setup(x => x.WalletRepository)
            .Returns(MockWalletRepository.GetWalletRepository().Object);
        
        mockUoW.Setup(x => x.UserRepository)
            .Returns(MockUserRepository.GetUserRepository().Object);
        
        return mockUoW;
    }
}