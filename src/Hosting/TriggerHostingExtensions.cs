using System.Reflection;
using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Controllers;
using InvvardDev.Ifttt.Hosting.Models;
using InvvardDev.Ifttt.Models.Trigger;
using InvvardDev.Ifttt.Reflection;
using InvvardDev.Ifttt.Services;
using InvvardDev.Ifttt.Toolkit.Contracts;
using InvvardDev.Ifttt.Toolkit.Hooks;

namespace InvvardDev.Ifttt.Hosting;

public static class TriggerHostingExtensions
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