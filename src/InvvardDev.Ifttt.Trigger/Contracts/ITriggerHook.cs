using System.Net;
using InvvardDev.Ifttt.Trigger.Models;

namespace InvvardDev.Ifttt.Trigger.Contracts;

public interface ITriggerHook
{
    Task<HttpStatusCode> SendNotification(ICollection<RealTimeNotificationModel> notificationData);
}
