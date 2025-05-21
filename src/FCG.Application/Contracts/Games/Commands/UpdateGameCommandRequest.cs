namespace FCG.Application.Contracts.Games.Commands;
public class UpdateGameCommandRequest : IRequest<UpdateGameCommandResponse>
{
    public Guid Key { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid CatalogKey { get; set; } = Guid.Empty;
}

