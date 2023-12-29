using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvvardDev.Ifttt.Service.Api.Trigger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTriggers(this IServiceCollection services, Action<IftttOptions> configureOptions)
    {
        services.AddHttpClient();
        return services;
    }
}
