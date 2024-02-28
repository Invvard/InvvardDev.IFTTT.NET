using System.Net;

namespace InvvardDev.Ifttt.Toolkit;

/// <summary>
/// This interface defines the method signature that has to be called when the trigger endpoint is hit.
/// The default implementation is registered as a transient services in the DI container when adding the trigger.  
/// </summary>
public interface ITriggerHook
{
    /// <summary>
    /// Sends a Real time notification to the IFTTT platform.
    /// It informs the platform that a trigger has been updated and that it should update data for the trigger identities or user ids sent.
    /// </summary>
    /// <param name="notificationData">A list of trigger identity or user id.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The response HTTP Status code received.</returns>
    Task<HttpStatusCode> SendNotification(ICollection<RealTimeNotificationModel> notificationData, CancellationToken cancellationToken);
}