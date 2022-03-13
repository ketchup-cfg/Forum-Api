using Somewhere.Data;
using Somewhere.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Somewhere.Services.Abstractions;
using Somewhere.Services.Services;

namespace Somewhere.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSomeDataServices(this IServiceCollection services)
    {
        services.AddScoped<IDatabase, Database>();
        services.AddSomeModelServices();
        return services;
    }

    private static IServiceCollection AddSomeModelServices(this IServiceCollection services)
    {
        services.AddScoped<ITopicsService, TopicsService>();
        return services;
    }
}