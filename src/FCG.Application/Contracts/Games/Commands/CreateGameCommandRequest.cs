using FCG.Application.Contracts.Games.Abstract;

namespace FCG.Application.Contracts.Games.Commands;
public class CreateGameCommandRequest : GameCommandModelBase, IRequest<CreateGameCommandResponse>
{
    public Guid CatalogKey { get; set; }
}

