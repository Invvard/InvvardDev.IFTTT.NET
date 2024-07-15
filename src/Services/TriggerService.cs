using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Models.Core;

namespace InvvardDev.Ifttt.Services;

internal class TriggerService(IProcessorRepository processorRepository, IServiceProvider serviceProvider) : ProcessorService(processorRepository, serviceProvider)
{
    protected override ProcessorKind Kind => ProcessorKind.Trigger;
}
