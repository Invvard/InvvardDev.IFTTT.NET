using System.Reflection;
using InvvardDev.Ifttt.Shared.Configuration;
using InvvardDev.Ifttt.Shared.Contracts;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Hooks;
using InvvardDev.Ifttt.Trigger.Models;
using InvvardDev.Ifttt.Trigger.Models.Contracts;
using InvvardDev.Ifttt.Trigger.Reflection;
using InvvardDev.Ifttt.Trigger.Services;
using Microsoft.Extensions.Options;

namespace InvvardDev.Ifttt.Trigger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTriggers(this IServiceCollection services)
    {
        services.AddHttpClient(IftttConstants.TriggerHttpClientName, (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<IftttOptions>>().Value;

            client.BaseAddress = new Uri(options.RealTimeBaseAddress);
            client.DefaultRequestHeaders.Add(IftttConstants.ServiceKeyHeader, options.ServiceKey);
        });

        services.AddSingleton<IProcessorRepository<TriggerMap>, TriggerRepository>();
        services.AddScoped<IAssemblyAccessor, AssemblyAccessor>();
        services.AddTransient<ITriggerMapper, TriggerMapper>();
        services.AddTransient<ITriggerHook, RealTimeNotificationWebHook>();
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
           .GetRequiredService<ITriggerMapper>()
           .MapTriggerProcessors()
           .MapTriggerFields();

        app.MapControllers();

        return app;
    }
}