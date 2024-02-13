using InvvardDev.Ifttt.Contracts;

namespace InvvardDev.Ifttt.Services;

public class ServiceTestSetup(IProcessorService processorService) : IIftttSetup
{
    public Task<object> GetSetupListing()
    {
        return Task.FromResult<object>(new
        {
            ProcessorRepository = processorService
        });
    }
}