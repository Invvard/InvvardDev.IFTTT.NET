using Invvard.Ifttt.ConsoleTest;
using InvvardDev.Ifttt.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebHost.CreateDefaultBuilder(args);

builder.ConfigureServices((ctx, services) =>
{
    var opt = ctx.Configuration.GetSection(ClientIftttOptions.DefaultSectionName).Get<ClientIftttOptions>();
    builder.AddIftttToolkit((iftttBuilder, options) =>
    {
        iftttBuilder.UseServiceKeyAuthentication(opt!.ServiceKey)
                    .AddTriggers()
                    .AddTriggerAutoMapper();
    });
});

builder.ConfigureIftttToolkit(iftttAppBuilder =>
{
    iftttAppBuilder.UseAuthentication()
                   .ConfigureTriggers();
});

builder.Build().Run();