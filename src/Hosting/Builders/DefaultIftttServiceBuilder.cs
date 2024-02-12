namespace InvvardDev.Ifttt.Hosting;

public sealed class DefaultIftttServiceBuilder(IServiceCollection services, string? serviceKey, string realTimeBaseAddress) : IIftttServiceBuilder
{
    public IServiceCollection Services { get; } = services;
    
    public string? ServiceKey { get; set; } = serviceKey;

    public string RealTimeBaseAddress { get; } = realTimeBaseAddress;
}