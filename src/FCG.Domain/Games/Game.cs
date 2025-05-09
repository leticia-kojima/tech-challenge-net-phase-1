using FCG.Domain._Common;
using FCG.Domain.Catalogs;

namespace FCG.Domain.Games;
public class Game : EntityBase
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Guid CatalogKey { get; private set; }

    public virtual Catalog Catalog { get; private set; }
    public virtual IEnumerable<GameEvaluation> Evaluations { get; private set; }
}
