namespace InvvardDev.Ifttt.Core.Hosting;

public sealed class DefaultIftttAppBuilder(IApplicationBuilder app) : IIftttAppBuilder
{
    public IApplicationBuilder App { get; } = app;
}