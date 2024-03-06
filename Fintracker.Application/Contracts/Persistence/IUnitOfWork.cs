namespace Fintracker.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    public IBudgetRepository BudgetRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public ICurrencyRepository CurrencyRepository { get; }
    public ITransactionRepository TransactionRepository { get; }
    public IWalletRepository WalletRepository { get; }
    Task SaveAsync();
}