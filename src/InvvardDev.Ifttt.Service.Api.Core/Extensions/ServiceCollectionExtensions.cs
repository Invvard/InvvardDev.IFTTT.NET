using InvvardDev.Ifttt.Service.Api.Core.Authentication;
using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
    
    public static IApplicationBuilder ConfigureIftttApiClient(this IApplicationBuilder app)
    {
        app.UseMiddleware<ServiceKeyMiddleware>();

        return app;
    }
}
