using Bogus;
using InvvardDev.Ifttt.Samples.Trigger.Data;
using InvvardDev.Ifttt.Samples.Trigger.Data.Models;
using InvvardDev.Ifttt.Samples.Trigger.Triggers;
using InvvardDev.Ifttt.Toolkit;

namespace InvvardDev.Ifttt.Samples.Trigger.Core;

public class TestSetup(IDataRepository<NugetPackageVersion> nugetRepository) : ITestSetup
{
    public async Task<ProcessorPayload> PrepareSetupListing(CancellationToken cancellationToken)
    {
        var packageNames = (await nugetRepository.GetAll(cancellationToken)).DistinctBy(p => p.PackageName)
                                                                            .Select(p => p.PackageName);

        var processors = new ProcessorPayload { Triggers = new Processors() };

        processors.Triggers
                  .AddProcessor(NugetPackageUpdatedTrigger.TriggerSlug)
                  .AddDataField(NugetPackageUpdatedTrigger.TriggerSlug, "nuget_package_to_watch", data: new Faker().PickRandom(packageNames));

        return processors;
    }
}
