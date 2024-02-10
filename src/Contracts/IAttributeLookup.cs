namespace InvvardDev.Ifttt.Contracts;

public interface IAttributeLookup
{
    IEnumerable<Type> GetAnnotatedTypes();
}