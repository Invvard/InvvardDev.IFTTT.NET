using System.Threading.Tasks;
using InvvardDev.Ifttt.Samples.Trigger.Triggers;
using InvvardDev.Ifttt.Toolkit;

namespace InvvardDev.Ifttt.Samples.Trigger.Core;

public class TestSetup : ITestSetup
{
    public Task<ProcessorPayload> PrepareSetupListing()
    {
        var processors = new ProcessorPayload { Triggers = new Processors() };

        processors.Triggers
                  .AddProcessor(NugetPackageUpdatedTrigger.TriggerSlug)
                  .AddDataField(NugetPackageUpdatedTrigger.TriggerSlug, "nuget_package_to_watch", "Microsoft.Extensions.DependencyInjection")
                  .AddDataField(NugetPackageUpdatedTrigger.TriggerSlug, "updated_version", "1.2.3")
                  .AddDataField(NugetPackageUpdatedTrigger.TriggerSlug, "updated_date", DateTime.Now.ToLongDateString());

        return Task.FromResult(processors);
    }
}