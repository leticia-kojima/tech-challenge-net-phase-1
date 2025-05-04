namespace FCG.Domain._Common;
public class FCGValidationException : Exception
{
    public string Field { get; private set; }

    public FCGValidationException(string field, string message) : base(message)
    {
        Field = field;
    }
}
