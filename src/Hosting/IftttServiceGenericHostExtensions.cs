using System.Reflection;
using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Controllers;
using InvvardDev.Ifttt.Hosting.Middleware;
using InvvardDev.Ifttt.Hosting.Models;
using InvvardDev.Ifttt.Reflection;

namespace InvvardDev.Ifttt.Hosting;

public static class IftttGenericHostingExtensions
{
    public static IWebHostBuilder AddIftttToolkit(this IWebHostBuilder hostBuilder,
                                                  Action<IIftttServiceBuilder, IftttOptions> configureServicesDelegate)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);
        ArgumentNullException.ThrowIfNull(configureServicesDelegate);

        return hostBuilder.ConfigureServices((ctx, services) =>
        {
            var options = new IftttOptions();
            configureServicesDelegate(AddIftttToolkit(services, options), options);
        });
    }

    public static IWebHostBuilder ConfigureIftttToolkit(this IWebHostBuilder hostBuilder,
                                                        Action<IIftttAppBuilder> configureAppDelegate)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);
        ArgumentNullException.ThrowIfNull(configureAppDelegate);
        
        return hostBuilder.Configure((context, applicationBuilder) =>
        {
            configureAppDelegate(new DefaultIftttAppBuilder(applicationBuilder));
        });
    }

    public static IIftttServiceBuilder UseServiceKeyAuthentication(this IIftttServiceBuilder builder, string serviceKey)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(serviceKey);

        builder.ServiceKey = serviceKey;

        return builder;
    }

    public static IIftttAppBuilder UseAuthentication(this IIftttAppBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        appBuilder.App.UseMiddleware<ServiceKeyMiddleware>();

        return appBuilder;
    }

    private static IIftttServiceBuilder AddIftttToolkit(IServiceCollection services, IftttOptions options)
    {
        if (Uri.IsWellFormedUriString(options.RealTimeBaseAddress, UriKind.RelativeOrAbsolute))
        {
            throw new UriFormatException("The RealTimeBaseAddress is not a valid URI.");
        }
        
        var builder = new DefaultIftttServiceBuilder(services, options.ServiceKey, options.RealTimeBaseAddress);

        var apiBuilder = builder.Services.AddControllers();

        apiBuilder.AddApplicationPart(Assembly.GetAssembly(typeof(StatusController)) ?? throw new InvalidOperationException())
                  .AddControllersAsServices();

        builder.Services.AllowResolvingKeyedServicesAsDictionary();
        builder.Services.AddScoped<IAssemblyAccessor, AssemblyAccessor>();

        return builder;
    }
}