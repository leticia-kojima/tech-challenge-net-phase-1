namespace FCG.Domain._Common.Abstract;
public abstract class EntityBase
{
    protected EntityBase() {
        Key = Guid.NewGuid();
    }

    protected EntityBase(Guid key) => Key = key;

    public Guid Key { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public DateTime? DeletedAt { get; private set; }

    protected void WasUpdated() => UpdatedAt = DateTime.UtcNow;
    
    public void Delete() => DeletedAt = DateTime.UtcNow;
}
