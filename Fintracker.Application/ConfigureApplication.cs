using System.Reflection;
using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.Features.Behaviours;
using Fintracker.Application.Helpers;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fintracker.Application;

public static class ConfigureApplication
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services,
        IConfiguration configuration, string webRoot)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            x.AddOpenRequestPreProcessor(typeof(ValidationBehaviourPreProcess<>));
            x.AddOpenBehavior(typeof(LoggingPipelineBehaviour<,>));
        });
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.AddTransient<IHtmlPageHelper>(_ => new HtmlPageHelper(webRoot));
        return services;
    }
}