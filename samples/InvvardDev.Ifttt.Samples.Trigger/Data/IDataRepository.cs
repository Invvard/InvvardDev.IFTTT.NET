namespace InvvardDev.Ifttt.Samples.Trigger.Data;

public interface IDataRepository<TDataModel>
{
    Task<IReadOnlyCollection<TDataModel>> GetAll(CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<TDataModel>> GetCountByName(string name, int count = 50, CancellationToken cancellationToken = default);
    
    Task Add(TDataModel data, CancellationToken cancellationToken = default);
}
