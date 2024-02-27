# IFTTT.NET: Simplifying IFTTT integration for .NET Developers

**IFTTT.NET** is a powerful .NET library designed to streamline your interaction with the **[IFTTT](https://ifttt.com/explore)** platform. Whether you're building web applications, services, or IoT solutions, our toolkit empowers you to seamlessly integrate IFTTT into your projects.

## Why Use IFTTT.NET?

- **Boilerplate-Free**: Say goodbye to repetitive code! Our library abstracts away the complexities of IFTTT integration, allowing you to focus on what matters most.
- **Minimal Effort**: Get started quickly. IFTTT.NET simplifies the process of setting up triggers, actions, and applets.
- **Modern API**: We've designed IFTTT.NET to be intuitive and easy to use.

## Getting Started

> &nbsp;&nbsp; **TL;DR**\
> &nbsp;&nbsp; Go to the [Samples](https://github.com/Invvard/InvvardDev.IFTTT.NET/tree/main/samples) folder and check out the sample projects.

### Setup an IFTTT Trigger

1. **Install package**: Add our NuGet package [InvvardDev.Ifttt](https://www.nuget.org/packages/InvvardDev.Ifttt) to your project.
2. **Create a Trigger class**: add a class implementing the `ITrigger` interface with the TriggerAttribute on top. This will allow IFTTT.NET to find, recognize and handle your Trigger:

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

   The `TriggerAttribute` is used to define the Trigger slug. The `DataFieldAttribute` is used to define the Trigger field slug.\
   The `ExecuteAsync` method is the entry point for your Trigger logic. It will be called by IFTTT.NET when the Trigger is activated.
3. **Add the test setup**: one of the mandatory step for publishing an IFTTT service is to have a test setup endpoint.\
   Just add a class implementing the `ITestSetup` interface.

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

   The `PrepareSetupListing` method is used to define the test setup payload. It will be called by IFTTT.NET when the test setup endpoint is called.\
   You can find the complete list of required Triggers, actions and queries in your service dashboard, under the `API` tab, `Endpoint tests` section, `Test setup` tab and click on either "**View** or **Download** a scaffold JSON response for your service".

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

5. **Run your application**: That's it! Your Trigger is now ready to be used in IFTTT Applets.

### Swagger support

Swagger is a good way to test your Triggers and Actions. Just add the `AddSwaggerGen` and `UseSwagger` methods to your application startup logic (see step 4).

```csharp
builder.Services.AddSwaggerGen();
[...]
app.UseSwagger().UseSwaggerUI();
```

In the event that you want to run Swagger with the Service Key Authentication activated, configure SwaggerGen like this:

```csharp
builder.Services.AddSwaggerGen(options => options.AddIftttServiceKeyScheme());
```

It will add a new security scheme to the Swagger UI, allowing you to test your Triggers and actions with the Service Key Authentication.

## Real time notifications

Next, you can use the real time notification feature to make your Trigger more responsive.\
When you have a new data to send to IFTTT, prepare your Trigger identities or User IDs and call `ITriggerHook.SendNotification`.\
Here is an example of a controller sending a notification to IFTTT with Trigger identities:

```csharp
[ApiController]
[Route("api/[controller]")]
public class RealTimeNotificationController(ITriggerHook realTimeHook) : ControllerBase
{
    [HttpPost]
    public async Task NotifyAsync(CancellationToken cancellationToken = default)
    {
        var notificationRequest = new List<RealTimeNotificationModel>
                                  {
                                      RealTimeNotificationModel.CreateTriggerIdentity("trigger_identity_12345"),
                                      RealTimeNotificationModel.CreateTriggerIdentity("trigger_identity_67890"),
                                  };
        
        await realTimeHook.SendNotification(notificationRequest, cancellationToken);
    }
}
```

If you use User IDs, just call `RealTimeNotificationModel.CreateUserId("<user_id>")` instead of `RealTimeNotificationModel.CreateTriggerIdentity("<trigger_identity>")`.

## Roadmap

As of today, the library is still in its early stages and only supports Triggers, basic Data field and Service Key Authentication. We are working on adding support for Data field validation, User Authenticated integration and other advanced features .\
Fortunately, IFTTT is designed in such a way that once Triggers are implemented, Actions and Queries should be easy to implement.
