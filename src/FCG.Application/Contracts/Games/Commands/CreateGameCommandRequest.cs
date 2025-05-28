namespace FCG.Application.Contracts.Games.Commands;
public class CreateGameCommandRequest : IRequest<CreateGameCommandResponse>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid CatalogKey { get; set; }
}

