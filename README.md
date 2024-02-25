# IFTTT.NET: Simplifying IFTTT integration for .NET Developers

**IFTTT.NET** is a powerful .NET library designed to streamline your interaction with the **[IFTTT](https://ifttt.com/explore)** platform. Whether you're building web applications, services, or IoT solutions, our toolkit empowers you to seamlessly integrate IFTTT into your projects.

## Why Use IFTTT.NET?

- **Boilerplate-Free**: Say goodbye to repetitive code! Our library abstracts away the complexities of IFTTT integration, allowing you to focus on what matters most.
- **Minimal Effort**: Get started quickly. IFTTT.NET simplifies the process of setting up triggers, actions, and applets.
- **Modern API**: We've designed IFTTT.NET to be intuitive and easy to use.

## Getting Started

> &nbsp;&nbsp; **TL;DR**\
> &nbsp;&nbsp; Go to the [Samples](https://github.com/Invvard/InvvardDev.IFTTT.NET/tree/main/samples) folder and check out the sample projects.

### Setup an IFTTT trigger

1. **Install package**: Add our NuGet package [InvvardDev.Ifttt](https://www.nuget.org/packages/InvvardDev.Ifttt) to your project.
2. **Create a trigger class**: add a class implementing the `ITrigger` interface with the TriggerAttribute on top. This will allow IFTTT.NET to find, recognize and handle your trigger:
   ```csharp
   [Trigger("trigger_slug")]
   public class YourNewTrigger : ITrigger
   {
       [DataField("trigger_field_slug")]
       public string YourTriggerFieldName { get; init; } = default!;
 
       public Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default)
       {
           // Your trigger logic goes here
        
           return Task.CompletedTask;
       }
   }
   ```
   The `TriggerAttribute` is used to define the trigger slug. The `DataFieldAttribute` is used to define the trigger field slug.\
   The `ExecuteAsync` method is the entry point for your trigger logic. It will be called by IFTTT.NET when the trigger is activated.
3. **Add the test setup**: one of the mandatory step for publishing an IFTTT service is to have a test setup endpoint.\
   Just add a class implementing the `ITestSetup` interface.\
   ```csharp
   public class TestSetup : ITestSetup
   {
       public Task<ProcessorPayload> PrepareSetupListing()
       {
           var processors = new ProcessorPayload { Triggers = new Processors() };
   
           processors.Triggers
                     .AddProcessor("trigger_slug")
                     .AddDataField("trigger_slug", "trigger_field_slug", "some_value");
   
           return Task.FromResult(processors);
       }
   }
   ```
   The `PrepareSetupListing` method is used to define the test setup payload. It will be called by IFTTT.NET when the test setup endpoint is called.

4. **Configure startup**: In your application startup logic, configure the IFTTT.NET client:
   ```csharp
   var builder = WebApplication.CreateBuilder(args);
   
   builder.Services
          .AddIftttToolkit("<your-service-key>")
          .AddTestSetupService<TestSetup>()
          .AddTriggerAutoMapper()
          .AddTriggers();
   
   var app = builder.Build();
   
   app.ConfigureIftttToolkit()
      .UseServiceKeyAuthentication()
      .ConfigureTriggers();
   
   app.Run();
   ```
5. **Run your application**: That's it! Your trigger is now ready to be used in IFTTT applets.

If you add Swagger support, you will be able to test your triggers directly from the Swagger UI.\
Next, you can use the real-time notification feature to make your trigger more responsive
## 
