using Fintracker.Application.Contracts.Persistence;
using Fintracker.Domain.Entities;
using Fintracker.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fintracker.Persistence;

public static class ConfigurePresistence
{
    public static IServiceCollection ConfigurePresistenceServices(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(cfg.GetConnectionString("LocalDbConnectionString"));
        });

        IdentityBuilder builder = services.AddIdentityCore<User>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;

            options.User.RequireUniqueEmail = true;
        });

        builder = new IdentityBuilder(builder.UserType, builder.Services);
        builder.AddSignInManager<SignInManager<User>>();
        builder.AddRoles<IdentityRole<Guid>>();
        builder.AddEntityFrameworkStores<AppDbContext>();
        builder.AddDefaultTokenProviders();
        
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}