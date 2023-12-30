using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using InvvardDev.Ifttt.Service.Api.Trigger.Hooks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace InvvardDev.Ifttt.Service.Api.Trigger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTriggers(this IServiceCollection services)
    {
        services.AddOptions<IftttOptions>()
            .BindConfiguration(IftttOptions.DefaultSectionName)
            .ValidateDataAnnotations();
        
        services.AddTransient<ITriggerHook, RealTimeNotificationWebHook>();

        services.AddHttpClient(IftttConstants.TriggerHttpClientName, (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<IftttOptions>>().Value;
            
            client.BaseAddress = new Uri(options.RealTimeBaseAddress);
            client.DefaultRequestHeaders.Add(IftttConstants.ServiceKeyHeader, options.ServiceKey);
        });
        
        return services;
    }
}
