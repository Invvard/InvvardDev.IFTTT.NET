namespace InvvardDev.Ifttt.Configuration;

public static class IftttConstants
{
    public static string ServiceKeyHeader => "IFTTT-Service-Key";

    private const string BaseApiPath = "ifttt/v1";
    public const string BaseTriggersApiPath = $"{BaseApiPath}/triggers";
    public const string StatusApiPath = $"{BaseApiPath}/status";
    public const string TestingApiPath = $"{BaseApiPath}/test/setup";
    
    public static string TriggerHttpClientName => "ifttt-trigger";
}