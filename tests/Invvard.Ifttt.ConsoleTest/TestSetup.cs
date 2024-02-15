using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Toolkit.Models;

namespace Invvard.Ifttt.ConsoleTest;

public class TestSetup : ITestSetup
{
    public Task<Samples> PrepareSetupListing()
    {
        var samples = new Samples();
        
        samples.Triggers
               .AddProcessor("nuget_package_updated")
               .AddDataField("nuget_package_updated", "nuget_package_to_watch", "data")
               .AddProcessor("test_new_trigger")
               .AddDataField("test_new_trigger", "test_trigger_field", "test data")
               .AddDataField("test_new_trigger", "second_trigger_field", "another data");

        return Task.FromResult(samples);
    }
}