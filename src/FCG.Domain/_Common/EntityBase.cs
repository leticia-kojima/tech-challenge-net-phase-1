namespace FCG.Domain._Common;
public abstract class EntityBase
{
    protected EntityBase() { }

    protected EntityBase(Guid key) => Key = key;

    public Guid Key { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public DateTime? DeletedAt { get; private set; }

    protected void WasUpdated() => UpdatedAt = DateTime.UtcNow;
}
