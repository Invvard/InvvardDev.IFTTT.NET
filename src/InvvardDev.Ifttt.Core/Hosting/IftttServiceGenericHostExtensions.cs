using InvvardDev.Ifttt.Core.Authentication;
using InvvardDev.Ifttt.Shared.Configuration;
using InvvardDev.Ifttt.Shared.Contracts;

namespace InvvardDev.Ifttt.Core.Hosting;

public static class IftttServiceGenericHostExtensions
{
    public static IHostBuilder AddIftttToolkit(this IWebHostBuilder hostBuilder,
                                               Action<WebHostBuilderContext, IIftttServiceBuilder> configureDelegate)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);
        ArgumentNullException.ThrowIfNull(configureDelegate);
        
        return hostBuilder.ConfigureServices((context, services) => configureDelegate(context, AddIftttToolkit(services)));
    }

    private static IIftttServiceBuilder AddIftttToolkit(IServiceCollection services)
    {
#if NET6_0 || NET7_0
        services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.SnakeCaseLower;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
#endif

        services.AllowResolvingKeyedServicesAsDictionary();

        return services;
    }

    public static IApplicationBuilder AddAuthentication(this IWebHostBuilder app)
    {
        app.UseMiddleware<ServiceKeyMiddleware>();

        return app;
    }
}