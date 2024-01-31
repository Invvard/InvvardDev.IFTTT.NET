using InvvardDev.Ifttt.Core.Contracts;

namespace InvvardDev.Ifttt.Core.Services;

public class DataFieldsRepository([FromKeyedServices(nameof(ProcessorRepository))] IRepository processorRepository) : DataRepository
{
    public override void UpsertType(string slug, Type type)
    {
        if (processorRepository.GetDataType(slug) is null)
        {
            throw new InvalidOperationException($"Unable to find a processor with '{slug}' to attach Data Fields to.");
        }

        DataTypes[slug] = type;
    }
}