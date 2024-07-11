using InvvardDev.Ifttt.Hosting;
using InvvardDev.Ifttt.Samples.Trigger.Core;
using InvvardDev.Ifttt.Samples.Trigger.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var clientIftttOptions = builder.Configuration.GetSection(ClientIftttOptions.DefaultSectionName).Get<ClientIftttOptions>();

builder.Services
       .AddHttpLogging(logging =>
       {
           logging.LoggingFields = HttpLoggingFields.All;
           
           logging.MediaTypeOptions.AddText("application/x-www-form-urlencoded");
           logging.MediaTypeOptions.AddText("multipart/form-data");
           
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

app.UseHttpLogging()
   .UseRouting()
   .UseSwagger()
   .UseSwaggerUI();

app.ConfigureIftttToolkit()
   .UseServiceKeyAuthentication(clientIftttOptions.BypassServiceKey)
   .ConfigureTriggers();

await app.RunAsync();
