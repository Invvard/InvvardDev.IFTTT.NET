using System.Reflection;
using InvvardDev.Ifttt.Core.Hosting;
using InvvardDev.Ifttt.Shared.Configuration;
using InvvardDev.Ifttt.Shared.Contracts;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Controllers;
using InvvardDev.Ifttt.Trigger.Hooks;
using InvvardDev.Ifttt.Trigger.Models;
using InvvardDev.Ifttt.Trigger.Models.Contracts;
using InvvardDev.Ifttt.Trigger.Reflection;
using InvvardDev.Ifttt.Trigger.Services;

namespace InvvardDev.Ifttt.Trigger.Hosting;

public static class ServiceCollectionExtensions
{
    public static IIftttServiceBuilder AddTriggers(this IIftttServiceBuilder builder)
    {
        builder.Services.AddHttpClient(IftttConstants.TriggerHttpClientName, (_, client) =>
        {
            client.BaseAddress = new Uri(builder.RealTimeBaseAddress);
            client.DefaultRequestHeaders.Add(IftttConstants.ServiceKeyHeader, builder.ServiceKey);
        });

        builder.Services.AddTransient<ITriggerHook, RealTimeNotificationWebHook>();

        builder.Services
               .AddControllers()
               .AddApplicationPart(Assembly.GetAssembly(typeof(TriggerController)) ?? throw new InvalidOperationException())
               .AddControllersAsServices();

        return builder;
    }

    public static IIftttServiceBuilder AddTriggerAutoMapper(this IIftttServiceBuilder builder)
    {
        builder.Services
               .AddSingleton<IProcessorRepository<TriggerMap>, TriggerRepository>()
               .AddTransient<ITriggerMapper, TriggerMapper>()
               .AddKeyedTransient<IAttributeLookup, TriggerAttributeLookup>(nameof(TriggerAttributeLookup))
               .AddKeyedTransient<IAttributeLookup, TriggerFieldsAttributeLookup>(nameof(TriggerFieldsAttributeLookup))
               .AddHostedService<TriggerAutoMapperService>();

        return builder;
    }

    public static IIftttAppBuilder ConfigureTriggers(this IIftttAppBuilder appBuilder)
    {
        appBuilder.App.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return appBuilder;
    }
}