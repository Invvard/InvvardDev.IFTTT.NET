using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using Microsoft.Extensions.Options;

namespace InvvardDev.Ifttt.Service.Api.Core.Authentication;

public class ServiceKeyMiddleware(RequestDelegate next, IOptions<IftttOptions> options)
{
    private readonly string serviceKey = options.Value.ServiceKey;

    public async Task InvokeAsync(HttpContext context)
    {
        IHttpContextAccessor httpContextAccessor = context.RequestServices.GetRequiredService<IHttpContextAccessor>();
        if (context.Request.Headers.TryGetValue(IftttConstants.ServiceKeyHeader, out var receivedServiceKey) && receivedServiceKey == serviceKey)
        {
            await next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}