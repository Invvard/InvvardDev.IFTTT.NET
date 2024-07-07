using InvvardDev.Ifttt.Hosting;
using InvvardDev.Ifttt.Samples.Trigger.Core;
using InvvardDev.Ifttt.Samples.Trigger.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var clientIftttOptions = builder.Configuration.GetSection(ClientIftttOptions.DefaultSectionName).Get<ClientIftttOptions>();

builder.Services
       .AddHttpLogging(logging =>
       {
           logging.RequestHeaders.Add("IFTTT-Test-Mode");
           logging.RequestHeaders.Add("x-datadog-trace-id");
           logging.RequestHeaders.Add("x-datadog-tags");
           logging.RequestHeaders.Add("traceparent");
       })
       .AddApplicationInsightsTelemetry()
       .AddSwaggerGen(options => options.AddIftttServiceKeyScheme());

builder.Services
       .AddIftttToolkit(clientIftttOptions.ServiceKey)
       .AddTestSetupService<TestSetup>()
       .AddTriggerAutoMapper()
       .AddTriggers();

var app = builder.Build();

app.UseRouting();

app.UseHttpLogging();

app.ConfigureIftttToolkit()
   .UseServiceKeyAuthentication()
   .ConfigureTriggers();

app.UseSwagger()
   .UseSwaggerUI();

await app.RunAsync();
