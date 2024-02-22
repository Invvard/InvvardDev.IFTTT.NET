namespace InvvardDev.Ifttt.Hosting;

public sealed class DefaultIftttServiceBuilder(IServiceCollection services) : IIftttServiceBuilder
{
    public IServiceCollection Services { get; } = services;
    
}