using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Hosting.Models;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Reflection;
using InvvardDev.Ifttt.Services;
using InvvardDev.Ifttt.Toolkit.Contracts;
using InvvardDev.Ifttt.Toolkit.Hooks;
using Microsoft.Extensions.Options;

namespace InvvardDev.Ifttt.Hosting;

public static class TriggerHostingExtensions
{
    public static IIftttServiceBuilder AddTriggers(this IIftttServiceBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        builder.Services.AddHttpClient(IftttConstants.TriggerHttpClientName, (factory, client) =>
        {
            var options = factory.GetRequiredService<IOptions<IftttOptions>>();

            client.BaseAddress = new Uri(options.Value.RealTimeBaseAddress);
            client.DefaultRequestHeaders.Add(IftttConstants.ServiceKeyHeader, options.Value.ServiceKey);
        });

        builder.Services
               .AddKeyedTransient<IProcessorService, TriggerService>(ProcessorKind.Trigger)
               .AddKeyedTransient<IAttributeLookup, TriggerAttributeLookup>(nameof(TriggerAttributeLookup))
               .AddKeyedTransient<IAttributeLookup, TriggerFieldsAttributeLookup>(nameof(TriggerFieldsAttributeLookup));

        builder.Services.AddTransient<ITriggerHook, RealTimeNotificationWebHook>();

        return builder;
    }

    public static IIftttServiceBuilder AddTriggerAutoMapper(this IIftttServiceBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services
               .AddTransient<ITriggerMapper, TriggerMapper>()
               .AddHostedService<TriggerAutoMapperService>();

        return builder;
    }

    public static IIftttAppBuilder ConfigureTriggers(this IIftttAppBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        appBuilder.App
                  .UseRouting()
                  .UseEndpoints(endpoints =>
                  {
                      endpoints.MapControllers();
                  });

        return appBuilder;
    }
}