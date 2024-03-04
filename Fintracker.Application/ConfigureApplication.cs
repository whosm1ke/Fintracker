using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fintracker.Application;

public static class ConfigureApplication
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
}