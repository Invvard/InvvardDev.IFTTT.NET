namespace InvvardDev.Ifttt.Hosting;

public sealed class DefaultIftttAppBuilder(IApplicationBuilder app) : IIftttAppBuilder
{
    public IApplicationBuilder App { get; } = app;
}