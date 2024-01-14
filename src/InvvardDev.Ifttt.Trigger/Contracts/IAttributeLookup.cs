namespace InvvardDev.Ifttt.Trigger.Contracts;

public interface IAttributeLookup
{
    IEnumerable<Type> GetAnnotatedTypes();
}