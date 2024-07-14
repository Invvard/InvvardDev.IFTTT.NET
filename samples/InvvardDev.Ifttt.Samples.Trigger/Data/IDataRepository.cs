namespace InvvardDev.Ifttt.Samples.Trigger.Data;

public interface IDataRepository<TDataModel>
{
    Task<IReadOnlyCollection<TDataModel>> GetAll(CancellationToken cancellationToken = default);
    
    Task<TDataModel?> GetByName(string name, CancellationToken cancellationToken = default);
    
    Task Add(TDataModel data, CancellationToken cancellationToken = default);
}
