using FCG.Application.Contracts.Games.Abstract;
using FCG.Domain.Games;

namespace FCG.Application.Contracts.Games.Commands;
public class UpdateGameCommandResponse : GameCommandModelBase
{
    public UpdateGameCommandResponse(Game game) : base(game)
    {
        Key = game.Key;
    }

    public Guid Key { get; set; }
}

