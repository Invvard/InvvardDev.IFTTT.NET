namespace InvvardDev.Ifttt.Service.Api.Trigger.Contracts;

public interface IAttributeLookup
{
    IEnumerable<Type> GetAnnotatedTypes();
}