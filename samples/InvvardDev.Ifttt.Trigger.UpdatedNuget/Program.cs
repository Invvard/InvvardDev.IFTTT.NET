using InvvardDev.Ifttt.Hosting;
using InvvardDev.Ifttt.Trigger.UpdatedNuget.Core;
using InvvardDev.Ifttt.Trigger.UpdatedNuget.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var clientIftttOptions = builder.Configuration.GetSection(ClientIftttOptions.DefaultSectionName).Get<ClientIftttOptions>();

builder.Services.AddSwaggerGen(options => options.AddIftttSecurityKeyScheme());

builder.Services
       .AddIftttToolkit(clientIftttOptions.ServiceKey)
       .AddTestSetupService<TestSetup>()
       .AddTriggerAutoMapper()
       .AddTriggers();

var app = builder.Build();

app.UseRouting();

app.ConfigureIftttToolkit()
   .UseServiceKeyAuthentication()
   .ConfigureTriggers();

app.UseSwagger()
   .UseSwaggerUI();

app.Run();