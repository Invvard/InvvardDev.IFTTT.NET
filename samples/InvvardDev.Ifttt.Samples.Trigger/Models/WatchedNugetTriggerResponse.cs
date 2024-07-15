using InvvardDev.Ifttt.Samples.Trigger.Data.Models;
using InvvardDev.Ifttt.Toolkit;

namespace InvvardDev.Ifttt.Samples.Trigger.Models;

public record WatchedNugetTriggerResponse(string NugetPackageName, string PackageVersion, string ReleaseDateTime, string Id, DateTimeOffset Timestamp) : TriggerResponse(Id, Timestamp)
{
    public static explicit operator WatchedNugetTriggerResponse(NugetPackageVersion version)
    {
        return new WatchedNugetTriggerResponse(version.PackageName,
                                               version.Version,
                                               version.UpdatedDateTime.ToString("o"), // Assuming ISO 8601 format for the date time
                                               version.Id,
                                               TimeProvider.System.GetUtcNow());
    }
}
