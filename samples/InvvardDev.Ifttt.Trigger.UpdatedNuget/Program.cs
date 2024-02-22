using InvvardDev.Ifttt.Hosting;
using InvvardDev.Ifttt.Trigger.UpdatedNuget.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services
       .AddIftttToolkit(options => options.ServiceKey = "your-service")
       .AddTestSetupService<TestSetup>()
       .AddTriggerAutoMapper()
       .AddTriggers();

var app = builder.Build();

app.UseRouting();

app.ConfigureIftttToolkit()
   //.UseServiceKeyAuthentication()
   .ConfigureTriggers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();
}

app.Run();