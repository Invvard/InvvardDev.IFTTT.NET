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
    public static IIftttServiceBuilder AddTriggers(this IIftttServiceBuilder builder)
    {
        builder.Services.AddHttpClient(IftttConstants.TriggerHttpClientName, (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<IftttOptions>>().Value;

            client.BaseAddress = new Uri(options.RealTimeBaseAddress);
            client.DefaultRequestHeaders.Add(IftttConstants.ServiceKeyHeader, options.ServiceKey);
        });

        builder.AddSingleton<IProcessorRepository<TriggerMap>, TriggerRepository>();
        builder.AddScoped<IAssemblyAccessor, AssemblyAccessor>();
        builder.AddTransient<ITriggerMapper, TriggerMapper>();
        builder.AddTransient<ITriggerHook, RealTimeNotificationWebHook>();
        builder.AddKeyedTransient<IAttributeLookup, TriggerAttributeLookup>(nameof(TriggerAttributeLookup));
        builder.AddKeyedTransient<IAttributeLookup, TriggerFieldsAttributeLookup>(nameof(TriggerFieldsAttributeLookup));

        builder.AddControllers()
                .AddApplicationPart(Assembly.GetAssembly(typeof(IftttConstants)) ?? throw new InvalidOperationException())
                .AddControllersAsServices();

        return builder;
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