namespace FCG.Domain._Common.Exceptions;
public class FCGDuplicateException : Exception
{
    public FCGDuplicateException(string entity, string message) : base(message)
    {
        Entity = entity;
    }

    public FCGDuplicateException(Guid key, string entity, string message) : base(message)
    {
        Key = key;
        Entity = entity;
    }

    public Guid? Key { get; private set; }
    public string Entity { get; private set; }
}
