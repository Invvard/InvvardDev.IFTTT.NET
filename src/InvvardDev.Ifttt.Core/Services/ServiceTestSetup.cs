using InvvardDev.Ifttt.Shared.Contracts;
using InvvardDev.Ifttt.Shared.Services;
using InvvardDev.Ifttt.Core.Contracts;

namespace InvvardDev.Ifttt.Core.Services;

public class ServiceTestSetup(IRepository[] repositories) : IIftttSetup
{
    public Task<object> GetSetupListing()
    {
        if (GetRepository<ProcessorRepository<T>>() is { } processorRepository)
        {
            var processorSlugs = processorRepository.G();
            foreach (var processorSlug in processorSlugs)
            {
            
            }
        }
        

        return Task.FromResult<object>(repositories);
    }

    private IProcessorRepository<T>? GetRepository<T>()
        where T: IProcessorRepository<T>
        => repositories.(nameof(T));
}