using FCG.Domain.Games;

namespace FCG.Domain.Catalogs;
public class Catalog : EntityBase
{
    protected Catalog() : base() { }

    public Catalog(
        Guid key,
        string name,
        string description
    ) : base(key)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }

    public virtual IReadOnlyCollection<Game> Games { get; private set; }
}
