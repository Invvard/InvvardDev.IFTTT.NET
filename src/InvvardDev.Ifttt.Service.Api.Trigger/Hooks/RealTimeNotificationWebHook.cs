using InvvardDev.Ifttt.Service.Api.Trigger.Models;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Hooks;

public class RealTimeNotificationWebHook : ITriggerHook
{
    public Task SendNotification(IList<RealTimeNotificationModel> notificationData)
    {
        throw new NotImplementedException();
    }
}
