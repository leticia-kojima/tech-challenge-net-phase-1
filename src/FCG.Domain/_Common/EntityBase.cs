namespace FCG.Domain._Common;
public abstract class EntityBase
{
    protected EntityBase()
    {
        Key = Guid.NewGuid();
        CreatedAt = UpdatedAt = DateTime.UtcNow;
    }

    public Guid Key { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public DateTime DeletedAt { get; private set; }

    public bool IsDeleted => DeletedAt != default;

    public void WasUpdated() => UpdatedAt = DateTime.UtcNow;

    public void WasDeleted() => DeletedAt = DateTime.UtcNow;
}
