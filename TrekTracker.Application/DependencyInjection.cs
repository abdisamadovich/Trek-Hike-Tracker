using Microsoft.Extensions.DependencyInjection;

namespace TrekTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Application services (empty for now, all MediatR handlers are in Infrastructure)
        return services;
    }
}
