using InvvardDev.Ifttt.Configuration;

namespace InvvardDev.Ifttt.Authentication;

public class ServiceKeyMiddleware(RequestDelegate next, string serviceKey)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(IftttConstants.ServiceKeyHeader, out var receivedServiceKey)
            && receivedServiceKey == serviceKey)
        {
            await next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}