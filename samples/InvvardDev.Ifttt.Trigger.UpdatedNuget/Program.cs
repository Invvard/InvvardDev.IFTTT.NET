using InvvardDev.Ifttt.Hosting;
using InvvardDev.Ifttt.Trigger.UpdatedNuget.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services
       .AddIftttToolkit("<your-service-key>")
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