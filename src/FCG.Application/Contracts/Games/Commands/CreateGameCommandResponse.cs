using FCG.Application.Contracts.Games.Abstract;
using FCG.Domain.Games;

namespace FCG.Application.Contracts.Games.Commands;
public class CreateGameCommandResponse : GameCommandModelBase
{
    public CreateGameCommandResponse(Game game) : base(game)
    {
        Key = game.Key;
    }

    public Guid Key { get; set; }
}

