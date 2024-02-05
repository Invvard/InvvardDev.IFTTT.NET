using System.Net;

namespace InvvardDev.Ifttt.Trigger.Models.Contracts;

public interface ITriggerHook
{
    Task<HttpStatusCode> SendNotification(ICollection<RealTimeNotificationModel> notificationData);
}
