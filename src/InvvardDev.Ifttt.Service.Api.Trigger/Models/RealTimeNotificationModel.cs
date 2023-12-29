namespace InvvardDev.Ifttt.Service.Api.Trigger.Models;

public class RealTimeNotificationModel
{
    private RealTimeNotificationModel(string? triggerIdentity, string? userId)
    {
        TriggerIdentity = triggerIdentity;
        UserId = userId;
    }

    public string? TriggerIdentity { get; }
    public string? UserId { get; }

    public static RealTimeNotificationModel CreateTriggerIdentity(string triggerIdentity) => new(triggerIdentity, null);

    public static RealTimeNotificationModel CreateUserId(string userId) => new(null, userId);
}
