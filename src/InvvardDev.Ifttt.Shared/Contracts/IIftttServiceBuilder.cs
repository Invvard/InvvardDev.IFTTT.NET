using Microsoft.Extensions.DependencyInjection;

namespace InvvardDev.Ifttt.Shared.Contracts;

public interface IIftttServiceBuilder
{
    IServiceCollection Services { get; }
}