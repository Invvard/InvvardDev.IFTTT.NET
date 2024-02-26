using Microsoft.Extensions.Options;

namespace InvvardDev.Ifttt.Hosting.Middleware;

/// <summary>
/// Middleware for checking the service key of the incoming request.
/// </summary>
/// <param name="next"></param>
/// <param name="options"></param>
internal class ServiceKeyMiddleware(RequestDelegate next, IOptions<IftttOptions> options)
{
    private readonly string serviceKey = options.Value.ServiceKey ?? throw new ArgumentNullException(nameof(options));

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