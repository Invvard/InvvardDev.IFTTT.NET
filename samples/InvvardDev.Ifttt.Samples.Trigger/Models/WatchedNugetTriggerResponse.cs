using InvvardDev.Ifttt.Toolkit;

namespace InvvardDev.Ifttt.Samples.Trigger.Models;

public record WatchedNugetTriggerResponse(string NugetPackageName, string PackageVersion, string ReleaseDateTime, string Id, DateTimeOffset Timestamp) : TriggerResponse(Id, Timestamp);
