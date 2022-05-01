using HeavyMetalMachine.Core.Abstractions;
using HeavyMetalMachine.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Database = HeavyMetalMachine.Core.Services.Database;
using IDatabase = HeavyMetalMachine.Core.Abstractions.IDatabase;

namespace HeavyMetalMachine.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHeavyMetalDataServices(this IServiceCollection services)
    {
        services.AddScoped<IDatabase, Database>();
        services.AddHeavyMetalModelServices();
        return services;
    }

    private static IServiceCollection AddHeavyMetalModelServices(this IServiceCollection services)
    {
        services.AddScoped<IPostsService, PostsService>();
        return services;
    }
}