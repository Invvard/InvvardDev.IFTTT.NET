using System.Net;
using InvvardDev.Ifttt.Models.Trigger;

namespace InvvardDev.Ifttt.Contracts;

public interface ITriggerHook
{
    Task<HttpStatusCode> SendNotification(ICollection<RealTimeNotificationModel> notificationData);
}