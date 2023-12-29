using InvvardDev.Ifttt.Service.Api.Trigger.Models;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Hooks;

public interface ITriggerHook
{
    Task SendNotification(IList<RealTimeNotificationModel> notificationData);
}
