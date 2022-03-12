using Forum.Data;
using Forum.Data.Abstractions;
using Forum.Services.Abstractions;
using Forum.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForumDataServices(this IServiceCollection services)
    {
        services.AddScoped<IDatabase, Database>();
        services.AddForumModelServices();
        return services;
    }

    private static IServiceCollection AddForumModelServices(this IServiceCollection services)
    {
        services.AddScoped<ITopicsService, TopicsService>();
        return services;
    }
}