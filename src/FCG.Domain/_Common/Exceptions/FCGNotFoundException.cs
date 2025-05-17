namespace FCG.Domain._Common.Exceptions;
public class FCGNotFoundException : Exception
{
    public FCGNotFoundException(Guid key, string entity, string message) : base(message)
    {
        Key = key;
        Entity = entity;
    }

    public Guid Key { get; private set; }
    public string Entity { get; private set; }
}
