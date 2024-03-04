using Fintracker.Application.Contracts.Persistence;

namespace Fintracker.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{

    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    private IBudgetRepository _budgetRepository;
    public IBudgetRepository BudgetRepository => _budgetRepository ??= new BudgetRepository(_context);
    
    private ICategoryRepository _categoryRepository;
    public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
    
    private ICurrencyRepository _currencyRepository;
    public ICurrencyRepository CurrencyRepository => _currencyRepository ??= new CurrencyRepository(_context);
    
    private ITransactionRepository _transactionRepository;
    public ITransactionRepository TransactionRepository => _transactionRepository ??= new TransactionRepository(_context);
    
    private IUserRepository _userRepository;
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
    
    private IWalletRepository _walletRepository;
    public IWalletRepository WalletRepository => _walletRepository ??= new WalletRepository(_context);
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

}