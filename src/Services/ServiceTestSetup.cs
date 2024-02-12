using InvvardDev.Ifttt.Contracts;

namespace InvvardDev.Ifttt.Services;

public class ServiceTestSetup(IProcessorService processorRepository) : IIftttSetup
{
    public Task<object> GetSetupListing()
    {
        return Task.FromResult<object>(new
        {
            ProcessorRepository = processorRepository
        });
    }
}