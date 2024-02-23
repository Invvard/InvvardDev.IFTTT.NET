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
    /// <summary>
    /// Extension method to add IftttToolkit to the service collection with the provided IFTTT service key.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="serviceKey">The IFTTT service key.</param>
    /// <returns>The <see cref="IIftttServiceBuilder"/> instance.</returns>
    public static IIftttServiceBuilder AddIftttToolkit(this IServiceCollection services, string serviceKey)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceKey);

        return services.AddIftttToolkit(options => options.ServiceKey = serviceKey);
    }

    /// <summary>
    /// Extension method to add IftttToolkit to the service collection with a custom set of <see cref="IftttOptions"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="setupAction">Custom set of options.</param>
    /// <returns>The <see cref="IIftttServiceBuilder"/> instance.</returns>
    public static IIftttServiceBuilder AddIftttToolkit(this IServiceCollection services, Action<IftttOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        var builder = new DefaultIftttServiceBuilder(services);

        builder.Services.Configure(setupAction);

        return AddIftttToolkitCore(builder);
    }

    /// <summary>
    /// Extension method to add the <see cref="ITestSetup"/> service implementation to the service collection.
    /// </summary>
    /// <param name="builder">The <see cref="IIftttServiceBuilder"/> instance.</param>
    /// <typeparam name="T">The <see cref="ITestSetup"/> implementation type.</typeparam>
    /// <returns>The <see cref="IIftttServiceBuilder"/> instance.</returns>
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

    /// <summary>
    /// Extension method to configure the IFTTT toolkit in the application.
    /// </summary>
    /// <param name="appBuilder">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="IIftttAppBuilder"/> instance.</returns>
    public static IIftttAppBuilder ConfigureIftttToolkit(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        return new DefaultIftttAppBuilder(appBuilder);
    }

    /// <summary>
    /// Extension method to use the IFTTT service key authentication middleware.
    /// </summary>
    /// <param name="appBuilder">The <see cref="IIftttAppBuilder"/> instance.</param>
    /// <returns>The <see cref="IIftttAppBuilder"/> instance.</returns>
    public static IIftttAppBuilder UseServiceKeyAuthentication(this IIftttAppBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        appBuilder.App.UseMiddleware<ServiceKeyMiddleware>();

        return appBuilder;
    }
}