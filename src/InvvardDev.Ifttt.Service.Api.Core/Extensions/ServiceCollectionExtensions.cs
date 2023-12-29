using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvvardDev.Ifttt.Service.Api.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIftttApiClient(this IServiceCollection services, Action<IftttOptions> configureOptions)
    {
        return services;
    }
}
