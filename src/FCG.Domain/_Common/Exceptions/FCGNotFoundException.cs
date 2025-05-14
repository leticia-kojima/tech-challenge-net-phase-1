namespace FCG.Domain._Common.Exceptions;
public class FCGNotFoundException : Exception
{
    public string Entity { get; private set; }

    public FCGNotFoundException(string entity, string message) : base(message)
    {
        Entity = entity;
    }
}
