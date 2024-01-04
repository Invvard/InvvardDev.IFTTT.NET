using Microsoft.AspNetCore.Builder;


namespace InvvardDev.Ifttt.Service.Api.Trigger;

public static class TriggerExtensions
{
    public static IApplicationBuilder UseTrigger(this IApplicationBuilder app)
    {
        return app;
    }
}