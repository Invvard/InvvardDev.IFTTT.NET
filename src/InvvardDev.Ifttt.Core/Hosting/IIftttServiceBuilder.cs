namespace InvvardDev.Ifttt.Core.Hosting;

public interface IIftttServiceBuilder
{
    IServiceCollection Services { get; }
    
    string? ServiceKey { get; set; }
    
    string RealTimeBaseAddress { get; }
}