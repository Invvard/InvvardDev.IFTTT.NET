namespace InvvardDev.Ifttt.Contracts;

internal interface IAttributeLookup
{
    IEnumerable<Type> GetAnnotatedTypes();
}