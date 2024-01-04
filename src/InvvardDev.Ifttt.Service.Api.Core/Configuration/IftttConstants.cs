namespace InvvardDev.Ifttt.Service.Api.Core.Configuration;

public static class IftttConstants
{
    public static string ServiceKeyHeader => "IFTTT-Service-Key";
    
    public const string BaseTriggersApiPath = "ifttt/v1/triggers";
    
    public static string TriggerHttpClientName => "ifttt-trigger";
}