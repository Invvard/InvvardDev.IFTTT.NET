using InvvardDev.Ifttt.Hosting.Models;

namespace InvvardDev.Ifttt.Hosting.Middleware;

internal class ServiceKeyMiddleware(RequestDelegate next, string serviceKey)
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