using Fintracker.Application.Contracts.Identity;
using Fintracker.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Http;

namespace Fintracker.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{

    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UnitOfWork(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private IBudgetRepository _budgetRepository;
    public IBudgetRepository BudgetRepository => _budgetRepository ??= new BudgetRepository(_context);
    
    private ICategoryRepository _categoryRepository;
    public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
    
    private ICurrencyRepository _currencyRepository;
    public ICurrencyRepository CurrencyRepository => _currencyRepository ??= new CurrencyRepository(_context);
    
    private ITransactionRepository _transactionRepository;
    public ITransactionRepository TransactionRepository => _transactionRepository ??= new TransactionRepository(_context);
    public IUserRepository UserRepository { get; }


    private IWalletRepository _walletRepository;
    public IWalletRepository WalletRepository => _walletRepository ??= new WalletRepository(_context);
    public async Task SaveAsync()
    {
        var username = _httpContextAccessor.HttpContext.User.FindFirst("Uid")?.Value;
        await _context.SaveChangesAsync(username ?? "Not authorized");
    }

    
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

}