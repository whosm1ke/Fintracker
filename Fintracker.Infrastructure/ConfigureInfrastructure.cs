using Fintracker.Application.Contracts.Infrastructure;
using Fintracker.Application.Models.Mail;
using Fintracker.Infrastructure.Mail;
using Fintracker.Infrastructure.Monobank;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fintracker.Infrastructure;

public static class ConfigureInfrastructure
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailSender,EmailSender>();
        services.AddHttpClient("MonobankClient",x =>
        {
            x.BaseAddress = new Uri("https://api.monobank.ua");
        });
        services.AddTransient<IMonobankService, MonobankService>();
        return services;
    }
}