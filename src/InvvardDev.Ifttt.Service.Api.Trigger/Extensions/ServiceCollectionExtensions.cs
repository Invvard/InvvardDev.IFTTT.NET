using InvvardDev.Ifttt.Service.Api.Core;
using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;
using InvvardDev.Ifttt.Service.Api.Trigger.Hooks;
using InvvardDev.Ifttt.Service.Api.Trigger.TriggerRepository;
using Microsoft.Extensions.Options;

namespace InvvardDev.Ifttt.Service.Api.Trigger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTriggers(this IServiceCollection services)
    {
        services.AddIftttApiClient();

        services.AddHttpClient(IftttConstants.TriggerHttpClientName, (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<IftttOptions>>().Value;

            client.BaseAddress = new Uri(options.RealTimeBaseAddress);
            client.DefaultRequestHeaders.Add(IftttConstants.ServiceKeyHeader, options.ServiceKey);
        });

        services.AddControllers();

        services.AddTransient<ITriggerHook, RealTimeNotificationWebHook>();
        services.AddSingleton<ITriggerRepository, TriggerRepositoryService>();

        services.AddHttpContextAccessor();

        return services;
    }

    public static IApplicationBuilder ConfigureTriggers(this WebApplication app)
    {
        app.Services
           .GetRequiredService<ITriggerRepository>()
           .AddTriggerTypes(TriggerAttributeLookup.GetTriggerTypes());

        app.MapControllers();
        app.ConfigureIftttApiClient();
        
        return app;
    }
}