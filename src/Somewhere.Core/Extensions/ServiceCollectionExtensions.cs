using Somewhere.Data;
using Somewhere.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Somewhere.Core.Abstractions;
using Somewhere.Core.Services;

namespace Somewhere.Core.Extensions;

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