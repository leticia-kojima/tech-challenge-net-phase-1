using FCG.Domain._Common;
using FCG.Domain.Games;

namespace FCG.Domain.Catalogs;
public class Catalog : EntityBase
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public virtual IEnumerable<Game> Games { get; private set; }
}
