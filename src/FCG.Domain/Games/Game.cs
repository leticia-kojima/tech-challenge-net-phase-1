using FCG.Domain.Catalogs;

namespace FCG.Domain.Games;
public class Game : EntityBase
{
    private List<GameEvaluation> _evaluations = new();
    private List<GameDownload> _downloads = new();

    protected Game() : base() 
    {
        Title = string.Empty;
        Description = string.Empty;
    }

    public Game(
        Guid key,
        string title,
        string description,
        Guid catalogKey
    ) : base(key)
    {
        Title = title;
        Description = description;
        CatalogKey = catalogKey;
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public Guid CatalogKey { get; private set; }

    public virtual Catalog? Catalog { get; private set; }
    
    public virtual IReadOnlyCollection<GameEvaluation> Evaluations
    {
        get => _evaluations.AsReadOnly();
        private set => _evaluations = value == null ? new() : [.. value];
    }
    
    public virtual IReadOnlyCollection<GameDownload> Downloads
    {
        get => _downloads.AsReadOnly();
        private set => _downloads = value == null ? new() : [.. value];
    }

    public void SetData(string title, string description)
    {
        Title = title;
        Description = description;

        WasUpdated();
    }
}
