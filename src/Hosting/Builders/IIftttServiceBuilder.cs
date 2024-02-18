namespace InvvardDev.Ifttt.Hosting;

public interface IIftttServiceBuilder
{
    IServiceCollection Services { get; }
    
    string? ServiceKey { get; set; }
    
    string RealTimeBaseAddress { get; }
}