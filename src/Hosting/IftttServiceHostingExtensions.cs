using System.Reflection;
using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Controllers;
using InvvardDev.Ifttt.Hosting.Middleware;
using InvvardDev.Ifttt.Hosting.Models;
using InvvardDev.Ifttt.Reflection;
using InvvardDev.Ifttt.Services;
using InvvardDev.Ifttt.Toolkit.Contracts;

namespace InvvardDev.Ifttt.Hosting;

public static class IftttServiceHostingExtensions
{
    public static IIftttServiceBuilder AddIftttToolkit(this IServiceCollection services, string serviceKey)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);

        return services.AddIftttToolkit(options => options.ServiceKey = serviceKey);
    }

    public static IIftttServiceBuilder AddIftttToolkit(this IServiceCollection services, Action<IftttOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        var builder = new DefaultIftttServiceBuilder(services);

        builder.Services.Configure(setupAction);

        return AddIftttToolkitCore(builder);
    }

    public static IIftttServiceBuilder AddTestSetupService<T>(this IIftttServiceBuilder builder)
        where T : class, ITestSetup
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddScoped<ITestSetup, T>();

        return builder;
    }

    private static IIftttServiceBuilder AddIftttToolkitCore(IIftttServiceBuilder builder)
    {
        builder.Services
               .AddControllers()
               .AddApplicationPart(Assembly.GetAssembly(typeof(StatusController)) ?? throw new InvalidOperationException())
               .AddControllersAsServices();

        builder.Services
               .AddScoped<IAssemblyAccessor, AssemblyAccessor>()
               .AddSingleton<IProcessorRepository, ProcessorRepository>();

        return builder;
    }

    public static IIftttAppBuilder ConfigureIftttToolkit(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        return new DefaultIftttAppBuilder(appBuilder);
    }

    public static IIftttAppBuilder UseServiceKeyAuthentication(this IIftttAppBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        appBuilder.App.UseMiddleware<ServiceKeyMiddleware>();

        return appBuilder;
    }
}