using FCG.Application.Contracts.Games.Abstract;

namespace FCG.Application.Contracts.Games.Commands;
public class UpdateGameCommandRequest : GameCommandModelBase, IRequest<UpdateGameCommandResponse>
{
    public Guid Key { get; set; }
    public Guid CatalogKey { get; set; }
}

