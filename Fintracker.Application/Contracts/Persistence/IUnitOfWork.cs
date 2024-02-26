namespace Fintracker.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    public IBudgetRepository BudgetRepository { get; set; }
    public ICategoryRepository CategoryRepository { get; set; }
    public ICurrencyRepository CurrencyRepository { get; set; }
    public ITransactionRepository TransactionRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IWalletRepository WalletRepository { get; set; }
    Task<int> SaveAsync();
}