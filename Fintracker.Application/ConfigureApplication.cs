using System.Reflection;
using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fintracker.Application;

public static class ConfigureApplication
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.AddTransient<IHtmlPageHelper, HtmlPagesHelper>();
        return services;
    }
}