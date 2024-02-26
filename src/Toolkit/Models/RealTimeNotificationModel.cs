using System.Text.Json.Serialization;

namespace InvvardDev.Ifttt.Toolkit;

/// <summary>
/// Class to hold the real-time notification model for IFTTT.
/// </summary>
public class RealTimeNotificationModel
{
    [JsonConstructor]
    public RealTimeNotificationModel()
    {
    }

    private RealTimeNotificationModel(string? triggerIdentity, string? userId)
    {
        TriggerIdentity = triggerIdentity;
        UserId = userId;
    }

    /// <summary>
    /// Gets or sets the trigger identity impacted by the trigger update.
    /// </summary>
    public string? TriggerIdentity { get; set; }

    /// <summary>
    /// Gets or sets the user identifier impacted by the trigger update.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Creates a new instance of the <see cref="RealTimeNotificationModel"/> with a trigger identity.
    /// </summary>
    /// <remarks>Real-time notification can receive either trigger identities or user identifiers.</remarks>
    /// <param name="triggerIdentity">The trigger identity to add.</param>
    /// <returns>A new real-time notification trigger identity.</returns>
    public static RealTimeNotificationModel CreateTriggerIdentity(string triggerIdentity) => new(triggerIdentity, null);

    /// <summary>
    /// Creates a new instance of the <see cref="RealTimeNotificationModel"/> with a user identifier.
    /// </summary>
    /// <remarks>Real-time notification can receive either trigger identities or user identifiers.</remarks>
    /// <param name="userId">The user identifier to add.</param>
    /// <returns>A new real-time notification user identifier.</returns>
    public static RealTimeNotificationModel CreateUserId(string userId) => new(null, userId);
}