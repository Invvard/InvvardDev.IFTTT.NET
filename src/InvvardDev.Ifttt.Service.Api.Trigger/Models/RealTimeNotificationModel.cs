﻿using System.Text.Json.Serialization;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Models;

public class RealTimeNotificationModel
{
    public RealTimeNotificationModel()
    {
    }

    private RealTimeNotificationModel(string? triggerIdentity, string? userId)
    {
        TriggerIdentity = triggerIdentity;
        UserId = userId;
    }

    public string? TriggerIdentity { get; set; }

    public string? UserId { get; set; }

    public static RealTimeNotificationModel CreateTriggerIdentity(string triggerIdentity) => new(triggerIdentity, null);

    public static RealTimeNotificationModel CreateUserId(string userId) => new(null, userId);
}