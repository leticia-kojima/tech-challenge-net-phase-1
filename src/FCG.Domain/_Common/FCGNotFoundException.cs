namespace FCG.Domain._Common;
public class FCGNotFoundException : Exception
{
    public string Entity { get; private set; }

    public FCGNotFoundException(string entity, string message) : base(message)
    {
        Entity = entity;
    }
}
