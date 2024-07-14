
using InvvardDev.Ifttt.Toolkit;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Samples.Trigger.Triggers;

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
