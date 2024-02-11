using System.Net;
using InvvardDev.Ifttt.Toolkit.Models;

namespace InvvardDev.Ifttt.Toolkit.Contracts;

public interface ITriggerHook
{
    Task<HttpStatusCode> SendNotification(ICollection<RealTimeNotificationModel> notificationData);
}