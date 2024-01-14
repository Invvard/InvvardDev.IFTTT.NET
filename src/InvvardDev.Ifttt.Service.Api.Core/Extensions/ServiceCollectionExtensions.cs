using InvvardDev.Ifttt.Service.Api.Core.Authentication;
using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using Microsoft.Extensions.Options;

namespace InvvardDev.Ifttt.Service.Api.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIftttApiClient(this IServiceCollection services)
    {
        services.AddOptions<IftttOptions>()
                .BindConfiguration(IftttOptions.DefaultSectionName)
                .ValidateDataAnnotations();
        
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