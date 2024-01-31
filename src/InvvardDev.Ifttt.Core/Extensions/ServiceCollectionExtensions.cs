using InvvardDev.Ifttt.Core.Authentication;
using InvvardDev.Ifttt.Core.Configuration;
using InvvardDev.Ifttt.Core.Contracts;
using InvvardDev.Ifttt.Core.Services;
using Microsoft.Extensions.Options;

namespace InvvardDev.Ifttt.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIftttApiClient(this IServiceCollection services)
    {
        services.AddOptions<IftttOptions>()
                .BindConfiguration(IftttOptions.DefaultSectionName)
                .ValidateDataAnnotations();

#if NET6_0 || NET7_0
        services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.SnakeCaseLower;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
#endif

        services.AddKeyedSingleton<IRepository, ProcessorRepository>(nameof(ProcessorRepository));
        services.AddKeyedSingleton<IRepository, DataFieldsRepository>(nameof(DataFieldsRepository));
        
        return services;
    }

    public static IApplicationBuilder ConfigureIftttApiClient(this WebApplication app)
    {
        var options = app.Services.GetService<IOptions<IftttOptions>>();
        if (!app.Environment.IsDevelopment() || options?.Value.BypassServiceKey is false)
        {
            app.UseMiddleware<ServiceKeyMiddleware>();
        }

        return app;
    }
}