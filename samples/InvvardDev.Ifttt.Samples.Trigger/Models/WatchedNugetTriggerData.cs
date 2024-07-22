using InvvardDev.Ifttt.Samples.Trigger.Data.Models;
using InvvardDev.Ifttt.Toolkit;

namespace InvvardDev.Ifttt.Samples.Trigger.Models;

public record WatchedNugetTriggerData(string NugetPackageName, string PackageVersion, string ReleaseDateTime, string Id, DateTimeOffset Timestamp) : TriggerData(Id, Timestamp)
{
    public static explicit operator WatchedNugetTriggerData(NugetPackageVersion version)
        => new(version.PackageName,
               version.Version,
               version.UpdatedDateTime.ToString("o"), // Assuming ISO 8601 format for the date time
               version.Id,
               version.UpdatedDateTime);
}
