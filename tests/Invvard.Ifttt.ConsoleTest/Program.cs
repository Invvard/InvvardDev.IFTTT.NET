// See https://aka.ms/new-console-template for more information

using Invvard.Ifttt.ConsoleTest;
using InvvardDev.Ifttt.Core.Hosting;
using InvvardDev.Ifttt.Trigger;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebHost.CreateDefaultBuilder(args);

builder.AddIftttToolkit((ctx, serviceBuilder) =>
{
    var iftttConfiguration = ctx.Configuration.GetSection(IftttOptions.DefaultSectionName).Get<IftttOptions>()!;

    if (!iftttConfiguration.BypassServiceKey)
    {
        serviceBuilder.AddAuthentication(iftttConfiguration.ServiceKey);
    }
    serviceBuilder.AddTriggers();
});