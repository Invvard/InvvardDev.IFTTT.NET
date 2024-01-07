using System.Reflection;
using InvvardDev.Ifttt.Service.Api.Core;
using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;
using InvvardDev.Ifttt.Service.Api.Trigger.Hooks;
using InvvardDev.Ifttt.Service.Api.Trigger.Repositories;
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

        services.AddTransient<ITriggerHook, RealTimeNotificationWebHook>();
        services.AddSingleton<ITriggerRepository, TriggerRepositoryService>();
        services.AddKeyedTransient<IAttributeLookup, TriggerAttributeLookup>(nameof(TriggerAttributeLookup));
        services.AddKeyedTransient<IAttributeLookup, TriggerFieldsAttributeLookup>(nameof(TriggerFieldsAttributeLookup));

        services.AddControllers()
                .AddApplicationPart(Assembly.GetAssembly(typeof(IftttConstants)) ?? throw new InvalidOperationException())
                .AddControllersAsServices();

        return services;
    }

    public static IApplicationBuilder ConfigureTriggers(this WebApplication app)
    {
        app.Services
           .GetRequiredService<ITriggerRepository>()
           .MapTriggerTypes();

        app.ConfigureIftttApiClient();
        app.MapControllers();
        
        return app;
    }
}