using System.Net;
using InvvardDev.Ifttt.Service.Api.Trigger.Models;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Contracts;

public interface ITriggerHook
{
    Task<HttpStatusCode> SendNotification(ICollection<RealTimeNotificationModel> notificationData);
}
